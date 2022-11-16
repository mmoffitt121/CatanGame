using Catan.GameManagement;
using Catan.Settings;
using Catan.Tests;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class TestSuite : MonoBehaviour
{
    public GameObject testMenu;
    public GameObject openTestMenuButton;
    public TextMeshProUGUI testStatisticsBox;
    public TextMeshProUGUI playerName;
    public TextMeshProUGUI playerStats;
    public Statistics stats;

    public int selectedPlayer = 0;

    public GameManager gm;

    public void OpenTestMenu()
    {
        testMenu.SetActive(true);
        openTestMenuButton.SetActive(false);
    }

    public void CloseTestMenu()
    {
        testMenu.SetActive(false);
        openTestMenuButton.SetActive(true);
    }

    public void SaveGame()
    {
        stats.SaveGame();
        UpdateStatistics();
    }

    void UpdateStatistics()
    {
        testStatisticsBox.text =
            "Number of Games: " + stats.NumberOfGames + "\n" +
            "Mean # of Turns/Game: " + stats.MeanTurns + "\n" +
            "Median # of Turns/Game: " + stats.MedianTurns + "\n" +
            "Average VP Disparity: " + stats.AverageVPDisparity + "\n" +
            "Stalemates: " + stats.StaleMates + ", " + stats.StaleMateRatio * 100 + "%\n";
        playerName.text = gm.players[selectedPlayer].playerName;
        playerName.color = gm.players[selectedPlayer].primaryUIColor;
        if (gm.players[selectedPlayer].isAI)
        {
            playerStats.text =
                "Number of Wins: " + stats.Victories(selectedPlayer) + "\n" +
                "Win Percentage: " + stats.VictoryPercentage(selectedPlayer) + "%\n" +
                "AI: " + gm.players[selectedPlayer].agent.agentName;
        }
        else
        {
            playerStats.text = "Non-AI player.\n" +
                "# of Wins: " + stats.Victories(selectedPlayer) +
                "Win %" + stats.VictoryPercentage(selectedPlayer);
        }
    }

    public void ChangePlayer(int change)
    {
        selectedPlayer += change;
        if (selectedPlayer > gm.players.Count() - 1)
        {
            selectedPlayer -= gm.players.Count();
        }
        if (selectedPlayer < 0)
        {
            selectedPlayer += gm.players.Count();
        }
        UpdateStatistics();
    }

    void Start()
    {
        if (!GameSettings.testing)
        {
            Destroy(openTestMenuButton);
            Destroy(testMenu);
        }
        else
        {
            gm = GameObject.Find("Game Manager").GetComponent<GameManager>();
            gm.testSuite = this;
            UpdateStatistics();
        }
    }
}
