using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Events;

public class UserManager : MonoBehaviour
{
    private int money = 100;
    private IGameManager gameManager;
    public IGameManager GameManager
    {
        get { return gameManager; }
    }

    public void Init(IGameManager gameManager)
    {
        this.gameManager = gameManager;
        // MoneyUI 초기화
        this.gameManager.UpdateMoneyUI(money);
    }

    /// <summary>
    /// 출금
    /// </summary>
    /// <param name="value">금액</param>
    public bool WithdrawMoneyEvent(int value)
    {
        if (money - value > 0)
        {
            money -= value;
            gameManager.UpdateMoneyUI(money);
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
        gameManager.UpdateMoneyUI(money);
    }
}
