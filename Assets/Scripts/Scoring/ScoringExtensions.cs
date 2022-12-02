/// AUTHOR: Matthew Moffitt
/// FILENAME: ScoringExtensions.cs
/// SPECIFICATION: Helps scoring
/// FOR: CS 3368 Introduction to Artificial Intelligence Section 001

using Catan.Players;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Catan.Scoring
{
    /// <summary>
    /// Responsible for helping scoring
    /// </summary>
    public static class ScoringExtensions
    {
        /// <summary>
        /// Returns the player with 10 or more victory points
        /// </summary>
        /// <param name="players"></param>
        /// <returns></returns>
        public static Player GetWinner(this Player[] players)
        {
            foreach (Player p in players)
            {
                if (p.victoryPoints >= 10)
                {
                    return p;
                }
            }

            return null;
        }
    }
}
