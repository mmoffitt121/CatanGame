using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Catan.GameBoard
{
    public class Road
    {
        public int xCoord;
        public int yCoord;
        public int xDataIndex;
        public int yDataIndex;

        public int playerIndex;

        public Road(int x, int y)
        {
            xDataIndex = x;
            yDataIndex = y;
            playerIndex = -1;
        }
    }
}
