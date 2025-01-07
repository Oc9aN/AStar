using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Events;

public class UserManager : MonoBehaviour
{
    public event UnityAction<int> OnMoneyChanged;
    private int money = 100;

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
