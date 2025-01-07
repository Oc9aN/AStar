using System.Collections;
using System.Collections.Generic;
using TowerSystem;
using UnitSystem;
using UnityEngine;
using UnityEngine.UI;

public interface IGameManager
{
    public bool TrySpendMoney(int value);
    public void AddMoney(int value);
}

public class GameManager : MonoBehaviour, IGameManager
{
    // 하위 매니저
    private UnitManager unitManager;
    private UserManager userManager;
    private UIManager uiManager;
    private TowerManager towerManager;

    // JSON 데이터
    private StageData stageData;
    private TypeTierData typeTierData;

    // 게임 데이터
    private int towerLevel = 0;

    // 버튼 -> 스크립트로 이벤트 추가, 한번에 버튼 이벤트를 확인하기 위함
    [SerializeField] Button createTowerButton;
    [SerializeField] Button levelStartButton;

    #region Init
    private void Awake()
    {
        InitManagers();
        AddButtonEvent();
        InitJsonData();
    }

    // 하위 매니저 초기화
    private void InitManagers()
    {
        uiManager = GetComponentInChildren<UIManager>();

        userManager = GetComponentInChildren<UserManager>();

        unitManager = GetComponentInChildren<UnitManager>();
        unitManager.Init(this);

        towerManager = GetComponentInChildren<TowerManager>();
        towerManager.Init(this);

        // 상호 이벤트 연결
        uiManager.SubscribeMoneyUpdates(userManager);   // user의 돈 변화를 감지
    }

    // 버튼 이벤트 추가
    private void AddButtonEvent()
    {
        createTowerButton.onClick.AddListener(CreateTower);
        levelStartButton.onClick.AddListener(() => StartCoroutine(StartWave()));
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
        foreach (var wave in stageData.waves)
        {
            Debug.Log($"웨이브 시작: {wave.wave}");
            foreach (var monster in wave.monsters)
            {
                yield return new WaitForSeconds(monster.delay);
                Debug.Log($"생성 -> Type: {monster.type}");
                unitManager.CreateUnit(monster.type);
            }
            yield return new WaitForSeconds(wave.waveDelay);
        }
        yield break;
    }
    #endregion

    #region Money
    public bool TrySpendMoney(int value)
    {
        // 돈 출금
        return userManager.WithdrawMoneyEvent(value);
    }

    public void AddMoney(int value)
    {
        // 돈 입금
        userManager.DepositMoneyEvent(value);
    }

    public void UpdateMoneyUI(int value) => uiManager.UpdateMoneyUI(value);
    #endregion

    #region Tower
    public void CreateTower()
    {
        // 랜덤으로 타워 생성, 타입과 티어로 구별, 티어는 0~4까지, 티어가 낮을수록 강한것
        (string type, int tier) = RandomManager.RandomTower(typeTierData.type, typeTierData.tierPercent[towerLevel].percent);
        towerManager.CreateTower("type", tier);
    }
    #endregion
}
