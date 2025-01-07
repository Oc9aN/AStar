using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] TMP_Text moneyUI;
    [SerializeField] TMP_Text xSizeText;
    [SerializeField] TMP_Text ySizeText;

    [SerializeField] TMP_Text xStart;
    [SerializeField] TMP_Text yStart;

    [SerializeField] TMP_Text xDest;
    [SerializeField] TMP_Text yDest;
    public void SubscribeMoneyUpdates(UserManager userManager)
    {
        userManager.OnMoneyChanged += UpdateMoneyUI;
    }

    public void UpdateMoneyUI(int value)
    {
        // MoneyUI 업데이트
        moneyUI.text = value.ToString();
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
