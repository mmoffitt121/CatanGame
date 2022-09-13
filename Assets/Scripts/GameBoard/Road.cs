using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Catan.GameBoard
{
    public class Road
    {
        public int xCoord;
        public int yCoord;
        public int playerIndex;

        public Road(int x, int y)
        {
            xCoord = x;
            yCoord = y;
        }
    }
}
