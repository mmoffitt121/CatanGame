using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Catan.GameManagement;
using Catan.Settings;
using Catan.Players;
using System;
using System.Linq;

namespace Catan.Tests
{
    public class Statistics : MonoBehaviour
    {
        private GameManager gm;
        public List<Game> games;

        #region Saving
        public void SaveGame()
        {
            Game game = new Game();
            game.turns = gm.totalTurns;
            game.rounds = gm.totalRounds;
            game.playerCount = gm.players.Length;

            game.names = new string[gm.players.Length];
            game.agentTypes = new string[gm.players.Length];
            game.victoryPoints = new int[gm.players.Length];
            // game.towns = new int[gm.players.Length];
            // game.cities = new int[gm.players.Length];
            // game.ports = new int[gm.players.Length];

            for (int i = 0; i < gm.players.Length; i++)
            {
                game.names[i] = gm.players[i].playerName.ToString();
                game.agentTypes[i] = gm.players[i].agent.agentName.ToString();
                game.victoryPoints[i] = gm.players[i].victoryPoints;
            }
            games.Add(game);
        }
        #endregion

        #region Derived Data
        public int NumberOfGames
        {
            get
            {
                return games.Count;
            }
        }

        public float MeanTurns
        {
            get
            {
                float result = 0f;
                float count = games.Count;
                foreach (Game g in games)
                {
                    result += g.turns / count;
                }
                return result;
            }
        }

        public float MedianTurns
        {
            get
            {
                if (games == null || games.Count == 0) { return 0f; }

                games.Sort((x, y) => x.turns.CompareTo(y.turns));
                return games[(int)games.Count / 2].turns;
            }
        }

        public float AverageVPDisparity
        {
            get
            {
                float result = 0f;
                float count = games.Count;
                foreach (Game g in games)
                {
                    float vpDisp = g.victoryPoints.Max() - g.victoryPoints.Min();
                    result += vpDisp / count;
                }
                return result;
            }
        }

        public int StaleMates
        {
            get
            {
                int result = 0;
                foreach (Game g in games)
                {
                    bool victory = false;
                    foreach (int vp in g.victoryPoints)
                    {
                        if (vp >= GameSettings.vpWinCondition)
                        {
                            victory = true;
                            break;
                        }
                    }

                    if (!victory) { result++; }
                }
                return result;
            }
        }

        public float StaleMateRatio
        {
            get
            {
                if (games == null || games.Count == 0) { return 0; }
                return ((float)StaleMates) / games.Count;
            }
        }

        public int Victories(int player)
        {
            return games.Where(g => g.victoryPoints[player] >= 10).Count();
        }

        public float VictoryPercentage(int player)
        {
            if (games == null || games.Count == 0) { return 0; }
            return games.Where(g => g.victoryPoints[player] >= 10).Count()
                / (float)games.Count * 100f;
        }
        #endregion

        #region Start
        void Start()
        {
            if (GameSettings.testing)
            {
                gm = GameObject.Find("Game Manager").GetComponent<GameManager>();
                gm.stats = this;
            }
            else
            {
                Destroy(this);
            }
        }
        #endregion
    }
}