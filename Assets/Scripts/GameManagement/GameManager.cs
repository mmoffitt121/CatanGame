/// AUTHOR: Matthew Moffitt, Evan Griffin, Alex Rizzo, Brandon Villalobos
/// FILENAME: GameManager.cs
/// SPECIFICATION: File that manages turns and game data
/// FOR: CS 3368 Introduction to Artificial Intelligence Section 001

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Catan.Players;
using Catan.GameBoard;
using Catan.UI;
using Catan.ResourcePhase;
using TMPro;
using Catan.Scoring;
using Catan.AI;
using System.Linq;
using UnityEngine.UIElements;

namespace Catan.GameManagement
{
    public class GameManager : MonoBehaviour
    {
        public Player[] players;
        public Texture2D[] resourceIcons;
        public Player currentPlayer
        {
            get
            {
                return players[turn];
            }
        }

        public int turn = -1;
        public int phase = -1;
        public bool starting = true;
        public bool reverseTurnOrder = false;

        public bool movingRobber = false;
        public (int, int) robberLocation;

        public UIManager UIManager;
        public BoardInitializer boardInitializer;
        public Board board;

        public ScoreBuilder scoreBuilder;

        public GameObject diceRoller;

        public void Start()
        {
            SetDefaultPlayers();
            boardInitializer.Initialize();
        }

        public void SetDefaultPlayers()
        {
            players = new Player[4];
            
            players[0] = new Player();
            players[0].playerColor = new Color(100 / 255f, 100 / 255f, 255 / 255f);
            players[0].primaryUIColor = new Color(190 / 255f, 200 / 255f, 255 / 255f);
            players[0].secondaryUIColor = new Color(10 / 255f, 10 / 255f, 10 / 255f);
            players[0].playerName = "Player 1";
            players[0].playerIndex = 0;

            players[1] = new Player();
            players[1].playerColor = new Color(255 / 255f, 100 / 255f, 100 / 255f);
            players[1].primaryUIColor = new Color(255 / 255f, 150 / 255f, 150 / 255f);
            players[1].secondaryUIColor = new Color(10 / 255f, 10 / 255f, 10 / 255f);
            players[1].playerName = "Player 2";
            players[1].playerIndex = 1;

            players[2] = new Player();
            players[2].playerColor = new Color(240 / 255f, 240 / 255f, 240 / 255f);
            players[2].primaryUIColor = new Color(250 / 255f, 250 / 255f, 250 / 255f);
            players[2].secondaryUIColor = new Color(10 / 255f, 10 / 255f, 10 / 255f);
            players[2].playerName = "Player 3";
            players[2].playerIndex = 2;

            players[3] = new Player();
            players[3].playerColor = new Color(255 / 255f, 150 / 255f, 100 / 255f);
            players[3].primaryUIColor = new Color(255 / 255f, 200 / 255f, 150 / 255f);
            players[3].secondaryUIColor = new Color(10 / 255f, 10 / 255f, 10 / 255f);
            players[3].playerName = "Player 4";
            players[3].playerIndex = 3;

            foreach (Player p in players)
            {
                p.resources = new Resource[6];
                p.resources[0] = new Resource(Resource.ResourceType.Grain, 0);
                p.resources[1] = new Resource(Resource.ResourceType.Wool, 0);
                p.resources[2] = new Resource(Resource.ResourceType.Wood, 0);
                p.resources[3] = new Resource(Resource.ResourceType.Brick, 0);
                p.resources[4] = new Resource(Resource.ResourceType.Ore, 0);
            }
        }

        public void UpdateScores()
        {
            scoreBuilder.CalculateScores(players);
            UIManager.UpdateUI();
        }

        public void Roll()
        {
            diceRoller.SetActive(true);
            diceRoller.transform.GetChild(7).GetComponent<DiceCheckZoneScript>().Roll();
        }

        public void Rolled(int result)
        {
            diceRoller.SetActive(false);
            if (result == 7)
            {
                UIManager.StartMoveRobber();
                movingRobber = true;
                return;
            }
            board.DistributeResources(players, result);
            UIManager.Rolled();
        }

        public void AdvanceTurn()
        {
            scoreBuilder.CalculateScores(players);
            phase++;

            if (starting)
            {
                if (phase > 1 && !reverseTurnOrder)
                {
                    phase = 0;
                    turn++;
                }
                else if (phase > 1 && reverseTurnOrder)
                {
                    phase = 0;
                    turn--;
                }

                if (turn >= players.Length)
                {
                    reverseTurnOrder = true;
                    turn = players.Length - 1;
                }
                if (turn < 0 && reverseTurnOrder)
                {
                    starting = false;
                    reverseTurnOrder = false;
                    turn = 0;
                }

                if (currentPlayer.isAI)
                {
                    switch (phase)
                    {
                        case 0:
                            // Catan.AI.Agent.Roll()
                            AdvanceTurn();
                            break;
                        case 1:
                            // Catan.AI.Agent.Trade()
                            AdvanceTurn();
                            break;
                    }
                }
            }
            else
            {
                

                if (phase > 2)
                {
                    phase = 0;
                    turn++;
                }

                if (turn >= players.Length || turn == -1)
                {
                    turn = 0;
                }

                if (currentPlayer.isAI)
                {
                    switch (phase)
                    {
                        case 0:
                            // Catan.AI.Agent.Roll()
                            AdvanceTurn();
                            break;
                        case 1:
                            // Catan.AI.Agent.Trade()
                            AdvanceTurn();
                            break;
                        case 2:
                            // Catan.AI.Agent.Build()
                            AdvanceTurn();
                            break;
                    }
                }
            }
            
        }
    }
}
