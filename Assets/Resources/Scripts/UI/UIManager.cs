using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] TMP_Text hpUI;         // 현재 체력
    [SerializeField] TMP_Text moneyUI;      // 보유 금액
    [SerializeField] TMP_Text xSizeText;    // 맵 크기
    [SerializeField] TMP_Text ySizeText;
    [SerializeField] TMP_Text xStart;       // 시작 위치
    [SerializeField] TMP_Text yStart;
    [SerializeField] TMP_Text xDest;        // 끝 위치
    [SerializeField] TMP_Text yDest;
    public void SubscribeUserDataUpdate(UserManager userManager)
    {
        userManager.OnMoneyChanged += UpdateMoneyUI;
        userManager.OnHpChanged += UpdateHpUI;
    }

    private void UpdateHpUI(int hp, int maxHP)
    {
        hpUI.text = $"{hp} / {maxHP}";
    }

    private void UpdateMoneyUI(int money)
    {
        moneyUI.text = money.ToString();
    }

    public void UpdateSizeUI(int xSize, int ySize)
    {
        xSizeText.text = xSize.ToString();
        ySizeText.text = ySize.ToString();
    }

    public void UpdateStartUI(int x, int y)
    {
        xStart.text = x.ToString();
        yStart.text = y.ToString();
    }

    public void UpdateDestUI(int x, int y)
    {
        xDest.text = x.ToString();
        yDest.text = y.ToString();
    }
}
