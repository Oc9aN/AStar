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
public class UserManager : MonoBehaviour
{
    public event UnityAction<int> OnMoneyChanged;
    private int money = 100;

    public void SubscribeMoneyAdd(IAddableMoney addable)
    {
        addable.OnAddMoney += DepositMoneyEvent;
    }

    public void SubscribeMoneyUse(ISpendableMoney spendable)
    {
        spendable.OnSpendMoney += WithdrawMoneyEvent;
    }

    private void Start()
    {
        OnMoneyChanged?.Invoke(money);
    }

    /// <summary>
    /// 출금
    /// </summary>
    /// <param name="value">금액</param>
    public bool WithdrawMoneyEvent(int value)
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
    public void DepositMoneyEvent(int value)
    {
        money += value;
        OnMoneyChanged?.Invoke(money);
    }
}
