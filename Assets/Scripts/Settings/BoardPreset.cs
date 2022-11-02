using Catan.GameBoard;
using Catan.Players;
using Catan.ResourcePhase;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Catan.Settings
{
    public class BoardPreset
    {
        public string name;
        public string desc;
        public Tile.TileType[] tileTypes;
        public int[] tileAmounts;
        public int[] boardShape;
        public int[] diceValues;
        public Resource.ResourceType[] portTypes;
        public int[] portAmounts;

        public BoardPreset()
        {

        }

        public BoardPreset(string name, string desc, Tile.TileType[] tileTypes, int[] tileAmounts, int[] boardShape, int[] diceValues, Resource.ResourceType[] portTypes, int[] portAmounts)
        {
            this.name = name;
            this.desc = desc;
            this.tileTypes = tileTypes;
            this.tileAmounts = tileAmounts;
            this.boardShape = boardShape;
            this.diceValues = diceValues;
            this.portTypes = portTypes;
            this.portAmounts = portAmounts;
        }
    }
}
