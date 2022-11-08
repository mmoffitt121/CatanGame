using Catan.GameManagement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TradePhase : MonoBehaviour
{
    public GameObject tradeWindow;
    public GameObject offerWindow;
    public GameObject portTradeWindow;
    public GameObject tradeButton;
    public GameManager gm;

    public void OpenTradeWindow()
    {

    }

    public void OpenOfferWindow()
    {

    }

    public void OpenPortTradeWindow()
    {

    }

    public void ShowTradeButton()
    {

    }

    public void CloseTradeWindows()
    {
        tradeWindow.SetActive(false);
        offerWindow.SetActive(false);
        portTradeWindow.SetActive(false);
        tradeButton.SetActive(false);
    }
}
