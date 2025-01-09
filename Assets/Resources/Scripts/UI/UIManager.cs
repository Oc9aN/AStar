using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] TMP_Text hpUI;         // 현재 체력
    [SerializeField] TMP_Text moneyUI;      // 보유 금액
    [SerializeField] TMP_Text levelUI;      // 현재 레벨
    public void SubscribeUserDataUpdate(UserManager userManager)
    {
        userManager.OnMoneyChanged += UpdateMoneyUI;
        userManager.OnHpChanged += UpdateHpUI;
        userManager.OnLevelChanged += UpdateLevelUI;
    }

    private void UpdateHpUI(int hp, int maxHP)
    {
        hpUI.text = $"{hp} / {maxHP}";
    }

    private void UpdateMoneyUI(int money)
    {
        moneyUI.text = money.ToString();
    }

    private void UpdateLevelUI(int level)
    {
        levelUI.text = level.ToString();
    }
}
