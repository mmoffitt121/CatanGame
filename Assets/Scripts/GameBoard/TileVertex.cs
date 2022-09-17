using System.Collections;
using System.Collections.Generic;
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
    }
}
