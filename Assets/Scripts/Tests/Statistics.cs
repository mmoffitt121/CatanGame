using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Catan.GameManagement;
using Catan.Settings;
using Catan.Players;

namespace Catan.Tests
{
    public class Statistics : MonoBehaviour
    {
        private GameManager gm;
        public List<Game> games;

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

        public int NumberOfGames
        {
            get
            {
                return games.Count;
            }
        }

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
    }
}