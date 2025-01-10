using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Tower Data", menuName = "Scriptable Object/Tower Data", order = int.MaxValue)]
public class TowerData : ScriptableObject
{
    [SerializeField] private TowerType type;
    public TowerType Type { get { return type; } }
    [SerializeField] private int damage;
    public int Damage { get { return damage; } }
    [SerializeField] private float delay;
    public float Delay { get { return delay; } }
    [SerializeField] private float radius;
    public float Radius { get { return radius; } }
}

public enum TowerType
{
    NORMAL,
}