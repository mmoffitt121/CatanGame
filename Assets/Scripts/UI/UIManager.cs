/// AUTHOR: Wuraola Alli, Jett Graham, Evan Griffin, Matthew Moffitt, Alex Rizzo, Brandon Villalobos
/// FILENAME: UIManager.cs
/// SPECIFICATION: File containing UI Information
/// FOR: CS 3368 Introduction to Artificial Intelligence Section 001

using Catan.GameManagement;
using Catan.Players;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;
using System.Net;

namespace Catan.UI
{
    public class UIManager : MonoBehaviour
    {
        public GameManager gameManager;

        public TextMeshProUGUI playerDisplay;
        public TextMeshProUGUI phaseDisplay;
        public TextMeshProUGUI nextDisplay;
        public Button nextButton;

        public TextMeshProUGUI oreDisplay;
        public TextMeshProUGUI brickDisplay;
        public TextMeshProUGUI woodDisplay;
        public TextMeshProUGUI grainDisplay;
        public TextMeshProUGUI sheepDisplay;

        public TextMeshProUGUI vPDisplay;

        public GameObject rSplit;

        public void AdvanceTurn()
        {
            if (gameManager.phase == 0 && !gameManager.starting && !gameManager.movingRobber)
            {
                gameManager.Roll();
                if (!gameManager.quickRolling) DisableAdvancement();
                return;
            }
            gameManager.AdvanceTurn();
            UpdateUI();
        }

        public void Rolled()
        {
            gameManager.AdvanceTurn();
            UpdateUI();
        }

        public void DisableAdvancement()
        {
            nextButton.interactable = false;
        }

        public void EnableAdvancement()
        {
            nextButton.interactable = true;
        }

        public void StartMoveRobber()
        {
            DisableAdvancement();
            phaseDisplay.text = "Move The Robber!";
            nextDisplay.text = "Next";
            nextButton.interactable = false;
            gameManager.movingRobber = true;
            if (gameManager.currentPlayer.isAI)
            {
                gameManager.currentPlayer.agent.ChooseRobberLocation();
            }
        }

        public void EndMoveRobber()
        {
            EnableAdvancement();
            gameManager.AdvanceTurn();
            gameManager.movingRobber = false;
        }

        public void SplitResources(Stack<Player> players)
        {
            if (players.Count > 0)
            {
                rSplit.SetActive(true);
                ResourceSplit splitter = rSplit.GetComponent<ResourceSplit>();
                splitter.InitializePlayer(players);
            }
            else
            {
                StartMoveRobber();
            } 
        }

        public void UpdateUI()
        {
            playerDisplay.text = gameManager.currentPlayer.playerName;
            playerDisplay.color = gameManager.currentPlayer.primaryUIColor;
            phaseDisplay.color = gameManager.currentPlayer.primaryUIColor;
            nextDisplay.color = gameManager.currentPlayer.secondaryUIColor;
            nextButton.GetComponent<Image>().color = gameManager.currentPlayer.primaryUIColor;

            grainDisplay.text = gameManager.currentPlayer.resources[0].amount.ToString();
            sheepDisplay.text = gameManager.currentPlayer.resources[1].amount.ToString();
            woodDisplay.text = gameManager.currentPlayer.resources[2].amount.ToString();
            brickDisplay.text = gameManager.currentPlayer.resources[3].amount.ToString();
            oreDisplay.text = gameManager.currentPlayer.resources[4].amount.ToString();

            if (gameManager.starting)
            {
                switch (gameManager.phase)
                {
                    case 0:
                        phaseDisplay.text = "Place Settlement";
                        nextDisplay.text = "Next";
                        nextButton.interactable = false;
                        break;
                    case 1:
                        phaseDisplay.text = "Place Road";
                        nextDisplay.text = "Next";
                        nextButton.interactable = false;
                        break;
                    default:
                        phaseDisplay.text = "Waiting...";
                        nextDisplay.text = "Waiting...";
                        break;
                }
            }
            else
            {
                switch (gameManager.phase)
                {
                    case 0:
                        phaseDisplay.text = "Roll Resources";
                        nextDisplay.text = "Roll";
                        nextButton.interactable = true;
                        break;
                    case 1:
                        phaseDisplay.text = "Trade";
                        nextDisplay.text = "End Trading";
                        nextButton.interactable = true;
                        break;
                    case 2:
                        phaseDisplay.text = "Build";
                        nextDisplay.text = "End Building";
                        nextButton.interactable = true;
                        break;
                    default:
                        phaseDisplay.text = "Waiting...";
                        nextDisplay.text = "Waiting...";
                        break;
                }
            }

            if (gameManager.currentPlayer.isAI)
            {
                nextButton.interactable = false;
            }

            vPDisplay.text = "VP: " + gameManager.currentPlayer.victoryPoints.ToString();
        }
    }

}
