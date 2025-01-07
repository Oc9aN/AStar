using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class RandomManager
{
    // int값 랜덤
    // 타워 랜덤
    public static int RandomInt(int min, int max)
    {
        return Random.Range(min, max);
    }

    public static (string, int) RandomTower(string[] towerTypes, float[] tierPercent)
    {
        // 티어별 확률에 맞게 랜덤 타워 생성
        float percent = Random.Range(0.0f, 100.0f);
        int type = Random.Range(0, towerTypes.Length);
        int tier = System.Array.FindIndex(tierPercent, v => v > percent);
        return (towerTypes[type], tier);
    }
}
