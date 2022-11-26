using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using System.Text;


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

