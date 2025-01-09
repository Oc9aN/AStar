using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class StageData
{
    public List<Level> levels;
}

[Serializable]
public class Level
{
    public List<Wave> waves;
}

[Serializable]
public class Wave
{
    public int wave;
    public List<Monster> monsters;
    public int waveDelay;
}

[Serializable]
public class Monster
{
    public string type;
    public int delay;
}