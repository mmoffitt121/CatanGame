using Catan.GameBoard;
using Catan.Players;
using Catan.ResourcePhase;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Catan.Settings
{
    public static class GameSettings
    {
        // Game Settings
        public static int maxTurns;
        public static int maxTradeAttempts;
        public static int maxBuildAttempts;

        public static bool animations;
        public static bool quickrolling;

        // Player Settings
        public static Player[] players;

        // Board Settings
        public static int chosenPreset = 0;
        public static BoardPreset[] presets = new BoardPreset[]
        {
            // ---
            // Classic Preset
            new BoardPreset()
            {
                name = "Classic",
                desc = "The classic Catan experience. 3-4 players recommended.",
                tileTypes = new Tile.TileType[]
                {
                    Tile.TileType.Pasture,
                    Tile.TileType.Field,
                    Tile.TileType.Forest,
                    Tile.TileType.Hills,
                    Tile.TileType.Mountains,
                    Tile.TileType.Desert
                },
                tileAmounts = new int[]
                {
                    4,
                    4,
                    4,
                    3,
                    3,
                    1
                },
                boardShape = new int[]
                {
                    3,
                    4,
                    5,
                    4,
                    3
                },
                diceValues = new int[]
                {
                    0,
                    0,
                    1,
                    2,
                    2,
                    2,
                    2,
                    0,
                    2,
                    2,
                    2,
                    2,
                    1
                },
                portTypes = new Resource.ResourceType[]
                {
                    Resource.ResourceType.Any,
                    Resource.ResourceType.Any,
                    Resource.ResourceType.Any,
                    Resource.ResourceType.Any,
                    Resource.ResourceType.Wool,
                    Resource.ResourceType.Grain,
                    Resource.ResourceType.Wood,
                    Resource.ResourceType.Brick,
                    Resource.ResourceType.Ore,
                },
                portAmounts = new int[]
                {
                    1,
                    1,
                    1,
                    1,
                    1,
                    1,
                    1,
                    1,
                    1
                }
            },
            // ---
            // 5-6 Player Expansion Preset
            new BoardPreset()
            {
                name = "Expanded",
                desc = "Classic Catan with the 5-6 player expansion. 5-6 players recommended.",
                tileTypes = new Tile.TileType[]
                {
                    Tile.TileType.Pasture,
                    Tile.TileType.Field,
                    Tile.TileType.Forest,
                    Tile.TileType.Hills,
                    Tile.TileType.Mountains,
                    Tile.TileType.Desert
                },
                tileAmounts = new int[]
                {
                    6,
                    6,
                    6,
                    5,
                    5,
                    2
                },
                boardShape = new int[]
                {
                    3,
                    4,
                    5,
                    6,
                    5,
                    4,
                    3
                },
                diceValues = new int[]
                {
                    0,
                    0,
                    2,
                    3,
                    3,
                    3,
                    3,
                    0,
                    3,
                    3,
                    3,
                    3,
                    2
                },
                portTypes = new Resource.ResourceType[]
                {
                    Resource.ResourceType.Any,
                    Resource.ResourceType.Any,
                    Resource.ResourceType.Any,
                    Resource.ResourceType.Any,
                    Resource.ResourceType.Any,
                    Resource.ResourceType.Wool,
                    Resource.ResourceType.Wool,
                    Resource.ResourceType.Grain,
                    Resource.ResourceType.Wood,
                    Resource.ResourceType.Brick,
                    Resource.ResourceType.Ore,
                },
                portAmounts = new int[]
                {
                    1,
                    1,
                    1,
                    1,
                    1,
                    1,
                    1,
                    1,
                    1,
                    1,
                    1
                }
            },
            // ---
            // Tiny Preset
            new BoardPreset()
            {
                name = "Tiny",
                desc = "Small and disjoint, this version of Catan is not recommended for any amount of players.",
                tileTypes = new Tile.TileType[]
                {
                    Tile.TileType.Pasture,
                    Tile.TileType.Field,
                    Tile.TileType.Forest,
                    Tile.TileType.Hills,
                    Tile.TileType.Mountains,
                    Tile.TileType.Desert
                },
                tileAmounts = new int[]
                {
                    2,
                    1,
                    1,
                    1,
                    1,
                    1
                },
                boardShape = new int[]
                {
                    2,
                    3,
                    2
                },
                diceValues = new int[]
                {
                    0,
                    0,
                    0,
                    1,
                    1,
                    1,
                    1,
                    0,
                    1,
                    1,
                    1,
                    1,
                    0
                },
                portTypes = new Resource.ResourceType[]
                {
                    Resource.ResourceType.Wool,
                    Resource.ResourceType.Grain,
                    Resource.ResourceType.Wood,
                    Resource.ResourceType.Brick,
                    Resource.ResourceType.Ore,
                },
                portAmounts = new int[]
                {
                    1,
                    1,
                    1,
                    1,
                    1
                }
            }
        };
    }
}
