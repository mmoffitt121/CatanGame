using Catan.GameManagement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TradePhase : MonoBehaviour
{
    public GameObject playerSelect;
    public GameObject tradeWindow;
    public GameObject portTradeWindow;
    public GameObject offerWindow;
    public GameObject tradeButton;
    public GameObject nextButton;
    public GameManager gm;

    public int selectedPlayerIndex;

    public void OpenPlayerSelectWindow()
    {
        CloseTradeWindows();
        playerSelect.GetComponent<TradePhasePlayerSelect>().Initialize();
        playerSelect.SetActive(true);
    }

    public void OpenTradeWindow()
    {
        CloseTradeWindows();
        tradeWindow.SetActive(true);
    }

    public void OpenOfferWindow()
    {
        CloseTradeWindows();
        offerWindow.SetActive(true);
    }

    public void OpenPortTradeWindow()
    {
        CloseTradeWindows();
        portTradeWindow.SetActive(true);
    }

    public void ShowTradeButton()
    {
        CloseTradeWindows();
        tradeButton.SetActive(true);
        nextButton.SetActive(true);
    }

    public void CloseTradeWindows()
    {
        playerSelect.SetActive(false);
        tradeWindow.SetActive(false);
        offerWindow.SetActive(false);
        portTradeWindow.SetActive(false);
        tradeButton.SetActive(false);
        nextButton.SetActive(false);
    }

    public void Start()
    {
        playerSelect.GetComponent<TradePhasePlayerSelect>().tradePhase = this;
        tradeWindow.GetComponent<TradePhaseTradeWindow>();
        portTradeWindow.GetComponent<TradePhasePortTrade>();
        offerWindow.GetComponent<TradePhaseTradeOffer>();
    }
}
