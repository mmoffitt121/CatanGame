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
using System.Text;

namespace Catan.Players
{
    /// <summary>
    /// Class holding data for a player
    /// </summary>
    public class Player
    {
        // A player's colors
        public Color playerColor;
        public Color primaryUIColor;
        public Color secondaryUIColor;

        /// <summary>
        /// Player name
        /// </summary>
        public string playerName;
        /// <summary>
        /// Index of player in player order
        /// </summary>
        public int playerIndex;
        /// <summary>
        /// Amount of victory points a player has earned. Calculated every turn.
        /// </summary>
        public int victoryPoints;

        /// <summary>
        /// True of a player is an AI agent
        /// </summary>
        public bool isAI;
        /// <summary>
        /// Player's attached AI agent that controls its behavior
        /// </summary>
        public Agent agent;

        /// <summary>
        /// Length of a player's longest road
        /// </summary>
        public int longestRoadLength;
        /// <summary>
        /// Whether a player has the longest road
        /// </summary>
        public bool longestRoad;

        /// <summary>
        /// List of a player's owned resources. Should always have every resource, even if amount is 0.
        /// </summary>
        public Resource[] resources;

        /// <summary>
        /// Sum of player's resources
        /// </summary>
        public int resourceSum
        {
            get
            {
                int sum = 0;
                foreach (Resource r in resources)
                {
                    if (r != null) sum += r.amount;
                }
                return sum;
            }
        }

        /// <summary>
        /// Grants the player a resource
        /// </summary>
        /// <param name="resource"></param>
        /// <param name="amount"></param>
        public void AddResource(Resource.ResourceType resource, int amount)
        {
            if (resource == Resource.ResourceType.None || resource == Resource.ResourceType.Any)
            {
                return;
            }
            resources.Where(rs => rs.type == resource).First().amount += amount;
        }

        /// <summary>
        /// Chooses a random resource from the player's resources
        /// </summary>
        /// <returns></returns>
        public Resource RandomResource()
        {
            int rand = Random.Range(0, resources.Length);
            int rIndex = rand;

            for (int i = 0; i < resources.Length; i++)
            {
                if (resources[(i + rand) % resources.Length].amount > 0)
                {
                    rIndex = (i + rand) % resources.Length;
                }
            }

            return new Resource(resources[rIndex].type, 1);
        }

        /// <summary>
        /// Returns true if a player has the specified resource
        /// </summary>
        /// <param name="toTest"></param>
        /// <param name="amount"></param>
        /// <returns></returns>
        public bool HasResource(Resource.ResourceType toTest, int amount = 1)
        {
            return resources.Where(r => r.type == toTest).First().amount >= amount;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="ai"></param>
        public Player(bool ai = false)
        {
            isAI = ai;
            if (ai)
            {
                agent = new RandomAgent(this);
            }
        }

        /// <summary>
        /// Returns the player as a string with the resources it has.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            string sb = "";
            sb += "'" + playerName + "' with resources: ";
            foreach (Resource r in resources)
            {
                sb += r.ToString();
            }

            return sb;
        }
    }
}
