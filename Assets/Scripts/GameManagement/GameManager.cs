using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Catan.Players;
using Catan.GameBoard;
using Catan.UI;
using Catan.ResourcePhase;
using TMPro;

namespace Catan.GameManagement
{
    public class GameManager : MonoBehaviour
    {
        public Player[] players;

        public Player currentPlayer
        {
            get
            {
                return players[turn];
            }
        }

        public int turn = -1;
        public int phase = -1;

        public UIManager UIManager;
        public BoardInitializer boardInitializer;

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

        public void AdvanceTurn()
        {
            phase++;

            if (phase > 2)
            {
                phase = 0;
                turn++;
            }

            if (turn >= players.Length || turn == -1)
            {
                turn = 0;
            }
        }
    }
}
