using Catan.Players;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Catan.Scoring
{
    public static class ScoringExtensions
    {
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
