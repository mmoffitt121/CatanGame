using Catan.GameBoard;
using Catan.Players;
using Catan.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TradePhasePortTrade : MonoBehaviour
{
    Player player;

    public Toggle[] toggles;
    public TextMeshProUGUI[] costs;
    public TextMeshProUGUI portsLabel;

    public TradePhase tradePhase;
    public Board board;

    public void Initialize(Player player)
    {
        this.player = player;
        portsLabel.color = player.primaryUIColor;
        Check(-1);
    }

    public void Check(int checkBox)
    {
        foreach (Toggle t in toggles)
        {
            t.isOn = false;
        }
        if (checkBox != -1)
        {
            toggles[checkBox].isOn = true;
            UpdateUI(checkBox);
        }
    }

    public void UpdateUI(int selected)
    {

    }
}
