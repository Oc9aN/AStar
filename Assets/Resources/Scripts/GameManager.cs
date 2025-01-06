using System.Collections;
using System.Collections.Generic;
using UnitSystem;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] UnitManager unitManager;

    private StageData stageData;

    private void Start()
    {
        // JSON 문자열을 클래스로 변환
        TextAsset jsonFile = Resources.Load<TextAsset>("JSON/stage1");
        stageData = JsonUtility.FromJson<StageData>(jsonFile.text);
    }

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
