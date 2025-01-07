using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] TMP_Text moneyUI;
    public void SubscribeMoneyUpdates(UserManager userManager)
    {
        userManager.OnMoneyChanged += UpdateMoneyUI;
    }

    public void UpdateMoneyUI(int value)
    {
        // MoneyUI 업데이트
        moneyUI.text = value.ToString();
    }
}
