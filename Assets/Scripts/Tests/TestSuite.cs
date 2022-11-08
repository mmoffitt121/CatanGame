using Catan.GameManagement;
using Catan.Settings;
using Catan.Tests;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TestSuite : MonoBehaviour
{
    public GameObject testMenu;
    public GameObject openTestMenuButton;
    public TextMeshProUGUI testStatisticsBox;
    public Statistics stats;

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
            "Mean # of Turns/Game: " + "___" + "\n" +
            "Median # of Turns/Game: " + "___" + "\n" +
            "Average VP Disparity: " + "___" + "\n";

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
        }
    }
}
