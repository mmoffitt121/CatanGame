/// AUTHOR: Matthew Moffitt
/// FILENAME: Road.cs
/// SPECIFICATION: File containing board info
/// FOR: CS 3368 Introduction to Artificial Intelligence Section 001

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Catan.GameBoard
{
    /// <summary>
    /// Object representing the roads on the board
    /// </summary>
    public class Road
    {
        // Coordinates for the positions of the road on the board in DATA FORM and POSITIONAL FORM
        public int xCoord;
        public int yCoord;
        public int xDataIndex;
        public int yDataIndex;

        /// <summary>
        /// Index of player who owns the road
        /// </summary>
        public int playerIndex;

        public Road(int x, int y)
        {
            xDataIndex = x;
            yDataIndex = y;
            playerIndex = -1;
        }

        public override string ToString()
        {
            return xDataIndex + ", " + yDataIndex;
        }
    }
}
