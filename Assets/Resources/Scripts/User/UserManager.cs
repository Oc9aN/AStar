using System;
using System.Collections;
using System.Collections.Generic;
using UnitSystem;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Events;

public interface ISpendableMoney
{
    public event Func<int, bool> OnSpendMoney;
}
public interface IAddableMoney
{
    public event UnityAction<int> OnAddMoney;
}
public interface ICausingDamage
{
    public event UnityAction<int> CausingDamage;
}
public class UserManager : MonoBehaviour
{
    public event UnityAction<int> OnMoneyChanged;
    public event UnityAction<int, int> OnHpChanged;
    public event UnityAction<int> OnLevelChanged;
    private const int MAX_HP = 100;
    private int money = 100;
    private int hp = MAX_HP;    // 현재 체력
    private int level = 0;
    public void SubscribeMoneyAdd(IAddableMoney addable)
    {
        addable.OnAddMoney += DepositMoneyEvent;
    }

    public void SubscribeMoneyUse(ISpendableMoney spendable)
    {
        spendable.OnSpendMoney += WithdrawMoneyEvent;
    }

    public void SubscribeHpDamaged(ICausingDamage causingDamage)
    {
        causingDamage.CausingDamage += HpDamaged;
    }

    private void Start()
    {
        OnMoneyChanged?.Invoke(money);
        OnHpChanged?.Invoke(hp, MAX_HP);
        OnLevelChanged?.Invoke(level);
    }

    /// <summary>
    /// 출금
    /// </summary>
    /// <param name="value">금액</param>
    private bool WithdrawMoneyEvent(int value)
    {
        if (money - value >= 0)
        {
            money -= value;
            OnMoneyChanged?.Invoke(money);
            return true;
        }
        else
        {
            return false;
        }
    }

    /// <summary>
    /// 입금
    /// </summary>
    /// <param name="value">금액</param>
    private void DepositMoneyEvent(int value)
    {
        money += value;
        OnMoneyChanged?.Invoke(money);
    }

    private void HpDamaged(int value)
    {
        hp -= value;
        OnHpChanged?.Invoke(hp, MAX_HP);
    }

    public void LevelUp()
    {
        level++;
        OnLevelChanged?.Invoke(level);
    }

    public int GetLevel() => level;

    private void OnDestroy()
    {
        OnMoneyChanged = null;
        OnHpChanged = null;
    }
}
