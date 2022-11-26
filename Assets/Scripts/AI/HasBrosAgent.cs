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
        public HasBrosAgent(Player plyr) : base(plyr)
        {
            agentName = "Has Bros Agent";
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
            public class BuildPhase : BuildPhase
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
}
        }
    }
}
