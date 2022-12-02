/// AUTHOR: Matthew Moffitt
/// FILENAME: TestSuite.cs
/// SPECIFICATION: Responsible for Testing
/// FOR: CS 3368 Introduction to Artificial Intelligence Section 001

using Catan.GameManagement;
using Catan.Settings;
using Catan.Tests;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

/// <summary>
/// Responsible for managing test data
/// </summary>
public class TestSuite : MonoBehaviour
{
    // UI members
    public GameObject testMenu;
    public GameObject openTestMenuButton;
    public TextMeshProUGUI testStatisticsBox;
    public TextMeshProUGUI playerName;
    public TextMeshProUGUI playerStats;

    /// <summary>
    /// Statistics
    /// </summary>
    public Statistics stats;
    /// <summary>
    /// Index of currently selected player
    /// </summary>
    public int selectedPlayer = 0;

    /// <summary>
    /// Game Manager
    /// </summary>
    public GameManager gm;

    // Display of test menu functions

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

    /// <summary>
    /// Saves the current game
    /// </summary>
    public void SaveGame()
    {
        stats.SaveGame();
        UpdateStatistics();
    }

    /// <summary>
    /// Updates the statistics display
    /// </summary>
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

    /// <summary>
    /// Switches displayed player
    /// </summary>
    /// <param name="change"></param>
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

    /// <summary>
    /// Initialization
    /// </summary>
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
