/// AUTHOR: Matthew Moffitt
/// FILENAME: Game.cs
/// SPECIFICATION: Responsible for Holding test data
/// FOR: CS 3368 Introduction to Artificial Intelligence Section 001

using System;

namespace Catan.Tests
{
    /// <summary>
    /// Holds data for a particular game.
    /// </summary>
    [Serializable]
    public class Game
    {
        public int turns;
        public int rounds;

        public int playerCount;
        public string[] names;
        public string[] agentTypes;
        public int[] victoryPoints;
        public int[] towns;
        public int[] cities;
        public int[] ports;
        public int[] longestRoadLength;
    }
}

