using System;
using UnityEngine;

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

        public Board board;

        /// <summary>
        /// Is called when the scene loads
        /// </summary>
        void Start()
        {
            board = GameObject.Find("Board").GetComponent(typeof(Board)) as Board;
            board.ClearTiles();
            board.tiles = Randomize();
            board.vertices = InitializeVertices(board.tiles);
            board.roads = InitializeRoads(board.vertices, board.tiles);
            board.PlaceTiles();
            board.PlaceVertices();
            board.PlaceRoads();
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

            for (int i = 0; i < boardHeight; i++)
            {
                tileArray[i] = new Tile[boardShape[i]];
                for (int j = 0; j < tileArray[i].Length; j++)
                {
                    tileArray[i][j] = new Tile(i, j, GetRandomTileType(tAmounts, tileTypes));
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
    }
}
