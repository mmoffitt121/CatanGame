using Catan.Players;
using Catan.TradePhase;
using Catan.ResourcePhase;
using Catan.BuildPhase;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Catan.GameManagement;
using Catan.GameBoard;

namespace Catan.AI
{
    /// <summary>
    /// AgentAPI class. Serves as a layer between the Catan Game and the AI. This is effectively the AI's version of a GUI. Should be attatched to a GameObject, and is used by all AI Agents.
    /// </summary>
    public class AgentAPI : MonoBehaviour
    {
        public GameManager gameManager;
        public InteractionManager interactionManager;
        public Board board;

        public Tile[][] Tiles
        {
            get
            {
                return board.tiles;
            }
        }
        public TileVertex[][] Vertices
        {
            get
            {
                return board.vertices;
            }
        }
        public Road[][] Roads
        {
            get
            {
                return board.roads;
            }
        }
        public Player[] Players
        {
            get
            {
                return gameManager.players;
            }
        }

        public void Roll()
        {
            gameManager.Roll();
        }

        public void MoveRobber(int i, int j)
        {
            TileGameObject obj = GameObject.Find("Tile(" + i + "," + j + ")").GetComponent<TileGameObject>();
            interactionManager.TileClicked(obj, i, j);
        }

        public void Trade(Player p1, Player p2, Resource[] p1Offer, Resource[] p2Offer)
        {
            Trader.Trade(p1, p2, p1Offer, p2Offer);
        }

        public bool RequestTrade(Player p1, Player p2, Resource[] p1Offer, Resource[] p2Offer)
        {
            return Trader.Request(p1, p2, p1Offer, p2Offer);
        }

        public bool BuildSettlement(Player p, int i, int j, bool starting = false)
        {
            TileVertexGameObject obj = GameObject.Find("Vertex(" + i + "," + j + ")")?.GetComponent<TileVertexGameObject>();
            if (obj == null) return false;
            if (board.vertices[i][j].development > 0) return false;
            return board.BuildVertex(p, (i, j), obj, starting);
        }

        public bool UpgradeSettlement(Player p, int i, int j)
        {
            TileVertexGameObject obj = GameObject.Find("Vertex(" + i + "," + j + ")")?.GetComponent<TileVertexGameObject>();
            if (obj == null) return false;
            if (board.vertices[i][j].development != TileVertex.Development.Town) return false;
            return board.BuildVertex(p, (i, j), obj);
        }

        public bool BuildRoad(Player p, int i, int j, bool starting = false)
        {
            RoadGameObject obj = GameObject.Find("Road(" + i + "," + j + ")")?.GetComponent<RoadGameObject>();
            if (obj == null) return false;
            return board.BuildRoad(p, (i, j), obj, starting);
        }

        public void AdvanceTurn()
        {
            gameManager.AdvanceTurn();
        }
    }
}
