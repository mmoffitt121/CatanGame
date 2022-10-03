using Catan.GameManagement;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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

        public void AdvanceTurn()
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
            gameManager.movingRobber = true;
        }

        public void EndMoveRobber()
        {
            EnableAdvancement();
            gameManager.AdvanceTurn();
            gameManager.movingRobber = false;
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

            vPDisplay.text = "VP: " + gameManager.currentPlayer.victoryPoints.ToString();
        }
    }

}
