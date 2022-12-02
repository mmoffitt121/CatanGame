/// AUTHOR: Matthew Moffitt
/// FILENAME: Port.cs
/// SPECIFICATION: File containing board info
/// FOR: CS 3368 Introduction to Artificial Intelligence Section 001

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Catan.ResourcePhase;

namespace Catan.GameBoard
{
    /// <summary>
    /// class representing a port on the board
    /// </summary>
    public class Port
    {
        // Port resource ratio
        public int toGet = 3;
        public int toGive = 1;
        // Port coordinate
        public int xCoord;
        public int yCoord;
        // Port resource type
        public Resource.ResourceType type;

        /// <summary>
        /// Float representing the direction the port comes off the vertex in degrees
        /// </summary>
        public float direction = 0;
    }
}
