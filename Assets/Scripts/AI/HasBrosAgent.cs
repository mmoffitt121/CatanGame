/// AUTHOR: Wuraola Alli, Jett Graham, Evan Griffin, Matthew Moffitt, Alex Rizzo, Brandon Villalobos
/// FILENAME: Agent.cs
/// SPECIFICATION: File containing agent activities and information
/// FOR: CS 3368 Introduction to Artificial Intelligence Section 001

using Catan.Players;
using Catan.ResourcePhase;
using Catan.TradePhase;
using Catan.BuildPhase;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using Catan.GameBoard;
using System.Text;

namespace Catan.AI
{
    /// <summary>
    /// AI agent class
    /// </summary>
    public class HasBrosAgent : Agent
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="plyr"></param>
        public HasBrosAgent(Player plyr) : base(plyr)
        {
            agentName = "Has Bros Agent";
        }

        /// <summary>
        /// Override void that controls the placement of the AI player's starting piece
        /// </summary>
        /// <param name="first"></param>
        public override void PlaceStartingPiece(bool first = false)
        {
            // Grab vertices from board
            TileVertex[][] verts = api.board.vertices;
            Road[][] roads = api.board.roads;

            // First, calculate expected value for every resource for every vertex.

            // Triple disjointed array of floats, and triple disjointed array of Resource Types.
            // Outermost array represents vertex row, middle array represents vertex column, and innermost array represents the expected value of each resource for a given vertex.
            float[][][] EVs = new float[verts.Length][][];
            Resource.ResourceType[][][] resources = new Resource.ResourceType[verts.Length][][];
            // Ports
            Resource.ResourceType[][] ports = new Resource.ResourceType[verts.Length][];

            // Lists representing expected values already owned by the player
            List<float> ownedEVs = new List<float>();
            List<Resource.ResourceType> ownedResources = new List<Resource.ResourceType>();
            // Ports
            Resource.ResourceType[] ownedPorts = api.Vertices.GetPlayerPorts(player);

            for (int i = 0; i < verts.Length; i++)
            {
                EVs[i] = new float[verts[i].Length][];
                resources[i] = new Resource.ResourceType[verts[i].Length][];
                ports[i] = new Resource.ResourceType[verts[i].Length];

                for (int j = 0; j < verts[i].Length; j++)
                {
                    // Check base case where building is impossible due to adjacent developed vertex.
                    (int, int) above = verts.VertexAboveVertex(i, j);
                    (int, int) below = verts.VertexBelowVertex(i, j);
                    (int, int) left = verts.VertexLeftOfVertex(i, j);
                    (int, int) right = verts.VertexRightOfVertex(i, j);

                    // Case in which the vertex is not valid to build on
                    if (above.Valid() && verts[above.Item1][above.Item2].development > 0 ||
                        below.Valid() && verts[below.Item1][below.Item2].development > 0 ||
                        left.Valid() && verts[left.Item1][left.Item2].development > 0 ||
                        right.Valid() && verts[right.Item1][right.Item2].development > 0)
                    {
                        EVs[i][j] = new float[1] { -10000 };
                        resources[i][j] = new Resource.ResourceType[] { Resource.ResourceType.None };
                        continue;
                    }

                    // Case in which the vertex is already owned
                    if (verts[i][j].development > 0 && verts[i][j].playerIndex == player.playerIndex)
                    {
                        EVs[i][j] = new float[1] { -10000 };
                        resources[i][j] = new Resource.ResourceType[] { Resource.ResourceType.None };

                        (Resource.ResourceType, float)[] ownedEV = api.board.CalculateVertexExpectedValues(i, j);
                        foreach ((Resource.ResourceType, float) ev in ownedEV)
                        {
                            ownedEVs.Add(ev.Item2);
                            ownedResources.Add(ev.Item1);
                        }

                        continue;
                    }

                    if (verts[i][j].port != null)
                    {
                        ports[i][j] = verts[i][j].port.type;
                    }

                    // Calculate expected value of resource
                    (Resource.ResourceType, float)[] expectedValues = api.board.CalculateVertexExpectedValues(i, j);
                    EVs[i][j] = new float[expectedValues.Length];
                    resources[i][j] = new Resource.ResourceType[expectedValues.Length];

                    for (int k = 0; k < expectedValues.Length; k++)
                    {
                        EVs[i][j][k] = expectedValues[k].Item2;
                        resources[i][j][k] = expectedValues[k].Item1;
                    }
                }
            }

            int baseCost = 4;
            int grainCost = 4;
            int sheepCost = 4;
            int woodCost = 4;
            int brickCost = 4;
            int oreCost = 4;

            // Calculate initial port costs
            for (int i = 0; i < ownedPorts.Length; i++)
            {
                switch (ownedPorts[i])
                {
                    case Resource.ResourceType.Any:
                        baseCost = 3;
                        break;
                    case Resource.ResourceType.Grain:
                        grainCost = 2;
                        break;
                    case Resource.ResourceType.Wool:
                        sheepCost = 2;
                        break;
                    case Resource.ResourceType.Wood:
                        woodCost = 2;
                        break;
                    case Resource.ResourceType.Brick:
                        brickCost = 2;
                        break;
                    case Resource.ResourceType.Ore:
                        oreCost = 2;
                        break;
                }
            }

            grainCost = Math.Min(baseCost, grainCost);
            sheepCost = Math.Min(baseCost, sheepCost);
            woodCost = Math.Min(baseCost, woodCost);
            brickCost = Math.Min(baseCost, brickCost);
            oreCost = Math.Min(baseCost, oreCost);

            // Calculate space with highest expected value
            List<(int, int)> excluded = new List<(int, int)>();
            bool buildResult = false;
            float highest;
            int highestI = 0;
            int highestJ = 0;

            float settlementEV;
            float cityEV;
            // Keep trying to build until we succeed, in order of value for each vertex
            while (buildResult == false)
            {
                highest = -10000f;
                highestI = 0;
                highestJ = 0;
                for (int i = 0; i < EVs.Length; i++)
                {
                    for (int j = 0; j < EVs[i].Length; j++)
                    {
                        // If we already tried this one and it failed, continue;
                        if (excluded.Contains((i, j)))
                        {
                            continue;
                        }

                        // Calculate port costs for this vertex
                        int baCost = baseCost;
                        int gCost = grainCost;
                        int sCost = sheepCost;
                        int wCost = woodCost;
                        int bCost = brickCost;
                        int oCost = oreCost;

                        if (api.board.vertices[i][j].port != null)
                        {
                            switch (api.board.vertices[i][j].port.type)
                            {
                                case Resource.ResourceType.Any:
                                    baCost = 3;
                                    break;
                                case Resource.ResourceType.Grain:
                                    gCost = 2;
                                    break;
                                case Resource.ResourceType.Wool:
                                    sCost = 2;
                                    break;
                                case Resource.ResourceType.Wood:
                                    wCost = 2;
                                    break;
                                case Resource.ResourceType.Brick:
                                    bCost = 2;
                                    break;
                                case Resource.ResourceType.Ore:
                                    oCost = 2;
                                    break;
                            }
                        }

                        gCost = Math.Min(baCost, gCost);
                        sCost = Math.Min(baCost, sCost);
                        wCost = Math.Min(baCost, wCost);
                        bCost = Math.Min(baCost, bCost);
                        oCost = Math.Min(baCost, oCost);

                        float gEV = 0f;
                        float sEV = 0f;
                        float wEV = 0f;
                        float bEV = 0f;
                        float oEV = 0f;

                        for (int k = 0; k < ownedResources.Count; k++)
                        {
                            switch (ownedResources[k])
                            {
                                case Resource.ResourceType.Grain:
                                    gEV += ownedEVs[k];
                                    break;
                                case Resource.ResourceType.Wool:
                                    sEV += ownedEVs[k];
                                    break;
                                case Resource.ResourceType.Wood:
                                    wEV += ownedEVs[k];
                                    break;
                                case Resource.ResourceType.Brick:
                                    bEV += ownedEVs[k];
                                    break;
                                case Resource.ResourceType.Ore:
                                    oEV += ownedEVs[k];
                                    break;
                            }
                        }

                        for (int k = 0; k < resources[i][j].Length; k++)
                        {
                            switch (resources[i][j][k])
                            {
                                case Resource.ResourceType.Grain:
                                    gEV += EVs[i][j][k];
                                    break;
                                case Resource.ResourceType.Wool:
                                    sEV += EVs[i][j][k];
                                    break;
                                case Resource.ResourceType.Wood:
                                    wEV += EVs[i][j][k];
                                    break;
                                case Resource.ResourceType.Brick:
                                    bEV += EVs[i][j][k];
                                    break;
                                case Resource.ResourceType.Ore:
                                    oEV += EVs[i][j][k];
                                    break;
                            }
                        }
                        float WEIGHT_MODIFIER = 2f;
                        float anyResourceForSettlementEV = (gEV / gCost / WEIGHT_MODIFIER) + (sEV / sCost / WEIGHT_MODIFIER) + (wEV / wCost / WEIGHT_MODIFIER) + (bEV / bCost / WEIGHT_MODIFIER) + (oEV / oCost);
                        float anyResourceForCityEV = (gEV / gCost / WEIGHT_MODIFIER / 2) + (sEV / sCost) + (wEV / wCost) + (bEV / bCost) + (oEV / oCost / WEIGHT_MODIFIER / 3);

                        // Expected value of building a settlement on this spot
                        float seEV = (gEV + sEV + wEV + bEV + anyResourceForSettlementEV) / 4;
                        // Expected value of building a city on this spot
                        float ciEV = (oEV * 3 + gEV * 2 + anyResourceForCityEV) / 5;

                        if (Math.Max(seEV, ciEV) > highest)
                        {
                            highest = Math.Max(seEV, ciEV);
                            settlementEV = seEV;
                            cityEV = ciEV;
                            highestI = i;
                            highestJ = j;
                        }
                    }
                }

                buildResult = api.BuildSettlement(player, highestI, highestJ, true);

                // If the build fails for whatever reason, add to list of excluded values and continue.
                if (!buildResult)
                {
                    excluded.Add((highestI, highestJ));
                }
            }

            // Builds road
            (int, int) road;
            road = api.board.vertices.RoadAboveVertex(api.board.roads, highestI, highestJ);
            if (api.BuildRoad(player, road.Item1, road.Item2, true)) return;

            road = api.board.vertices.RoadBelowVertex(api.board.roads, highestI, highestJ);
            if (api.BuildRoad(player, road.Item1, road.Item2, true)) return;

            road = api.board.vertices.RoadLeftOfVertex(api.board.roads, highestI, highestJ);
            if (api.BuildRoad(player, road.Item1, road.Item2, true)) return;

            road = api.board.vertices.RoadRightOfVertex(api.board.roads, highestI, highestJ);
            if (api.BuildRoad(player, road.Item1, road.Item2, true)) return;

            if (first)
            {
                api.board.DistributeResourcesFromVertex((highestI, highestJ));
            }
        }

