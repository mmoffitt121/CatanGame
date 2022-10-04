/// AUTHOR: Matthew Moffitt, Evan Griffin
/// FILENAME: Player.cs
/// SPECIFICATION: File that holds player data
/// FOR: CS 3368 Introduction to Artificial Intelligence Section 001

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Catan.ResourcePhase;
using Catan.AI;
using System.Linq;

namespace Catan.Players
{
    public class Player
    {
        public Color playerColor;

        public Color primaryUIColor;
        public Color secondaryUIColor;
        public string playerName;
        public int playerIndex;
        public int victoryPoints;

        public bool isAI;
        public Agent agent;

        public int longestRoadLength;
        public bool longestRoad;

        public Resource[] resources;

        public void AddResource(Resource.ResourceType resource, int amount)
        {
            if (resource == Resource.ResourceType.None || resource == Resource.ResourceType.Any)
            {
                return;
            }
            resources.Where(rs => rs.type == resource).First().amount += amount;
        }
    }
}
