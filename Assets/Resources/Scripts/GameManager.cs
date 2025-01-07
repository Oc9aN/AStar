using System.Collections;
using System.Collections.Generic;
using TowerSystem;
using UnitSystem;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    // 이벤트
    private event UnityAction<TypeTierData> OnCreateTower;
    private event UnityAction<string> OnCreateUnit;

    // JSON 데이터
    private StageData stageData;
    private TypeTierData typeTierData;

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

    // 하위 매니저로 이벤트 초기화
    private void InitManagers()
    {
        var uiManager = GetComponentInChildren<UIManager>();

        var userManager = GetComponentInChildren<UserManager>();

        var randomManager = GetComponentInChildren<RandomManager>();

        var unitManager = GetComponentInChildren<UnitManager>();

        var towerManager = GetComponentInChildren<TowerManager>();

        // 상호 이벤트 연결
        uiManager.SubscribeMoneyUpdates(userManager);   // user의 돈 변화를 감지
        userManager.SubscribeMoneyAdd(unitManager);     // Unit이 사망하면 ADD 이벤트 등록
        userManager.SubscribeMoneyUse(towerManager);

        OnCreateTower += towerManager.CreateTowerByRandom;
        OnCreateUnit += unitManager.CreateUnit;
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
                OnCreateUnit?.Invoke(monster.type);
            }
            yield return new WaitForSeconds(wave.waveDelay);
        }
        yield break;
    }
    #endregion

    #region Tower
    public void CreateTower()
    {
        // 랜덤으로 타워 생성, 타입과 티어로 구별, 티어는 0~4까지, 티어가 낮을수록 강한것
        OnCreateTower?.Invoke(typeTierData);
    }
    #endregion
}
