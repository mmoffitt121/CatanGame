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
            agentName = "Random Agent";
        }

        public override void StartTrading()
        {
            for (int i = 0; i < maxTrades; i++)
            {
                Player p = api.gameManager.players[UnityEngine.Random.Range(0, api.gameManager.players.Length)];
                if (p.resourceSum > 0 && p.playerIndex != player.playerIndex)
                {
                    Resource[] senderOffer = new Resource[] { player.RandomResource() };
                    Resource[] recieverOffer = new Resource[] { p.RandomResource() };
                    Trader.Request(player, p, senderOffer, recieverOffer);
                }
            }

            base.StartTrading();
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
