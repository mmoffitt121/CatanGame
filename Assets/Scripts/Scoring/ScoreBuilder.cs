/// AUTHOR: Matthew Moffitt, Wuraola Alli
/// FILENAME: ScoreBuilder.cs
/// SPECIFICATION: File that determines a player's score
/// FOR: CS 3368 Introduction to Artificial Intelligence Section 001

using Catan.GameBoard;
using Catan.Players;
using Catan.Util;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

namespace Catan.Scoring
{
    public class ScoreBuilder : MonoBehaviour
    {
        public Board board;

        public void CalculateScores(Player[] players)
        {
            foreach (Player p in players)
            {
                p.victoryPoints = 0;
            }

            TileVertex[][] vertices = board.vertices;
            Road[][] roads = board.roads;
            List<(int, int)>[] ends = new List<(int, int)>[players.Length];

            for (int i = 0; i < ends.Length; i++)
            {
                ends[i] = new List<(int, int)>();
            }

            // Calculate scores from houses, grab houses for branching off to calculate longest road
            for (int i = 0; i < vertices.Length; i++)
            {
                for (int j = 0; j < vertices[i].Length; j++)
                {
                    (int, int) up = vertices.RoadAboveVertex(roads, i, j);
                    (int, int) down = vertices.RoadBelowVertex(roads, i, j);
                    (int, int) left = vertices.RoadLeftOfVertex(roads, i, j);
                    (int, int) right = vertices.RoadRightOfVertex(roads, i, j);

                    foreach (Player p in players)
                    {
                        if (vertices[i][j].playerIndex == p.playerIndex)
                        {
                            p.victoryPoints += (int)vertices[i][j].development;  
                        }
                        if ((up.Valid() && roads[up.Item1][up.Item2].playerIndex == p.playerIndex) || 
                            (down.Valid() && roads[down.Item1][down.Item2].playerIndex == p.playerIndex) || 
                            (left.Valid() && roads[left.Item1][left.Item2].playerIndex == p.playerIndex) ||
                            (right.Valid() && roads[right.Item1][right.Item2].playerIndex == p.playerIndex))
                        {
                            ends[p.playerIndex].Add((i, j));
                        }
                    }
                }
            }

            // Calculate longest road

            // First, get original longest road
            Player longestRoadHolder = players.FirstTrue(p => p.longestRoad);

            // Calculate road lengths
            for (int i = 0; i < ends.Length; i++)
            {
                int maxLen = 0;
                
                foreach ((int, int) h in ends[i])
                {
                    List<(int, int)> visited = new List<(int, int)>();
                    int len = CalculateRoadLength(h, vertices, roads, visited, i);
                    if (len > maxLen)
                    {
                        maxLen = len;
                    }
                }

                players[i].longestRoadLength = maxLen - 1;
            }

            // Set road lengths
            int max = 0;
            int maxIndex = 0;
            for (int i = 0; i < players.Length; i++)
            {
                if (players[i].longestRoadLength > max)
                {
                    max = players[i].longestRoadLength;
                    maxIndex = i;
                }
            }

            if (longestRoadHolder != null && max > longestRoadHolder.longestRoadLength && max >= 5 || max >= 5)
            {
                foreach (Player p in players)
                {
                    p.longestRoad = false;
                }
                longestRoadHolder = players.SelectMax(p => p.longestRoadLength);
                longestRoadHolder.longestRoad = true;
            }

            if (longestRoadHolder != null)
            {
                longestRoadHolder.victoryPoints += 2;
            }
        }

        public int CalculateRoadLength((int, int) current, TileVertex[][] vertices, Road[][] roads, List<(int, int)> visited, int playerIndex)
        {
            if (current == (-1, -1) || visited.Contains(current))
            {
                return 0;
            }

            (int, int) below = vertices.RoadBelowVertex(roads, current.Item1, current.Item2);
            (int, int) above = vertices.RoadAboveVertex(roads, current.Item1, current.Item2);
            (int, int) left = vertices.RoadLeftOfVertex(roads, current.Item1, current.Item2);
            (int, int) right = vertices.RoadRightOfVertex(roads, current.Item1, current.Item2);

            int verticalCount = 0;
            int leftCount = 0;
            int rightCount = 0;

            visited.Add(current);

            if (below.Valid() && roads[below.Item1][below.Item2].playerIndex == playerIndex)
            {
                verticalCount = CalculateRoadLength(vertices.VertexBelowVertex(current.Item1, current.Item2), vertices, roads, visited, playerIndex);
            }
            if (above.Valid() && roads[above.Item1][above.Item2].playerIndex == playerIndex)
            {
                verticalCount = CalculateRoadLength(vertices.VertexAboveVertex(current.Item1, current.Item2), vertices, roads, visited, playerIndex);
            }
            if (left.Valid() && roads[left.Item1][left.Item2].playerIndex == playerIndex)
            {
                leftCount = CalculateRoadLength(vertices.VertexLeftOfVertex(current.Item1, current.Item2), vertices, roads, visited, playerIndex);
            }
            if (right.Valid() && roads[right.Item1][right.Item2].playerIndex == playerIndex)
            {
                rightCount = CalculateRoadLength(vertices.VertexRightOfVertex(current.Item1, current.Item2), vertices, roads, visited, playerIndex);
            }

            visited.Remove(current);
            return Math.Max(verticalCount, Math.Max(leftCount, rightCount)) + 1;
        }
    }
}
