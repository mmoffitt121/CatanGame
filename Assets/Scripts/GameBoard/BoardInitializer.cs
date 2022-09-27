using System;
using UnityEngine;
using Catan.ResourcePhase;
using System.Collections.Generic;

namespace Catan.GameBoard
{
    public class BoardInitializer : MonoBehaviour
    {
        /// <summary>
        /// A list of types of tiles that will be loaded onto the board. 
        /// </summary>
        public Tile.TileType[] tileTypes;
        /// <summary>
        /// A list of the amounts of each tile specified at the equivelant index in the tileTypes array.
        /// </summary>
        public int[] tileAmount;
        /// <summary>
        /// Each element corresponds to a row on the board, and each number details how many are in each row.
        /// </summary>
        public int[] boardShape;
        /// <summary>
        /// Each element corresponds to how many tiles of its index will be placed on the board.
        /// </summary>
        public int[] diceValues;
        /// <summary>
        /// A list of resource types that will be used as ports
        /// </summary>
        public Resource.ResourceType[] ports;
        /// <summary>
        /// Each element corresponds to how many of each type of port corresponding to its index will be placed on the board.
        /// </summary>
        public int[] portAmounts;

        /// <summary>
        /// Dictionary that takes tuple as key, with values representing if there's a tile (above, below, to the left of, and to the right of) the current point.
        /// 
        /// A value of -1 represents that the port angle does not exist.
        /// A value of -2 represents that the angle can be anything.
        /// </summary>
        public static readonly Dictionary<(bool, bool, bool, bool), float> PORT_ANGLES = new Dictionary<(bool, bool, bool, bool), float>()
        {
            { (false, false, false, false),   0f },
            { (false, false, false, true ),  0f },
            { (false, false, true , false),   0f },
            { (false, false, true , true ),    0f },
            { (false, true , false, false),  0f },
            { (false, true , false, true ),  0f },
            { (false, true , true , false), 0f },
            { (false, true , true , true ),   -100f },
            { (true , false, false, false),    0f },
            { (true , false, false, true ),   0f },
            { (true , false, true , false),  0f },
            { (true , false, true , true ),   -100f },
            { (true , true , false, false),   -100f },
            { (true , true , false, true ),   -100f },
            { (true , true , true , false),   -100f },
            { (true , true , true , true ),   -100f }
        };

        public Board board;

        /// <summary>
        /// Is called when the scene loads
        /// </summary>
        public void Initialize()
        {
            board = GameObject.Find("Board").GetComponent(typeof(Board)) as Board;
            board.ClearTiles();
            board.tiles = Randomize();
            board.vertices = InitializeVertices(board.tiles);
            board.roads = InitializeRoads(board.vertices, board.tiles);
            board.PlaceTiles();
            board.PlaceVertices();
            board.PlaceRoads();
            board.PrintTest();
            InitializePorts(board.vertices, board.tiles);
            board.PlacePorts();
        }

        /// <summary>
        /// Returns a randomized board of tiles. Responsible for generation of tiles.
        /// </summary>
        /// <returns></returns>
        public Tile[][] Randomize()
        {
            int boardHeight = boardShape.Length;
            Tile[][] tileArray = new Tile[boardHeight][];

            if (tileAmount.Length < tileTypes.Length)
            {
                Debug.LogError("Amount not specified for some tiles");
                return null;
            }

            int[] tAmounts = (int[])tileAmount.Clone();
            int[] dValues = (int[])diceValues.Clone();

            for (int i = 0; i < boardHeight; i++)
            {
                tileArray[i] = new Tile[boardShape[i]];
                for (int j = 0; j < tileArray[i].Length; j++)
                {
                    tileArray[i][j] = new Tile(i, j, GetRandomTileType(tAmounts, tileTypes));
                    if (tileArray[i][j].type != Tile.TileType.Desert)
                    {
                        tileArray[i][j].diceValue = GetRandomDiceValue(dValues);
                    }
                    else
                    {
                        tileArray[i][j].robber = true;
                    }
                }
            }
            return tileArray;
        }

        /// <summary>
        /// Creates an array of vertices from an array of tiles. Responsible for generation of vertices.
        /// </summary>
        /// <param name="tiles"></param>
        /// <returns></returns>
        public TileVertex[][] InitializeVertices(Tile[][] tiles)
        {
            if (tiles == null || tiles[0] == null)
            {
                Debug.LogError("No tiles were found.");
                return null;
            }

            int boardHeight = boardShape.Length;
            TileVertex[][] vertices = new TileVertex[boardHeight + 1][];

            for (int i = 0; i < boardHeight + 1; i++)
            {
                int aboveCount = boardShape[Math.Clamp(i - 1, 0, boardHeight - 1)];
                int belowCount = boardShape[Math.Clamp(i, 0, boardHeight - 1)];

                int heightstagger = 1;
                int remainder = 1;

                // If there are tiles below and above the current row, remainder = 1 if the amount of tiles in each is not equal.
                if (i - 1 >= 0 && i < boardHeight && boardShape[i] == boardShape[i - 1])
                {
                    remainder = 2;
                    // If the parity of the row number is 0, the height stagger is reversed.
                    if (i % 2 == 0)
                    {
                        heightstagger = 0;
                    }
                }

                // If there are more tiles above the row than below, then the height stagger is reversed.
                if (i - 1 >= 0 && i < boardHeight && boardShape[i] < boardShape[i - 1] || i == boardHeight)
                {
                    heightstagger = 0;
                }

                // Initialize row
                vertices[i] = new TileVertex[Math.Max(aboveCount, belowCount) * 2 + remainder];

                // Populate row
                for (int j = 0; j < vertices[i].Length; j++)
                {
                    bool up = (j + heightstagger) % 2 == 1;
                    vertices[i][j] = new TileVertex(up);
                }
            }

            return vertices;
        }

