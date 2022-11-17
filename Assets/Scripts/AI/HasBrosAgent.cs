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

namespace Catan.AI
{
    /// <summary>
    /// AI agent class
    /// </summary>
    public class HasBrosAgent : Agent
    {
        public HasBrosAgent(Player plyr) : base(plyr)
        {
            agentName = "Has Bros Agent";
        }

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

            for (int i = 0; i < verts.Length; i++)
            {
                EVs[i] = new float[verts[i].Length][];
                resources[i] = new Resource.ResourceType[verts[i].Length][];

                for (int j = 0; j < verts[i].Length; j++)
                {
                    // Check base case where building is impossible due to adjacent developed vertex.
                    (int, int) above = verts.VertexAboveVertex(i, j);
                    (int, int) below = verts.VertexBelowVertex(i, j);
                    (int, int) left = verts.VertexLeftOfVertex(i, j);
                    (int, int) right = verts.VertexRightOfVertex(i, j);

                    if (verts[i][j].development > 0 ||
                        above.Valid() && verts[above.Item1][above.Item2].development > 0 ||
                        below.Valid() && verts[below.Item1][below.Item2].development > 0 ||
                        left.Valid() && verts[left.Item1][left.Item2].development > 0 ||
                        right.Valid() && verts[right.Item1][right.Item2].development > 0)
                    {
                        EVs[i][j] = new float[1] { -10000 };
                        resources[i][j] = new Resource.ResourceType[] { Resource.ResourceType.None };
                        continue;
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

            // Calculate space with highest expected value
            float highest = -10000f;
            int highestI = 0;
            int highestJ = 0;
            for (int i = 0; i < EVs.Length; i++)
            {
                for (int j = 0; j < EVs[i].Length; j++)
                {
                    float ev = 0;
                    for (int k = 0; k < EVs[i][j].Length; k++)
                    {
                        ev += EVs[i][j][k];
                    }

                    if (ev > highest)
                    {
                        highest = ev;
                        highestI = i;
                        highestJ = j;
                    }
                }
            }

            bool result = api.BuildSettlement(player, highestI, highestJ, true);

            if (!result)
            {
                Debug.Log(highestI + " " + highestJ + " failed!");
            }

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

            // Depreciated

            /*int i;
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
            if (api.BuildRoad(player, road.Item1, road.Item2, true)) return;*/
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
    }
}
