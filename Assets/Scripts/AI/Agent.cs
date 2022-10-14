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
using Catan.GameBoard;

namespace Catan.AI
{
    /// <summary>
    /// AI agent class
    /// </summary>
    public class Agent
    {
        public Player player;
        public AgentAPI api;

        public Agent(Player plyr)
        {
            player = plyr;
            api = GameObject.Find("AI Manager").GetComponent<AgentAPI>();
        }

        public void Roll()
        {
            api.Roll();
        }

        public void ChooseRobberLocation()
        {
            int i = UnityEngine.Random.Range(0, api.board.tiles.Length);
            int j = UnityEngine.Random.Range(0, api.board.tiles[i].Length);
            api.MoveRobber(i, j);
        }

        public Resource[] ChooseDiscard(int discardAmount)
        {
            return new Resource[] { new Resource(Resource.ResourceType.Wool, discardAmount)
            ,new Resource(Resource.ResourceType.Wood, 0)
            ,new Resource(Resource.ResourceType.Ore, 0)
            ,new Resource(Resource.ResourceType.Brick, 0)
            ,new Resource(Resource.ResourceType.Grain, 0)};
        }

        public int ChooseSteal(Player[] stealFrom)
        {
            return UnityEngine.Random.Range(0, stealFrom.Length);
        }

        public void PlaceStartingPiece(bool first = false)
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

        public void StartTrading()
        {

        }

        public void StartBuilding()
        {

        }
    }
}
