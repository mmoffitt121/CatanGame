using Catan.GameManagement;
using Catan.Players;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TradePhasePlayerSelect : MonoBehaviour
{
    public TradePhase tradePhase;

    public GameObject[] buttons;
    public Player[] selectablePlayers;

    public void SelectPlayer(int player)
    {
        tradePhase.selectedPlayer = selectablePlayers[player];
        tradePhase.OpenTradeWindow();
    }

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
