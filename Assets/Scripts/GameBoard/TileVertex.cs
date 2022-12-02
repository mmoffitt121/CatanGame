/// AUTHOR: Matthew Moffitt
/// FILENAME: TileVertex.cs
/// SPECIFICATION: File containing board info
/// FOR: CS 3368 Introduction to Artificial Intelligence Section 001

using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Catan.GameBoard
{
    /// <summary>
    /// Class representing each vertex on the game board
    /// </summary>
    public class TileVertex
    {
        // Coordinates representing the location of the tilevertex
        public int xCoord;
        public int yCoord;
        public int xDataIndex;
        public int yDataIndex;

        /// <summary>
        /// Development level of vertex
        /// </summary>
        public Development development;
        /// <summary>
        /// Index of player on vertex
        /// </summary>
        public int playerIndex;
        /// <summary>
        /// Switch boolean representing whether a vertex has a road going up out of it, or down out of it.
        /// </summary>
        public bool up;

        /// <summary>
        /// Port attached to vertex.
        /// </summary>
        public Port port;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="isUp"></param>
        public TileVertex(bool isUp)
        {
            up = isUp;
            playerIndex = -1;
        }

        public enum Development
        {
            Undeveloped = 0,
            Town = 1,
            City = 2
        }

        /// <summary>
        /// Increases development of tile vertex
        /// </summary>
        public void AdvanceDevelopment()
        {
            switch (development)
            {
                case Development.Undeveloped:
                    development = Development.Town;
                    break;
                case Development.Town:
                    development = Development.City;
                    break;
                case Development.City:
                    break;
                default:
                    break;
            }
        }

        public override string ToString()
        {
            return xDataIndex + ", " + yDataIndex;
        }
    }
}