        public void InitializePorts(TileVertex[][] vertices, Tile[][] tiles)
        {
            int[] pAmounts = (int[])portAmounts.Clone();

            /*foreach (TileVertex[] v0 in vertices)
                foreach (TileVertex v in v0)
                    Debug.Log(v.xCoord + " " + v.yCoord);

            foreach (Tile[] t0 in tiles)
                foreach (Tile t in t0)
                    Debug.Log(t.xCoord + " " + t.yCoord);*/

            int max = 0;
            for (int i = 0; i < tiles.Length; i++)
            {
                if (tiles[i].Length > max)
                {
                    max = tiles[i].Length;
                }
            }
            max += 2;

            for (int i = 0; i < vertices.Length; i++)
            {
                for (int j = 0; j < vertices[i].Length; j++)
                {
                    bool above = vertices.TileAboveVertex(tiles, i, j, max) != (-1, -1);
                    bool below = vertices.TileBelowVertex(tiles, i, j, max) != (-1, -1);
                    bool left = vertices.TileLeftOfVertex(tiles, i, j) != (-1, -1);
                    bool right = vertices.TileRightOfVertex(tiles, i, j) != (-1, -1);

                    Resource.ResourceType t;
                    try
                    {
                        t = GetRandomPortType(pAmounts, ports);
                    }
                    catch
                    {
                        return;
                    }

                    vertices[i][j].port = new Port();
                    vertices[i][j].port.type = t;
                    float direction = PORT_ANGLES[(above, below, left, right)];
                    vertices[i][j].port.direction = direction;
                }
            }
        }

        public Road[][] InitializeRoads(TileVertex[][] vertices, Tile[][] tiles)
        {
            if (vertices == null || vertices[0] == null)
            {
                Debug.LogError("No vertices were found.");
                return null;
            }

            int boardHeight = boardShape.Length;
            Road[][] roads = new Road[boardHeight * 2 + 1][];

            for (int i = 0; i < boardHeight * 2 + 1; i++)
            {
                // If on row with two roads per hex
                if (i % 2 == 0)
                {
                    roads[i] = new Road[vertices[i / 2].Length - 1];
                }
                // Row with parallel roads
                else
                {
                    roads[i] = new Road[tiles[(i - 1) / 2].Length + 1];
                }

                for (int j = 0; j < roads[i].Length; j++)
                {
                    roads[i][j] = new Road(i, j);
                }
            }

            return roads;
        }

        /// <summary>
        /// Chooses a random tile type based on input data of tile types and corresponding tile amounts.
        /// </summary>
        /// <param name="tAmounts"></param>
        /// <param name="tTypes"></param>
        /// <returns></returns>
        public Tile.TileType GetRandomTileType(int[] tAmounts, Tile.TileType[] tTypes)
        {
            int rand = UnityEngine.Random.Range(0, tTypes.Length);
            for (int i = rand; i < rand + tTypes.Length; i++)
            {
                if (tAmounts[i % tTypes.Length] == 0)
                {
                    continue;
                }
                else
                {
                    tAmounts[i % tTypes.Length]--;
                    return tTypes[i % tTypes.Length];
                }
            }

            // return desert if none left in data
            return Tile.TileType.Desert;
        }

        /// <summary>
        /// Chooses a random dice value based on input data of dice values and corresponding dice value amounts.
        /// </summary>
        /// <param name="dValues"></param>
        /// <returns></returns>
        public int GetRandomDiceValue(int[] dValues)
        {
            int rand = UnityEngine.Random.Range(0, dValues.Length);
            for (int i = rand; i < rand + dValues.Length; i++)
            {
                if (dValues[i % dValues.Length] == 0)
                {
                    continue;
                }
                else
                {
                    dValues[i % dValues.Length]--;
                    return i % dValues.Length;
                }
            }

            // return 0 if none left in data
            return 0;
        }

        /// <summary>
        /// Chooses a random tile type based on input data of tile types and corresponding tile amounts.
        /// </summary>
        /// <param name="tAmounts"></param>
        /// <param name="tTypes"></param>
        /// <returns></returns>
        public Resource.ResourceType GetRandomPortType(int[] rAmounts, Resource.ResourceType[] rTypes)
        {
            int rand = UnityEngine.Random.Range(0, rTypes.Length);
            for (int i = rand; i < rand + rTypes.Length; i++)
            {
                if (rAmounts[i % rTypes.Length] <= 0)
                {
                    continue;
                }
                else
                {
                    rAmounts[i % rTypes.Length]--;
                    return rTypes[i % rTypes.Length];
                }
            }

            // Throw if none left
            throw new Exception("None");
        }
    }
}
