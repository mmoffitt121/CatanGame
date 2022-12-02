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
    /// <summary>
    /// Agent class, base class for all AI agents
    /// </summary>
    public abstract class Agent
    {
        /// <summary>
        /// The player the agent is attached to
        /// </summary>
        public Player player;
        /// <summary>
        /// The API that interfaces with the game, board, and other players
        /// </summary>
        public AgentAPI api;

        /// <summary>
        /// Name of the agent
        /// </summary>
        public string agentName = "Unnamed Agent";
        /// <summary>
        /// The difficulty level of the agent
        /// </summary>
        public Difficulty difficulty = Difficulty.Medium;

        /// <summary>
        /// The max amount of trades an agent can perform
        /// </summary>
        public int maxTrades = 3;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="plyr"> The player for which the agent is being created </param>
        public Agent(Player plyr)
        {
            player = plyr;
        }

        /// <summary>
        /// Initializes the agent API
        /// </summary>
        public void Initialize()
        {
            api = GameObject.Find("AI Manager").GetComponent<AgentAPI>();
        }

        /// <summary>
        /// Rolls the dice
        /// </summary>
        public virtual void Roll()
        {
            api.Roll();
        }

        /// <summary>
        /// Chooses the location of the robber upon rolling a 7.
        /// </summary>
        public virtual void ChooseRobberLocation()
        {
            int i = UnityEngine.Random.Range(0, api.board.tiles.Length);
            int j = UnityEngine.Random.Range(0, api.board.tiles[i].Length);
            api.MoveRobber(i, j);
        }

        /// <summary>
        /// Is called when it is time for the player to discard after having more than 7 cards when a 7 is rolled.
        /// </summary>
        /// <param name="discardAmount"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Chooses a player to steal from upon rolling a 7.
        /// </summary>
        /// <param name="stealFrom"></param>
        /// <returns></returns>
        public virtual int ChooseSteal(Player[] stealFrom)
        {
            return UnityEngine.Random.Range(0, stealFrom.Length);
        }

        /// <summary>
        /// Chooses a location for the agent's starting piece.
        /// </summary>
        /// <param name="first"></param>
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

        /// <summary>
        /// Is called when the agent's trade phase begins
        /// </summary>
        public virtual void StartTrading()
        {
            api.AdvanceTurn();
        }

        /// <summary>
        /// Chooses whether or not the agent will accept a trade deal
        /// </summary>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        /// <param name="p1Offer"></param>
        /// <param name="p2Offer"></param>
        /// <returns></returns>
        public virtual bool ChooseAcceptTradeDeal(Player p1, Player p2, Resource[] p1Offer, Resource[] p2Offer)
        {
            return true;
        }

        /// <summary>
        /// Is called when the agent's build phase is to begin
        /// </summary>
        public virtual void StartBuilding()
        {
            api.AdvanceTurn();
        }

        /// <summary>
        /// Is called when someone the agent offered to accepts.
        /// </summary>
        /// <param name="accepted"></param>
        public virtual void OfferResultRecieved(bool accepted)
        {

        }

        /// <summary>
        /// Enumerated type representing the difficulty of the agent.
        /// </summary>
        public enum Difficulty
        {
            Easy,
            Medium,
            Hard
        }
    }
}