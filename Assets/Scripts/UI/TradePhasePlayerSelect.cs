using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TradePhasePlayerSelect : MonoBehaviour
{
    public TradePhase tradePhase;

    public GameObject[] buttons;

    public void SelectPlayer(int player)
    {
        tradePhase.OpenTradeWindow();
    }

    public void Initialize()
    {
        
    }
}