        private int trades;
        public override void StartTrading()
        {
            Player p = api.Players[UnityEngine.Random.Range(0, api.Players.Length)];
            if (p.resourceSum > 0 && p.playerIndex != player.playerIndex)
            {
                Resource[] senderOffer = new Resource[] { player.RandomResource() };
                Resource[] recieverOffer = new Resource[] { p.RandomResource() };
                Trader.Request(player, p, senderOffer, recieverOffer);
            }
            else
            {
                OfferResultRecieved(false);
            }
        }

        public override void OfferResultRecieved(bool accepted)
        {
            trades++;
            if (trades >= maxTrades)
            {
                trades = 0;
                base.StartTrading();
            }
            else
            {
                StartTrading();
            }
        }

        public override void StartBuilding()
        {
            while (true)
            {
                bool canBuildSettlement = player.HasResource(Resource.ResourceType.Brick)
                    && player.HasResource(Resource.ResourceType.Wood)
                    && player.HasResource(Resource.ResourceType.Grain)
                    && player.HasResource(Resource.ResourceType.Wool);

                bool canBuildCity = player.HasResource(Resource.ResourceType.Grain, 2)
                    && player.HasResource(Resource.ResourceType.Ore, 3);

                bool canBuildRoad = player.HasResource(Resource.ResourceType.Brick)
                    && player.HasResource(Resource.ResourceType.Wood);

                (int, int)[] possibleSettlements = api.board.GetPossibleSettlements(player);
                (int, int)[] possibleCities = api.board.GetPossibleCities(player);
                (int, int)[] possibleRoads = api.board.GetPossibleRoads(player);

                if (canBuildSettlement && possibleSettlements.Length > 0)
                {
                    int i = UnityEngine.Random.Range(0, possibleSettlements.Length);
                    api.BuildSettlement(player, possibleSettlements[i].Item1, possibleSettlements[i].Item2);
                }
                else if (canBuildCity && possibleCities.Length > 0)
                {
                    int i = UnityEngine.Random.Range(0, possibleCities.Length);
                    api.UpgradeSettlement(player, possibleCities[i].Item1, possibleCities[i].Item2);
                }
                else if (canBuildRoad && possibleRoads.Length > 0)
                {
                    int i = UnityEngine.Random.Range(0, possibleRoads.Length);
                    api.BuildRoad(player, possibleRoads[i].Item1, possibleRoads[i].Item2);
                }
                else
                {
                    break;
                }
            }

            base.StartBuilding();
        }

