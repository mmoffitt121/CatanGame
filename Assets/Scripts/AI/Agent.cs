using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Catan.Players;
using Catan.ResourcePhase;
using Catan.TradePhase;
using Catan.BuildPhase;
using System.Linq;
using Catan.GameBoard;

namespace Catan.AI
{
    public abstract class Agent
    {
        public Player player;
        public AgentAPI api;

        public int maxTrades = 3;

        public Agent(Player plyr)
        {
            player = plyr;
            api = GameObject.Find("AI Manager").GetComponent<AgentAPI>();
        }

        public virtual void Roll()
        {
            api.Roll();
        }

        public virtual void ChooseRobberLocation()
        {
            int i = UnityEngine.Random.Range(0, api.board.tiles.Length);
            int j = UnityEngine.Random.Range(0, api.board.tiles[i].Length);
            api.MoveRobber(i, j);
        }

        public virtual Resource[] ChooseDiscard(int discardAmount)
        {
            Resource[] discard = new Resource[] { new Resource(Resource.ResourceType.Wool, 0)
            ,new Resource(Resource.ResourceType.Wood, 0)
            ,new Resource(Resource.ResourceType.Ore, 0)
            ,new Resource(Resource.ResourceType.Brick, 0)
            ,new Resource(Resource.ResourceType.Grain, 0)};

            int count = 0;
            while (count != discardAmount)
            {
                int rIndex = UnityEngine.Random.Range(0, discard.Length);
                int rMax = rIndex + discard.Length;
                for (int i = 0; i < rMax; i++)
                {
                    // If we're discarding less of the randomly chosen resource than the player has, add one to the total. Else, continue searching.
                    if (discard[i % discard.Length].amount < player.resources.Where(r => r.type == discard[i % discard.Length].type).First().amount)
                    {
                        discard[i % discard.Length].amount += 1;
                        count++;
                        break;
                    }
                }
            }

            return discard;
        }

        public virtual int ChooseSteal(Player[] stealFrom)
        {
            return UnityEngine.Random.Range(0, stealFrom.Length);
        }

        public virtual void PlaceStartingPiece(bool first = false)
        {
            int i;
            int j;
            while (true)
            {
                i = UnityEngine.Random.Range(0, api.board.vertices.Length);
                j = UnityEngine.Random.Range(0, api.board.vertices[i].Length);

                if (api.BuildSettlement(player, i, j, true)) break;
            }

            if (first)
            {
                api.board.DistributeResourcesFromVertex((i, j));
            }

            (int, int) road;
            road = api.board.vertices.RoadAboveVertex(api.board.roads, i, j);
            if (api.BuildRoad(player, road.Item1, road.Item2, true)) return;

            road = api.board.vertices.RoadBelowVertex(api.board.roads, i, j);
            if (api.BuildRoad(player, road.Item1, road.Item2, true)) return;

            road = api.board.vertices.RoadLeftOfVertex(api.board.roads, i, j);
            if (api.BuildRoad(player, road.Item1, road.Item2, true)) return;

            road = api.board.vertices.RoadRightOfVertex(api.board.roads, i, j);
            if (api.BuildRoad(player, road.Item1, road.Item2, true)) return;
        }

        public virtual void StartTrading()
        {
            api.gameManager.AdvanceTurn();
        }

        public virtual bool ChooseAcceptTradeDeal(Player p1, Player p2, Resource[] p1Offer, Resource[] p2Offer)
        {
            return true;
        }

        public virtual void StartBuilding()
        {
            
        }
    }
}