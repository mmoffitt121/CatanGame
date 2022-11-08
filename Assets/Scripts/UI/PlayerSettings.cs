using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Catan.AI;
using Catan.Players;
using Catan.ResourcePhase;
using System;

namespace Catan.UI
{
    public class PlayerSettings : MonoBehaviour
    {
        public TextMeshProUGUI playerOrAI;
        public GameObject playerNameInput;
        public GameObject playerName;
        public Image colorPanel;
        public TextMeshProUGUI chooseAgent;

        public static readonly Color[] possibleColors = {
            new Color(100 / 255f, 100 / 255f, 255 / 255f),
            new Color(255 / 255f, 100 / 255f, 100 / 255f),
            new Color(240 / 255f, 240 / 255f, 240 / 255f),
            new Color(255 / 255f, 150 / 255f, 100 / 255f),
            new Color(205 / 255f, 255 / 255f, 12 / 255f),
            new Color(10 / 255f, 200 / 255f, 200 / 255f)
        };
        public static readonly Color[] uiColors = {
            new Color(190 / 255f, 200 / 255f, 255 / 255f),
            new Color(255 / 255f, 150 / 255f, 150 / 255f),
            new Color(250 / 255f, 250 / 255f, 250 / 255f),
            new Color(255 / 255f, 200 / 255f, 150 / 255f),
            new Color(250 / 255f, 250 / 255f, 100 / 255f),
            new Color(10 / 255f, 200 / 255f, 200 / 255f)
        };
        public static readonly Color[] secondaryColors = {
            new Color(10 / 255f, 10 / 255f, 10 / 255f),
            new Color(10 / 255f, 10 / 255f, 10 / 255f),
            new Color(10 / 255f, 10 / 255f, 10 / 255f),
            new Color(10 / 255f, 10 / 255f, 10 / 255f),
            new Color(10 / 255f, 10 / 255f, 10 / 255f),
            new Color(10 / 255f, 10 / 255f, 10 / 255f)
        };

        public int index;
        public int color;
        public PlayerMode pMode;
        public string pName = "Player X";
        public AgentType chosenAgent;

        public void OpenNameBox()
        {
            pName = playerName.GetComponent<TextMeshProUGUI>().text;
            playerNameInput.GetComponent<TMP_InputField>().text = pName;

            playerNameInput.SetActive(true);
            playerName.SetActive(false);
        }

        public void CloseNameBox()
        {
            pName = playerNameInput.GetComponent<TMP_InputField>().text;
            playerName.GetComponent<TextMeshProUGUI>().text = pName;

            playerNameInput.SetActive(false);
            playerName.SetActive(true);
        }    

        public void ToggleNameBox()
        {
            if (playerNameInput.activeInHierarchy)
            {
                CloseNameBox();
            }
            else
            {
                OpenNameBox();
            }
        }

        public void PlayerTypeLeft()
        {
            int newMode = ((int)pMode) - 1;
            if (newMode < 0) { newMode += 3; }

            pMode = (PlayerMode)(newMode);
            UpdateDisplayColors();
        }

        public void PlayerTypeRight()
        {
            pMode = (PlayerMode)((((int)pMode) + 1) % 3);
            UpdateDisplayColors();
        }

        public void UpdateDisplayColors()
        {
            switch (pMode)
            {
                case PlayerMode.Player:
                    playerOrAI.text = "Player";
                    colorPanel.color = uiColors[color];
                    break;
                case PlayerMode.AI:
                    playerOrAI.text = "AI";
                    colorPanel.color = uiColors[color];
                    break;
                case PlayerMode.None:
                    playerOrAI.text = "Disabled";
                    colorPanel.color = new Color(0, 0, 0, 0.5f);
                    break;
            }
        }

        public void ColorLeft()
        {
            color = color - 1;
            if (color < 0) { color = possibleColors.Length - 1; }
            UpdateDisplayColors();
        }

        public void ColorRight()
        {
            color = (color + 1) % possibleColors.Length;
            UpdateDisplayColors();
        }

        public void AgentLeft()
        {
            chosenAgent = (AgentType)(((int)chosenAgent + 1) % Enum.GetNames(typeof(AgentType)).Length);
            UpdateAgentDisplay();
        }

        public void AgentRight()
        {
            chosenAgent = (AgentType)((int)chosenAgent - 1);
            if (chosenAgent < 0)
            {
                chosenAgent = (AgentType)(Enum.GetNames(typeof(AgentType)).Length - 1);
            }
            UpdateAgentDisplay();
        }

        public void UpdateAgentDisplay()
        {
            switch (chosenAgent)
            {
                case AgentType.HasBrosAI:
                    chooseAgent.text = "Has Bros";
                    break;
                case AgentType.Random:
                    chooseAgent.text = "Random";
                    break;
                default:
                    break;
            }
        }

        public Player GetPlayer()
        {
            if (pMode == PlayerMode.None) { return null; }

            Player player = new Player()
            {
                playerColor = possibleColors[color],
                primaryUIColor = uiColors[color],
                secondaryUIColor = secondaryColors[color],
                playerName = pName,
                playerIndex = index,
                resources = new Resource[5],
            };

            if (pMode == PlayerMode.AI)
            {
                player.isAI = true;

                switch (chosenAgent)
                {
                    case AgentType.Random:
                        player.agent = new RandomAgent(player);
                        break;
                    case AgentType.HasBrosAI:
                        player.agent = new HasBrosAgent(player);
                        break;
                }
            }

            player.resources[0] = new Resource(Resource.ResourceType.Grain, 0);
            player.resources[1] = new Resource(Resource.ResourceType.Wool, 0);
            player.resources[2] = new Resource(Resource.ResourceType.Wood, 0);
            player.resources[3] = new Resource(Resource.ResourceType.Brick, 0);
            player.resources[4] = new Resource(Resource.ResourceType.Ore, 0);

            return player;
        }

        public void Start()
        {
            chosenAgent = AgentType.HasBrosAI;
            colorPanel.color = uiColors[color];
            playerName.GetComponent<TextMeshProUGUI>().text = pName;
            UpdateDisplayColors();
            UpdateAgentDisplay();
        }

        public enum PlayerMode
        {
            Player,
            AI,
            None
        }

        public enum AgentType
        {
            Random,
            HasBrosAI
        }
    }
}
