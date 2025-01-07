using UnityEngine;

[CreateAssetMenu(fileName = "Unit Data", menuName = "Scriptable Object/Unit Data", order = int.MaxValue)]
public class UnitData : ScriptableObject
{
    [SerializeField] private int hp;
    public int Hp { get { return hp; } }
    [SerializeField] private int damage;
    public int Damage { get { return damage; } }
    [SerializeField] private float moveSpeed;
    public float MoveSpeed { get { return moveSpeed; } }
    [SerializeField] private int rewardMoney;
    public int RewardMoney { get { return rewardMoney; } }
}