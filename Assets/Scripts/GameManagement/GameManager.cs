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
using Catan.Settings;

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

        public bool nextTurn = false;

        public bool movingRobber = false;
        public (int, int) robberLocation;

        public UIManager UIManager;
        public BoardInitializer boardInitializer;
        public Board board;

        public ScoreBuilder scoreBuilder;

        public GameObject diceRoller;

        public bool quickRolling;

        public void Start()
        {
            LoadPlayers();
            boardInitializer.Initialize();
        }

        public void SetDefaultPlayers()
        {
            Player[] gplayers = new Player[6];
            
            gplayers[0] = new Player(true);
            gplayers[0].playerColor = new Color(100 / 255f, 100 / 255f, 255 / 255f);
            gplayers[0].primaryUIColor = new Color(190 / 255f, 200 / 255f, 255 / 255f);
            gplayers[0].secondaryUIColor = new Color(10 / 255f, 10 / 255f, 10 / 255f);
            gplayers[0].playerName = "Player 1";
            gplayers[0].playerIndex = 0;

            gplayers[1] = new Player(true);
            gplayers[1].playerColor = new Color(255 / 255f, 100 / 255f, 100 / 255f);
            gplayers[1].primaryUIColor = new Color(255 / 255f, 150 / 255f, 150 / 255f);
            gplayers[1].secondaryUIColor = new Color(10 / 255f, 10 / 255f, 10 / 255f);
            gplayers[1].playerName = "Player 2";
            gplayers[1].playerIndex = 1;

            gplayers[2] = new Player(true);
            gplayers[2].playerColor = new Color(240 / 255f, 240 / 255f, 240 / 255f);
            gplayers[2].primaryUIColor = new Color(250 / 255f, 250 / 255f, 250 / 255f);
            gplayers[2].secondaryUIColor = new Color(10 / 255f, 10 / 255f, 10 / 255f);
            gplayers[2].playerName = "Player 3";
            gplayers[2].playerIndex = 2;

            gplayers[3] = new Player(true);
            gplayers[3].playerColor = new Color(255 / 255f, 150 / 255f, 100 / 255f);
            gplayers[3].primaryUIColor = new Color(255 / 255f, 200 / 255f, 150 / 255f);
            gplayers[3].secondaryUIColor = new Color(10 / 255f, 10 / 255f, 10 / 255f);
            gplayers[3].playerName = "Player 4";
            gplayers[3].playerIndex = 3;

            gplayers[4] = new Player(true);
            gplayers[4].playerColor = new Color(205 / 255f, 255 / 255f, 12 / 255f);
            gplayers[4].primaryUIColor = new Color(250 / 255f, 250 / 255f, 100 / 255f);
            gplayers[4].secondaryUIColor = new Color(10 / 255f, 10 / 255f, 10 / 255f);
            gplayers[4].playerName = "Jim the AI";
            gplayers[4].playerIndex = 4;

            gplayers[5] = new Player(true);
            gplayers[5].playerColor = new Color(10 / 255f, 200 / 255f, 200 / 255f);
            gplayers[5].primaryUIColor = new Color(10 / 255f, 200 / 255f, 200 / 255f);
            gplayers[5].secondaryUIColor = new Color(10 / 255f, 10 / 255f, 10 / 255f);
            gplayers[5].playerName = "Kevin";
            gplayers[5].playerIndex = 5;

            foreach (Player p in gplayers)
            {
                p.resources = new Resource[5];
                p.resources[0] = new Resource(Resource.ResourceType.Grain, 0);
                p.resources[1] = new Resource(Resource.ResourceType.Wool, 0);
                p.resources[2] = new Resource(Resource.ResourceType.Wood, 0);
                p.resources[3] = new Resource(Resource.ResourceType.Brick, 0);
                p.resources[4] = new Resource(Resource.ResourceType.Ore, 0);
            }

            GameSettings.players = gplayers;
        }

        public void LoadPlayers()
        {
            if (GameSettings.players == null)
            {
                SetDefaultPlayers();
            }
            players = GameSettings.players;

            foreach (Player p in players)
            {
                if (p.isAI) { p.agent.Initialize(); }
            }
        }

        public void UpdateScores()
        {
            scoreBuilder.CalculateScores(players);
            UIManager.UpdateUI();
        }

        public void Roll()
        {
            if (quickRolling)
            {
                int r0 = Random.Range(0, 7);
                int r1 = Random.Range(0, 7);
                Rolled(r0 + r1);
                return;
            }
            diceRoller.SetActive(true);
            diceRoller.transform.GetChild(7).GetComponent<DiceCheckZoneScript>().Roll();
        }

        public void Rolled(int result)
        {
            diceRoller.SetActive(false);
            if (result == 7)
            {
                RolledSeven();
                return;
            }
            board.DistributeResources(players, result);
            UIManager.Rolled();
        }

        public void RolledSeven()
        {
            UIManager.SplitResources(new Stack<Player>(ResourceDistributor.GetSplittingPlayers(players)));
        }

        public void AdvanceTurn()
        {
            nextTurn = true;
        }

        public void ToNextTurn()
        {
            scoreBuilder.CalculateScores(players);
            phase++;

            Player winner = players.GetWinner();
            if (winner != null)
            {
                Debug.Log("Winner! " + winner.playerName + " has won!");
                return;
            }

            // In starting phase
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
            }
            // Normal gameplay loop
            else
            {
                // Next player
                if (phase > 2)
                {
                    phase = 0;
                    turn++;
                }

                // Reset turn
                if (turn >= players.Length || turn == -1)
                {
                    turn = 0;
                }

                // AI actions
            }

            if (currentPlayer.isAI)
            {
                if (starting)
                {
                    switch (phase)
                    {
                        case 0:
                            currentPlayer.agent?.PlaceStartingPiece(!reverseTurnOrder);
                            AdvanceTurn();
                            break;
                        case 1:
                            AdvanceTurn();
                            break;
                    }
                }
                else
                {
                    switch (phase)
                    {
                        case 0:
                            currentPlayer.agent?.api.Roll();
                            break;
                        case 1:
                            currentPlayer.agent?.StartTrading();
                            break;
                        case 2:
                            currentPlayer.agent?.StartBuilding();
                            break;
                    }
                }
            }

            UIManager.UpdateUI();
        }

        public void Update()
        {
            if (nextTurn)
            {
                nextTurn = false;
                ToNextTurn();
            }
        }
    }
}
