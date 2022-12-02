/// AUTHOR: Matthew Moffitt
/// FILENAME: BoardPreset.cs
/// SPECIFICATION: Holds preset board data
/// FOR: CS 3368 Introduction to Artificial Intelligence Section 001

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
        /// <summary>
        /// Preset Name
        /// </summary>
        public string name;
        /// <summary>
        /// Preset Description
        /// </summary>
        public string desc;
        /// <summary>
        /// Array of tile types
        /// </summary>
        public Tile.TileType[] tileTypes;
        /// <summary>
        /// Array of tile amounts, corresponding with tileTypes
        /// </summary>
        public int[] tileAmounts;
        /// <summary>
        /// Array representing shape of board, with the number in each index representing the amount of tiles in that index's corresponding row
        /// </summary>
        public int[] boardShape;
        /// <summary>
        /// Array representing commonality of each dice value's appearance on the board
        /// </summary>
        public int[] diceValues;
        /// <summary>
        /// Array representing resource types for ports
        /// </summary>
        public Resource.ResourceType[] portTypes;
        /// <summary>
        /// Array representing resource amounts for ports
        /// </summary>
        public int[] portAmounts;

        /// <summary>
        /// Constructor
        /// </summary>
        public BoardPreset()
        {

        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="name"></param>
        /// <param name="desc"></param>
        /// <param name="tileTypes"></param>
        /// <param name="tileAmounts"></param>
        /// <param name="boardShape"></param>
        /// <param name="diceValues"></param>
        /// <param name="portTypes"></param>
        /// <param name="portAmounts"></param>
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
