using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class StageData
{
    public int stage; // 스테이지 번호
    public List<Wave> waves; // 웨이브 리스트
}

[Serializable]
public class Wave
{
    public int wave; // 웨이브 번호
    public List<Monster> monsters; // 몬스터 리스트
    public float waveDelay; // 웨이브 딜레이 (초 단위)
}

[Serializable]
public class Monster
{
    public string type; // 몬스터 타입
    public float delay; // 딜레이 (초 단위)
}