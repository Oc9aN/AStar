using System.Collections;
using System.Collections.Generic;
using System.Linq;
using MapSystem;
using TowerSystem;
using UnitSystem;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public interface IGameStateChanger
{
    public GameState GameState { get; set; }
    public List<IGameStateListener> Listeners { get; set; }
    public void AddListener(IGameStateListener listener);
    public void RemoveListener(IGameStateListener listener);
    public void NotifyListener();
}
public interface IGameStateListener
{
    public void UpdateGameState(GameState state);
}

public enum GameState
{
    READY,
    ON_WAVE,
    END_WAVE,
}
public class GameManager : MonoBehaviour, IGameStateChanger
{
    // 이벤트
    private event UnityAction<TypeTierData> TryCreateTower;
    private event UnityAction<string> OnCreateUnit;

    // JSON 데이터
    private StageData stageData;
    private TypeTierData typeTierData;

    // 버튼 -> 스크립트로 이벤트 추가, 한번에 버튼 이벤트를 확인하기 위함
    [SerializeField] Button createTowerButton;
    [SerializeField] Button levelStartButton;

    // 상태 변수
    private bool isPlayable;    // 경로가 존재하여 플레이가 가능한 상태
    [SerializeField, ReadOnly] private GameState gameState = GameState.READY;
    public GameState GameState { get { return gameState; } set { gameState = value; NotifyListener(); } }

    private List<IGameStateListener> listeners = new();
    public List<IGameStateListener> Listeners { get { return listeners; } set { listeners = value; } }

    #region Init
    private void Awake()
    {
        InitManagers();
        AddButtonEvent();
        InitJsonData();
    }

    // 하위 매니저로 이벤트 초기화
    private void InitManagers()
    {
        var uiManager = GetComponentInChildren<UIManager>();

        var userManager = GetComponentInChildren<UserManager>();

        var mapManager = GetComponentInChildren<MapManager>();

        var randomManager = GetComponentInChildren<RandomManager>();

        var unitManager = GetComponentInChildren<UnitManager>();

        var towerManager = GetComponentInChildren<TowerManager>();

        // 상호 이벤트 연결
        uiManager.SubscribeUserDataUpdate(userManager);     // user의 데이터 변화를 감지
        userManager.SubscribeMoneyAdd(unitManager);         // unit이 user의 정보를 변경
        userManager.SubscribeHpDamaged(unitManager);
        userManager.SubscribeMoneyUse(towerManager);        // tower가 user의 정보를 변경

        mapManager.OnCreateMap += AddListener;              // 리스너로 추가
        mapManager.OnCreateMap += (_) => NotifyListener();
        towerManager.OnCreateTower += AddListener;
        towerManager.OnCreateTower += (_) => NotifyListener();

        mapManager.OnFindPath += (List<Vector3> path) => unitManager.path = path;   // unit이 이동할 path 전달
        mapManager.OnFindPath += (List<Vector3> path) => isPlayable = path != null; // path가 null이 아니면 경로가 존재 = 플레이 가능

        unitManager.OnRemoveAllUnitEvent += () => { if (GameState == GameState.END_WAVE) GameState = GameState.READY; };  // Ready로 변경

        TryCreateTower += towerManager.TryCreateTowerByRandom;
        OnCreateUnit += unitManager.CreateUnit;
    }

    // 버튼 이벤트 추가
    private void AddButtonEvent()
    {
        createTowerButton.onClick.AddListener(CreateTower);
        levelStartButton.onClick.AddListener(() =>
        {
            if (isPlayable && GameState == GameState.READY)
            {
                GameState = GameState.ON_WAVE;
                StartCoroutine(StartWave());
            }
        });
    }

    // JSON데이터 읽기
    private void InitJsonData()
    {
        // JSON 문자열을 클래스로 변환
        TextAsset jsonFile = Resources.Load<TextAsset>("JSON/stage1");
        stageData = JsonUtility.FromJson<StageData>(jsonFile.text);

        jsonFile = Resources.Load<TextAsset>("JSON/TypeTier");
        typeTierData = JsonUtility.FromJson<TypeTierData>(jsonFile.text);
    }
    #endregion

    #region Level
    private IEnumerator StartWave()
    {
        // 경로가 있어야 함.
        foreach (var wave in stageData.waves)
        {
            Debug.Log($"웨이브 시작: {wave.wave}");
            foreach (var monster in wave.monsters)
            {
                yield return new WaitForSeconds(monster.delay);
                Debug.Log($"생성 -> Type: {monster.type}");
                OnCreateUnit?.Invoke(monster.type);
            }
            yield return new WaitForSeconds(wave.waveDelay);
        }
        GameState = GameState.END_WAVE;
        yield break;
    }
    #endregion

    #region Tower
    public void CreateTower()
    {
        // 랜덤으로 타워 생성, 타입과 티어로 구별, 티어는 0~4까지, 티어가 낮을수록 강한것
        TryCreateTower?.Invoke(typeTierData);
    }
    #endregion

    #region State

    public void AddListener(GameObject[] objArray)
    {
        IGameStateListener[] listenerArray = objArray.Select(n => n.GetComponent<IGameStateListener>()).ToArray();
        Listeners.AddRange(listenerArray);
    }

    public void AddListener(GameObject obj)
    {
        IGameStateListener listenerArray = obj.GetComponent<IGameStateListener>();
        AddListener(listenerArray);
    }

    public void AddListener(IGameStateListener listener)
    {
        Listeners.Add(listener);
    }

    public void RemoveListener(IGameStateListener listener)
    {
        Listeners.Remove(listener);
    }

    public void NotifyListener()
    {
        Listeners.ForEach(n => n.UpdateGameState(GameState));
    }
    #endregion
}
