/// AUTHOR: Matthew Moffitt
/// FILENAME: TradePhasePlayerSelect.cs
/// SPECIFICATION: Responsible for trade phase player select
/// FOR: CS 3368 Introduction to Artificial Intelligence Section 001

using Catan.GameManagement;
using Catan.Players;
using Catan.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Responsible for selecting a player during trade phase
/// </summary>
public class TradePhasePlayerSelect : MonoBehaviour
{
    /// <summary>
    /// Trade phase
    /// </summary>
    public TradePhase tradePhase;

    // UI members
    public GameObject[] buttons;
    public Player[] selectablePlayers;

    // UI logic functions
    
    /// <summary>
    /// Selects the specified player to trade with
    /// </summary>
    /// <param name="player"></param>
    public void SelectPlayer(int player)
    {
        tradePhase.selectedPlayer = selectablePlayers[player];
        tradePhase.OpenTradeWindow();
    }

    /// <summary>
    /// Initializes Window
    /// </summary>
    public void Initialize()
    {
        GameManager gm = tradePhase.gm;
        List<Player> sPlayers = new List<Player>();
        foreach (Player p in gm.players)
        {
            if (gm.currentPlayer.playerIndex == p.playerIndex)
            {
                continue;
            }

            sPlayers.Add(p);
        }

        selectablePlayers = sPlayers.ToArray();

        foreach (GameObject btn in buttons)
        {
            btn.SetActive(false);
        }

        for (int i = 0; i < selectablePlayers.Count(); i++)
        {
            buttons[i].SetActive(true);
            Button btn = buttons[i].GetComponent<Button>();
            TextMeshProUGUI txt = buttons[i].transform.GetChild(0).GetComponent<TextMeshProUGUI>();

            btn.GetComponent<Image>().color = selectablePlayers[i].primaryUIColor;
            txt.text = selectablePlayers[i].playerName;
        }
    }
}
