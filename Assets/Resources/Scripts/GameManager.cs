using System.Collections;
using System.Collections.Generic;
using UnitSystem;
using UnityEngine;

public interface IGameManager
{
    public bool TrySpendMoney(int value);
    public void AddMoney(int value);
    public void UpdateMoneyUI(int value);
}

public class GameManager : MonoBehaviour, IGameManager
{
    private UnitManager unitManager;
    private UserManager userManager;
    private UIManager uIManager;
    private StageData stageData;

    private void Awake()
    {
        // 하위 매니저 초기화
        uIManager = GetComponentInChildren<UIManager>();

        unitManager = GetComponentInChildren<UnitManager>();
        unitManager.Init(this);

        userManager = GetComponentInChildren<UserManager>();
        userManager.Init(this);
    }

    private void Start()
    {
        // JSON 문자열을 클래스로 변환
        TextAsset jsonFile = Resources.Load<TextAsset>("JSON/stage1");
        stageData = JsonUtility.FromJson<StageData>(jsonFile.text);
    }

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

    public void UpdateMoneyUI(int value) => uIManager.UpdateMoneyUI(value);
    #endregion

    [ContextMenu("StartWave")]
    public void StartWaveCoroutine()
    {
        StartCoroutine(StartWave());
    }
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
}
