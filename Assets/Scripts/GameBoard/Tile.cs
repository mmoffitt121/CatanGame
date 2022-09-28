using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Mesh;

namespace Catan.GameBoard
{
    public class Tile
    {
        public TileType type;
        public int diceValue;
        public int xDataIndex;
        public int yDataIndex;
        public int xCoord;
        public int yCoord;

        public bool robber;

        public Color color
        {
            get
            {
                switch (type)
                {
                    case TileType.Pasture:
                        return new Color(120 / 255f, 255 / 255f, 70 / 255f);
                    case TileType.Field:
                        return new Color(255 / 255f, 255 / 255f, 100 / 255f);
                    case TileType.Forest:
                        return new Color(50 / 255f, 150 / 255f, 70 / 255f);
                    case TileType.Hills:
                        return new Color(255 / 255f, 100 / 255f, 50 / 255f);
                    case TileType.Mountains:
                        return new Color(200 / 255f, 200 / 255f, 230 / 255f);
                    case TileType.Desert:
                        return new Color(230 / 255f, 230 / 255f, 160 / 255f);
                    default:
                        return Color.white;
                }
            }
        }

        public Tile(int x, int y, TileType tileType)
        {
            xDataIndex = x;
            yDataIndex = y;
            type = tileType;
        }

        public enum TileType
        {
            Pasture,
            Field,
            Forest,
            Hills,
            Mountains,
            Desert
        }
    }
}