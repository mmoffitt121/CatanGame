using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Catan.GameBoard
{
    public class TileVertex
    {
        public int xCoord;
        public int yCoord;
        public Development development;
        public int playerIndex;
        public bool up;

        public TileVertex(int x, int y, bool isUp)
        {
            xCoord = x;
            yCoord = y;
            up = isUp;
        }

        public enum Development
        {
            Undeveloped = 0,
            Town = 1,
            City = 2
        }
    }
}
