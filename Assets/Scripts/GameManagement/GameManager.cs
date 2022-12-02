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
using Catan.Tests;
using System.Reflection;

namespace Catan.GameManagement
{
    /// <summary>
    /// Responsible for managing and scheduling all game activities
    /// </summary>
    public class GameManager : MonoBehaviour
    {
        /// <summary>
        /// Player[] containting all players in the current game
        /// </summary>
        public Player[] players;
        /// <summary>
        /// Resource icon image array
        /// </summary>
        public Texture2D[] resourceIcons;
        /// <summary>
        /// Returns the player who's turn it currently is
        /// </summary>
        public Player currentPlayer
        {
            get
            {
                return players[turn];
            }
        }

        /// <summary>
        /// Current turn
        /// </summary>
        public int turn = -1;
        /// <summary>
        /// Current phase
        /// </summary>
        public int phase = -1;
        /// <summary>
        /// Whether or not the game is in start phase
        /// </summary>
        public bool starting = true;
        /// <summary>
        /// Whether or not the game is in the second part of start phase where the turn order is reversed
        /// </summary>
        public bool reverseTurnOrder = false;

        /// <summary>
        /// Whether or not advancement can occur
        /// </summary>
        public bool nextTurn = false;

        /// <summary>
        /// Whether or not current player is moving robber
        /// </summary>
        public bool movingRobber = false;
        /// <summary>
        /// Integer tuple representing the location of the robber
        /// </summary>
        public (int, int) robberLocation;

        /// <summary>
        /// UI Manager, contains all functions pertaining to the user interface
        /// </summary>
        public UIManager UIManager;
        /// <summary>
        /// Class that initializes the game board
        /// </summary>
        public BoardInitializer boardInitializer;
        /// <summary>
        /// The game board
        /// </summary>
        public Board board;

        /// <summary>
        /// Class responsible for calculating scores
        /// </summary>
        public ScoreBuilder scoreBuilder;

        /// <summary>
        /// Class responsible for rolling dice
        /// </summary>
        public GameObject diceRoller;

        /// <summary>
        /// Whether or not dice animation will be skipped
        /// </summary>
        public bool quickRolling;

        /// <summary>
        /// Total turn counter
        /// </summary>
        public int totalTurns = 0;
        /// <summary>
        /// Total round counter
        /// </summary>
        public int totalRounds = 0;
        /// <summary>
        /// Class containing round statistics
        /// </summary>
        public Statistics stats;
        /// <summary>
        /// Class responsible for testing
        /// </summary>
        public TestSuite testSuite;

        public void Start()
        {
            LoadPlayers();
            quickRolling = GameSettings.quickrolling;
            boardInitializer.Initialize();
        }

        /// <summary>
        /// Generates dummy players. Not used in production, only used when game is loaded from the game scene rather than the main menu.
        /// </summary>
        public void SetDefaultPlayers()
        {
            Player[] gplayers = new Player[4];
            
            gplayers[0] = new Player(true);
            gplayers[0].agent = new HasBrosAgent(gplayers[0]);
            gplayers[0].playerColor = new Color(100 / 255f, 100 / 255f, 255 / 255f);
            gplayers[0].primaryUIColor = new Color(190 / 255f, 200 / 255f, 255 / 255f);
            gplayers[0].secondaryUIColor = new Color(10 / 255f, 10 / 255f, 10 / 255f);
            gplayers[0].playerName = "HasBros Agent";
            gplayers[0].playerIndex = 0;

            gplayers[1] = new Player(true);
            //gplayers[1].agent = new HasBrosAgent(gplayers[1]);
            gplayers[1].playerColor = new Color(255 / 255f, 100 / 255f, 100 / 255f);
            gplayers[1].primaryUIColor = new Color(255 / 255f, 150 / 255f, 150 / 255f);
            gplayers[1].secondaryUIColor = new Color(10 / 255f, 10 / 255f, 10 / 255f);
            gplayers[1].playerName = "Player 2";
            gplayers[1].playerIndex = 1;

            gplayers[2] = new Player(true);
            //gplayers[2].agent = new HasBrosAgent(gplayers[2]);
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

        /// <summary>
        /// Loads player info into the game
        /// </summary>
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

        /// <summary>
        /// Updates the game scores for each player
        /// </summary>
        /// <param name="updateUI"></param>
        public void UpdateScores(bool updateUI = true)
        {
            scoreBuilder.CalculateScores(players);

            if (updateUI) { UIManager.UpdateUI(); }
        }

        /// <summary>
        /// Rolls the dice
        /// </summary>
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

        /// <summary>
        /// Called after a dice rolling is complete
        /// </summary>
        /// <param name="result"></param>
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

        /// <summary>
        /// Is called when a seven is rolled, calls UI manager to split resources
        /// </summary>
        public void RolledSeven()
        {
            UIManager.SplitResources(new Stack<Player>(ResourceDistributor.GetSplittingPlayers(players)));
        }

        /// <summary>
        /// Advances the turn
        /// </summary>
        public void AdvanceTurn()
        {
            nextTurn = true;
        }

        /// <summary>
        /// Called on victory. If testing, the game restarts.
        /// </summary>
        /// <param name="winner"></param>
        public void OnVictory(Player winner)
        {
            Debug.Log("Winner! " + winner.playerName + " has won!");

            if (stats != null)
            {
                testSuite.SaveGame();
            }

            if (GameSettings.testing)
            {
                ResetGameAndBegin();
            }
        }

        /// <summary>
        /// Called on stalemate. If testing, the game restarts.
        /// </summary>
        public void OnStaleMate()
        {
            Debug.Log("Stalemate.");

            if (stats != null)
            {
                testSuite.SaveGame();
            }

            if (GameSettings.testing)
            {
                ResetGameAndBegin();
            }
        }

        /// <summary>
        /// Resets the game and starts it anew. Used for testing.
        /// </summary>
        public void ResetGameAndBegin()
        {
            ResetGame();
            UIManager.AdvanceTurn();
        }

        /// <summary>
        /// Resets and regenerates the game board and players
        /// </summary>
        public void ResetGame()
        {
            boardInitializer.Reinitialize();
            turn = 0;
            phase = -1;
            starting = true;
            reverseTurnOrder = false;
            nextTurn = false;
            movingRobber = false;
            totalRounds = 0;
            totalTurns = 0;

            UpdateScores(false);
            UIManager.ResetUI();
        }

        /// <summary>
        /// Called when turn advances
        /// </summary>
        public void ToNextTurn()
        {
            scoreBuilder.CalculateScores(players);
            phase++;

            Player winner = players.GetWinner();
            if (winner != null)
            {
                OnVictory(winner);
                return;
            }

            if (totalTurns > GameSettings.maxTurns)
            {
                OnStaleMate();
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
                    totalTurns += 1;
                }

                // Reset turn
                if (turn >= players.Length || turn == -1)
                {
                    turn = 0;
                    totalRounds += 1;
                }
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

        /// <summary>
        /// Advances turn when game is ready to advance.
        /// </summary>
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
