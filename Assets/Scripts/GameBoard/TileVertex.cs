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
    public class TileVertex
    {
        public int xCoord;
        public int yCoord;
        public int xDataIndex;
        public int yDataIndex;

        public Development development;
        public int playerIndex;
        public bool up;

        public Port port;

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
