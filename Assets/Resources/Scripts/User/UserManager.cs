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
    public event UnityAction<int, int> OnSizeChanged;
    public event UnityAction<int, int> OnStartChanged;
    public event UnityAction<int, int> OnDestChanged;
    private const int MAX_HP = 100;
    private int money = 100;
    private int hp = MAX_HP;    // 현재 체력
    private int xSize;    // 맵 크기
    private int ySize;
    private int xStart;       // 시작 위치
    private int yStart;
    private int xDest;        // 끝 위치
    private int yDest;
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

    public void HpDamaged(int value)
    {
        hp -= value;
        OnHpChanged?.Invoke(hp, MAX_HP);
    }

    public void SetMapSize(int x, int y)
    {
        xSize = x;
        ySize = y;
        OnSizeChanged?.Invoke(xSize, ySize);
    }

    public void SetStartPosition(int x, int y)
    {
        xStart = x;
        yStart = y;
        OnStartChanged?.Invoke(xStart, yStart);
    }

    public void SetDestPosition(int x, int y)
    {
        xDest = x;
        yDest = y;
        OnDestChanged?.Invoke(xDest, yDest);
    }

    private void OnDestroy()
    {
        OnMoneyChanged = null;
        OnHpChanged = null;
    }
}
