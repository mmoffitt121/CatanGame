using Mono.Cecil;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Catan.UI;

namespace Catan.Tests
{
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

