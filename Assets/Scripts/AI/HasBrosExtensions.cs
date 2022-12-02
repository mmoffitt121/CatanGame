using Catan.GameBoard;
using Catan.ResourcePhase;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Catan.AI
{
    public static class HasBrosExtensions
    {
        /// <summary>
        /// Gets the expected value of rolling two dice to hit a specific target
        /// </summary>
        /// <param name="integer"></param>
        /// <param name="diceNumber"></param>
        /// <param name="diceMax"></param>
        /// <returns></returns>
        public static float DiceExpectedValue(this int target, int diceMax = 6)
        {
            float numberOfRolls = 0;
            for (int i = 1; i < diceMax + 1; i++)
            {
                for (int j = 1; j < diceMax + 1; j++)
                {
                    if (i + j == target)
                    {
                        numberOfRolls++;
                    }
                }
            }
            return numberOfRolls / (diceMax * diceMax);
        }

        /// <summary>
        /// Calculates the expected value of a vertex's resource yield
        /// </summary>
        /// <param name="board"></param>
        /// <param name="i"> Representing i index of vertex </param>
        /// <param name="j"> Representing j index of vertex </param>
        /// <returns> Returns a float representing the expected value of that vertex </returns>
        public static (Resource.ResourceType resource, float)[] CalculateVertexExpectedValues(this Board board, int i, int j)
        {
            // List of expected values per resource
            List<(Resource.ResourceType resource, float)> list = new List<(Resource.ResourceType resource, float)>();

            (int, int)[] verts = new (int, int)[4];

            verts[0] = board.vertices.TileAboveVertex(board.tiles, i, j);
            verts[1] = board.vertices.TileBelowVertex(board.tiles, i, j);
            verts[2] = board.vertices.TileLeftOfVertex(board.tiles, i, j);
            verts[3] = board.vertices.TileRightOfVertex(board.tiles, i, j);

            foreach ((int, int) v in verts)
            {
                if (!v.Valid()) { continue; }

                Tile t = board.tiles[v.Item1][v.Item2];

                Resource.ResourceType tileResource = t.resourceType;
                float expectedValue = t.diceValue.DiceExpectedValue();

                list.Add((tileResource, expectedValue));
            }

            return list.ToArray();
        }
    }
}