        /*public class BuildPhase
        {
            private Player [] players;
            private int currentPlayer;
            private List<LogEvent> log;

            public BuildPhase( IBoard board, List<DevelopmentCard> deck, int[] resourceBank, Player[] players, int currentPlayer, List<LogEvent> log, int longestRoad, int largestArmy)
            {
                Board = board;
                DevelopmentCards = deck == null ? 0 : deck.Count;
                ResourceBank =  resourceBank == null ? null : resourceBank.ToArray();
                this.players = players;
                this.currentPlayer= currrentPlayer;
                this.log = log;
                if( players == null) players = new Player[0];
                AllPlayerIds =  players.Select(p => p.Id). ToArray();
                LongestRoadId = longestRoad;
                LargestArmyId = largestArmy;
            }
            public IBoard Board 
            {get; private set;}
            public int DevelopmentCards {get; private set;}
            public int [] ResourceBank{ get; private set;}
            public int [] AllPlayerIds {get; private set;}
            public int LongestRoadId{get; private set;}
            public int LargestArmyId{ get; private set;}

            public int GetPlayerScore(int playerId)
            {
                int result =0;
                if (playerId == LargestArmyId) result += 2;
                if(playerId == LongestRoadId) result += 2;
                // 2 per city and 1 per settlement
                Board.GetAllPieces(). Where(p => p.Value.Player == playerId). ForEach(p => result += p.Value.Token == Token.City ? 2 : 1);
                return result;
            }
            public int GetRoundNumber()
            {
                return log.OfType<RollLogEvent>() .Where(r.Player == 0). Count();

            }
            public int GetResourceCount (int playerId)
            {
                return players[playerID].Resources.Count;
    
            }
            public int GetKnghtCount(int playerID)
            {
                return players[playerID]. PlayedKnights;
            }

            public int GetSettlementsLeft(int playerID)
            {
                return players[playersID].SettlementsLeft;
            }
            public int GetCitiesLeft(int playerID)
            {
        return players[playerID]. CitiesLeft;
            }
            public int GetRoadsLeft (int playerID)
            {
                return players[playerID]. RoadsLeft;
    
            }
            public Resource[] GetOwnResources()
            {
                return players[currentPlayer].Resources.ToArray();
    
            }
            public DevelopmentCard[] GetOwnDevelopmentCards()
            {
                return players[currentPlayer].DevelopmentCards.ToArray();

            }
            public int GetResourceBank(Resource res)
            {

            return ResourceBank[(int)res];

            }

            public List<LogEvent> GetLatestEvents (int amount)
            {
                return log.Skip(Math.Max(0, log.Count() - amount)). Take(amount).ToList();

            }
            public List<LogEvent> GetEventsSince(DateTime time)
            {
                return log.Where(e => e.TimeStamp. CompareTo(time) > 0).ToList();

            }
        }*/
    }
}
