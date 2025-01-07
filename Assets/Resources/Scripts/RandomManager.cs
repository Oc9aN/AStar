using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomManager : MonoBehaviour
{
    // 티어별 확률에 맞게 랜덤 타워 생성
    public static (string, int) RandomTower(string[] towerTypes, float[] tierPercent)
    {
        float percent = Random.Range(0.0f, 100.0f);
        int type = Random.Range(0, towerTypes.Length);
        int tier = System.Array.FindIndex(tierPercent, v => v > percent);
        return (towerTypes[type], tier);
    }
}
