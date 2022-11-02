/// AUTHOR: Matthew Moffitt
/// FILENAME: BoardExtensions.cs
/// SPECIFICATION: File containing board info
/// FOR: CS 3368 Introduction to Artificial Intelligence Section 001

using Catan.GameBoard;
using Catan.Players;
using Catan.ResourcePhase;
using Catan.Util;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Unity.VisualScripting;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

namespace Catan.GameBoard
{
    /// <summary>
    ///     Extension class for board navigation. 
    /// </summary>
    public static class BoardExtensions
    {
        /// <summary>
        ///     Gets the vertex above the specified vertex.
        /// </summary>
        /// <param name="i"></param>
        /// <param name="j"></param>
        /// <returns>
        ///     Two integers representing the location of a vertex in Data-Index form
        /// </returns>
        /// <remarks></remarks>
        public static (int, int) VertexAboveVertex(this TileVertex[][] vertices, int i, int j)
        {
            if (vertices[i][j].up)
            {
                return (-1, -1);
            }

            int v = vertices.ConvertVertical(j, i, i - 1);

            if (v == -1)
            {
                return (-1, -1);
            }

            try
            {
                TileVertex test = vertices[i - 1][v];
                if (test != null)
                {
                    return (i - 1, v);
                }
            }
            catch { }

            return (-1, -1);
        }

        /// <summary>
        ///     Gets the vertex below the specified vertex.
        /// </summary>
        /// <param name="i"></param>
        /// <param name="j"></param>
        /// <returns>
        ///     Two integers representing the location of a vertex in Data-Index form
        /// </returns>
        /// <remarks></remarks>
        public static (int, int) VertexBelowVertex(this TileVertex[][] vertices, int i, int j)
        {
            if (!vertices[i][j].up)
            {
                return (-1, -1);
            }
            
            int v = vertices.ConvertVertical(j, i, i + 1);

            if (v == -1)
            {
                return (-1, -1);
            }

            try
            {
                TileVertex test = vertices[i + 1][v];
                if (test != null)
                {
                    return (i + 1, v);
                }
            }
            catch { }

            return (-1, -1);
        }

        /// <summary>
        ///     Gets the vertex to the left of the specified vertex.
        /// </summary>
        /// <param name="i"></param>
        /// <param name="j"></param>
        /// <returns>
        ///     Two integers representing the location of a vertex in Data-Index form
        /// </returns>
        /// <remarks></remarks>
        public static (int, int) VertexLeftOfVertex(this TileVertex[][] vertices, int i, int j)
        {
            try
            {
                TileVertex test = vertices[i][j - 1];
                if (test != null)
                {
                    return (i, j - 1);
                }
            }
            catch { }

            return (-1, -1);
        }

        /// <summary>
        ///     Gets the vertex to the right of the specified vertex.
        /// </summary>
        /// <param name="i"></param>
        /// <param name="j"></param>
        /// <returns>
        ///     Two integers representing the location of a vertex in Data-Index form
        /// </returns>
        /// <remarks></remarks>
        public static (int, int) VertexRightOfVertex(this TileVertex[][] vertices, int i, int j)
        {
            try
            {
                TileVertex test = vertices[i][j + 1];
                if (test != null)
                {
                    return (i, j + 1);
                }
            }
            catch { }

            return (-1, -1);
        }

        /// <summary>
        ///     Converts a horizontal value x in row i0 to it's corresponding horizontal value in row i1
        /// </summary>
        /// <param name="vertices"></param>
        /// <param name="x"></param>
        /// <param name="i0"></param>
        /// <param name="i1"></param>
        /// <returns>
        ///     An integer representing the y-location of a vertex in Data-Index form
        /// </returns>
        /// <remarks></remarks>
        public static int ConvertVertical(this TileVertex[][] vertices, int x, int i0, int i1)
        {
            int ycoord = vertices[i0][x].yCoord;

            try
            {
                if (vertices[i1].Count() < 1)
                {
                    return -1;
                }
            }
            catch
            {
                return -1;
            }

            TileVertex shifted = vertices[i1].Where(v => v.yCoord == ycoord).FirstOrDefault();
            if (shifted == null)
            {
                return -1;
            }

            return shifted.yDataIndex;
        }
        
        /// <summary>
        ///     Takes the location of a vertex in DATA FORM and converts it to the location of the tile above the vertex
        /// </summary>
        /// <param name="vertices"></param>
        /// <param name="tiles"></param>
        /// <param name="i"></param>
        /// <param name="j"></param>
        /// <param name="max"></param>
        /// <returns>
        ///     Integer tuple location of found tile in DATA FORM, else (-1, -1)
        /// </returns>
        public static (int, int) TileAboveVertex(this TileVertex[][] vertices, Tile[][] tiles, int i, int j)
        {
            int max = 0;
            for (int k = 0; k < tiles.Length; k++)
            {
                if (tiles[k].Length > max)
                {
                    max = tiles[k].Length;
                }
            }
            max += 2;

            return vertices.TileAboveVertex(tiles, i, j, max);
        }

        /// <summary>
        ///     Takes the location of a vertex in DATA FORM and converts it to the location of the tile below the vertex
        /// </summary>
        /// <param name="vertices"></param>
        /// <param name="tiles"></param>
        /// <param name="i"></param>
        /// <param name="j"></param>
        /// <param name="j"></param>
        /// <returns>
        ///     Integer tuple location of found tile in DATA FORM, else (-1, -1)
        /// </returns>
        public static (int, int) TileBelowVertex(this TileVertex[][] vertices, Tile[][] tiles, int i, int j)
        {
            int max = 0;
            for (int k = 0; k < tiles.Length; k++)
            {
                if (tiles[k].Length > max)
                {
                    max = tiles[k].Length;
                }
            }
            max += 2;

            return vertices.TileBelowVertex(tiles, i, j, max);
        }
        /// <summary>
        ///     Takes the location of a vertex in DATA FORM and converts it to the location of the tile above the vertex
        /// </summary>
        /// <param name="vertices"></param>
        /// <param name="tiles"></param>
        /// <param name="i"></param>
        /// <param name="j"></param>
        /// <param name="max"></param>
        /// <returns>
        ///     Integer tuple location of found tile in DATA FORM, else (-1, -1)
        /// </returns>
        public static (int, int) TileAboveVertex(this TileVertex[][] vertices, Tile[][] tiles, int i, int j, int max)
        {
            if (!vertices[i][j].up) return (-1, -1);

            int xc = vertices[i][j].xCoord;
            int yc = vertices[i][j].yCoord;

            // Subtracts 1 if row and max row length have certain parity
            int smallOffset = (max % 2 == 1 ? (i + 1) % 2 : (i) % 2);

            int x = xc - 1;
            int y = (int)(yc / 2) - 1 - smallOffset;

            (int xout, int yout) = tiles.GetTileDataCoord(x, y);

            return (xout, yout);
        }

        /// <summary>
        ///     Takes the location of a vertex in DATA FORM and converts it to the location of the tile below the vertex
        /// </summary>
        /// <param name="vertices"></param>
        /// <param name="tiles"></param>
        /// <param name="i"></param>
        /// <param name="j"></param>
        /// <param name="j"></param>
        /// <returns>
        ///     Integer tuple location of found tile in DATA FORM, else (-1, -1)
        /// </returns>
        public static (int, int) TileBelowVertex(this TileVertex[][] vertices, Tile[][] tiles, int i, int j, int max)
        {
            if (vertices[i][j].up) return (-1, -1);

            int xc = vertices[i][j].xCoord;
            int yc = vertices[i][j].yCoord;

            // Subtracts 1 if row and max row length have certain parity
            int smallOffset = (max % 2 == 0 ? (i + 1) % 2 : (i) % 2);

            int x = xc;
            int y = (int)(yc / 2) - 1 - smallOffset;

            (int xout, int yout) = tiles.GetTileDataCoord(x, y);
            return (xout, yout);
        }

        /// <summary>
        ///     Takes the location of a vertex in DATA FORM and converts it to the location of the tile right of the vertex
        /// </summary>
        /// <param name="vertices"></param>
        /// <param name="tiles"></param>
        /// <param name="i"></param>
        /// <param name="j"></param>
        /// <returns>
        ///     Integer tuple location of found tile in DATA FORM, else (-1, -1)
        /// </returns>
        public static (int, int) TileRightOfVertex(this TileVertex[][] vertices, Tile[][] tiles, int i, int j)
        {
            int xc = vertices[i][j].xCoord;
            int yc = vertices[i][j].yCoord;

            int x = xc - (vertices[i][j].up ? 0 : 1);
            int y = (int)(yc / 2) - 1;

            (int xout, int yout) = tiles.GetTileDataCoord(x, y);
            return (xout, yout);
        }

        /// <summary>
        ///     Takes the location of a vertex in DATA FORM and converts it to the location of the tile left of the vertex
        /// </summary>
        /// <param name="vertices"></param>
        /// <param name="tiles"></param>
        /// <param name="i"></param>
        /// <param name="j"></param>
        /// <returns>
        ///     Integer tuple location of found tile in DATA FORM, else (-1, -1)
        /// </returns>
        public static (int, int) TileLeftOfVertex(this TileVertex[][] vertices, Tile[][] tiles, int i, int j)
        {
            int xc = vertices[i][j].xCoord;
            int yc = vertices[i][j].yCoord;

            int x = xc - (vertices[i][j].up ? 0 : 1);
            int y = (int)(yc / 2) - 2;

            (int xout, int yout) = tiles.GetTileDataCoord(x, y);
            return (xout, yout);
        }

        /// <summary>
        ///     Takes the location of a vertex in DATA FORM and converts it to the location of the tile above the vertex
        /// </summary>
        /// <param name="vertices"></param>
        /// <param name="roads"></param>
        /// <param name="i"></param>
        /// <param name="j"></param>
        /// <returns>
        ///     Integer tuple location of found road in DATA FORM, else (-1, -1)
        /// </returns>
        public static (int, int) RoadAboveVertex(this TileVertex[][] vertices, Road[][] roads, int i, int j)
        {
            if (vertices[i][j].up) return (-1, -1);

            int xc = vertices[i][j].xCoord;
            int yc = vertices[i][j].yCoord;

            int x = xc * 2 - 1;
            int y = yc;

            (int xout, int yout) = roads.GetRoadDataCoord(x, y);
            return (xout, yout);
        }

        /// <summary>
        ///     Takes the location of a vertex in DATA FORM and converts it to the location of the tile below the vertex
        /// </summary>
        /// <param name="vertices"></param>
        /// <param name="roads"></param>
        /// <param name="i"></param>
        /// <param name="j"></param>
        /// <returns>
        ///     Integer tuple location of found road in DATA FORM, else (-1, -1)
        /// </returns>
        public static (int, int) RoadBelowVertex(this TileVertex[][] vertices, Road[][] roads, int i, int j)
        {
            if (!vertices[i][j].up) return (-1, -1);

            int xc = vertices[i][j].xCoord;
            int yc = vertices[i][j].yCoord;

            int x = xc * 2 + 1;
            int y = yc;

            (int xout, int yout) = roads.GetRoadDataCoord(x, y);
            return (xout, yout);
        }

        /// <summary>
        ///     Takes the location of a vertex in DATA FORM and converts it to the location of the tile to the right of the vertex
        /// </summary>
        /// <param name="vertices"></param>
        /// <param name="roads"></param>
        /// <param name="i"></param>
        /// <param name="j"></param>
        /// <returns>
        ///     Integer tuple location of found road in DATA FORM, else (-1, -1)
        /// </returns>
        public static (int, int) RoadRightOfVertex(this TileVertex[][] vertices, Road[][] roads, int i, int j)
        {
            if (j >= roads[i * 2].Length)
            {
                return (-1, -1);
            }

            if (roads[i * 2][j] == null)
            {
                return (-1, -1);
            }

            return (i * 2, j);
        }

        /// <summary>
        ///     Takes the location of a vertex in DATA FORM and converts it to the location of the tile to the left of the vertex
        /// </summary>
        /// <param name="vertices"></param>
        /// <param name="roads"></param>
        /// <param name="i"></param>
        /// <param name="j"></param>
        /// <returns>
        ///     Integer tuple location of found road in DATA FORM, else (-1, -1)
        /// </returns>
        public static (int, int) RoadLeftOfVertex(this TileVertex[][] vertices, Road[][] roads, int i, int j)
        {
            if (j <= 0)
            {
                return (-1, -1);
            }

            if (roads[i * 2][j - 1] == null)
            {
                return (-1, -1);
            }

            return (i * 2, j - 1);
        }

        /// <summary>
        /// Takes the location of a vertex in DATA FORM and grabs all adjacent roads
        /// </summary>
        /// <param name="vertices"></param>
        /// <param name="roads"></param>
        /// <param name="vertex"></param>
        /// <returns>
        /// A list of roads adjacent to the specified vertex
        /// </returns>
        public static (int, int)[] AdjacentRoadsToVertex(this TileVertex[][] vertices, Road[][] roads, (int, int) vertex)
        {
            (int, int) above = vertices.RoadAboveVertex(roads, vertex.Item1, vertex.Item2);
            (int, int) below = vertices.RoadBelowVertex(roads, vertex.Item1, vertex.Item2);
            (int, int) left = vertices.RoadLeftOfVertex(roads, vertex.Item1, vertex.Item2);
            (int, int) right = vertices.RoadRightOfVertex(roads, vertex.Item1, vertex.Item2);

            List<(int, int)> output = new List<(int, int)>();
            if (above != (-1, -1)) output.Add(above);
            if (below != (-1, -1)) output.Add(below);
            if (left != (-1, -1)) output.Add(left);
            if (right != (-1, -1)) output.Add(right);

            return output.ToArray();
        }

        /// <summary>
        /// Gets both vertices adjacent to a particular vertex.
        /// </summary>
        /// <param name="roads"></param>
        /// <param name="vertices"></param>
        /// <param name="road"></param>
        /// <returns>
        /// An array of tuples specifying the locations of the two vertices. Will always return an array of size 2, as all roads have two bounding vertices.
        /// </returns>
        public static (int, int)[] AdjacentVerticesToRoad(this Road[][] roads, TileVertex[][] vertices, (int, int) road)
        {
            int i = road.Item1;
            int j = road.Item2;
            (int, int)[] arr = new (int, int)[2];

            if (j >= vertices[i / 2].Length)
            {
                arr[0] = (-1, -1);
                arr[1] = (-1, -1);
            }

            if (vertices[i / 2][j] == null)
            {
                arr[0] = (-1, -1);
                arr[1] = (-1, -1);
            }

            // Case when road is running left-right
            if (i % 2 == 0)
            {
                arr[0] = (i / 2, j);
                arr[1] = (i / 2, j + 1);
            }
            // Case when road is running up-down
            else
            {
                arr[0] = (-1, -1);
                arr[1] = (-1, -1);
                for (int k = 0; k < vertices[i / 2].Length; k++)
                {
                    if (vertices.RoadBelowVertex(roads, i/2, k) == road)
                    {
                        arr[0] = (i / 2, k);
                    }
                }

                for (int k = 0; k < vertices[i / 2 + 1].Length; k++)
                {
                    if (vertices.RoadAboveVertex(roads, i / 2 + 1, k) == road)
                    {
                        arr[1] = (i / 2 + 1, k);
                    }
                }
            }

            return arr;
        }

        /// <summary>
        ///     Converts a tile coordinate in WORLD FORM to DATA FORM
        /// </summary>
        /// <param name="tiles"></param>
        /// <param name="i"></param>
        /// <param name="j"></param>
        /// <returns>
        ///     Integer tuple in DATA FORM. If point not found in data, (-1, -1)
        /// </returns>
        public static (int, int) GetTileDataCoord(this Tile[][] tiles, int i, int j)
        {
            int y;
            try
            {
                y = tiles[i].Where(k => k.yCoord == j).First().yDataIndex;
            }
            catch
            {
                return (-1, -1);
            }
            return (i, y);
        }

        /// <summary>
        ///     Converts a vertex coordinate in WORLD FORM to DATA FORM
        /// </summary>
        /// <param name="tiles"></param>
        /// <param name="i"></param>
        /// <param name="j"></param>
        /// <returns>
        ///     Integer tuple in DATA FORM. If point not found in data, (-1, -1)
        /// </returns>
        public static (int, int) GetVertexDataCoord(this TileVertex[][] vertices, int i, int j)
        {
            int y;
            try
            {
                y = vertices[i].Where(k => k.yCoord == j).First().yDataIndex;
            }
            catch
            {
                return (-1, -1);
            }
            return (i, y);
        }

        /// <summary>
        ///     Converts a road coordinate in WORLD FORM to DATA FORM
        /// </summary>
        /// <param name="tiles"></param>
        /// <param name="i"></param>
        /// <param name="j"></param>
        /// <returns>
        ///     Integer tuple in DATA FORM. If point not found in data, (-1, -1)
        /// </returns>
        public static (int, int) GetRoadDataCoord(this Road[][] roads, int i, int j)
        {
            int y;
            try
            {
                y = roads[i].Where(k => k.yCoord == j).First().yDataIndex;
            }
            catch
            {
                return (-1, -1);
            }
            return (i, y);
        }

        /// <summary>
        ///     Gets a tile coordinate in DATA FORM and calculates the surrounding 6 vertices in order: Top left, top, top right, bottom left, bottom, bottom right
        /// </summary>
        /// <param name="tiles"></param>
        /// <param name="i"></param>
        /// <param name="j"></param>
        /// <returns>An array of 6 vertices in DATA FORM</returns>
        public static (int, int)[] GetSurroundingVertices(this Tile[][] tiles, TileVertex[][] vertices, int i, int j)
        {
            int maxIsEven = (tiles.SelectMax(tArr => tArr.Length).Length + 1) % 2;

            int x0 = tiles[i][j].xCoord;
            int y0 = (int)(tiles[i][j].yCoord + 1) * 2 + i % 2 + maxIsEven * (int)Mathf.Pow(-1, i);

            (int, int) top = vertices.GetVertexDataCoord(x0, y0);
            (int, int) bottom = vertices.GetVertexDataCoord(x0 + 1, y0);
            (int, int)[] output = 
            {
                (top.Item1, top.Item2),
                (top.Item1, top.Item2 + 1),
                (top.Item1, top.Item2 + 2),
                (bottom.Item1, bottom.Item2),
                (bottom.Item1, bottom.Item2 + 1),
                (bottom.Item1, bottom.Item2 + 2)
            };

            return output;
        }

        /// <summary>
        /// Converts polar coordinates to cartesian
        /// </summary>
        /// <param name="theta"></param>
        /// <param name="r"></param>
        /// <returns>
        /// Returns a Vector3 object with a y value of 0
        /// </returns>
        public static Vector3 PolarToCartesian(float theta, float r)
        {
            r = Mathf.Deg2Rad * r;
            return new Vector3(r * Mathf.Cos(theta), 0, r * Mathf.Sin(theta));
        }

        /// <summary>
        /// Returns true if a tuple does not equal (-1, -1). Compliments other functions that search for tiles, vertices, etc.
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public static bool Valid(this (int, int) point)
        {
            return point != (-1, -1);
        }

        /// <summary>
        /// Returns a list of ports the specified player controls on the board
        /// </summary>
        /// <param name="tiles"></param>
        /// <param name="player"></param>
        /// <returns>
        /// A list of ports the specified player controls on the board
        /// </returns>
        public static Resource.ResourceType[] GetPlayerPorts(this TileVertex[][] tiles, Player player)
        {
            List<Resource.ResourceType> ports = new List<Resource.ResourceType>();
            for (int i = 0; i < tiles.Length; i++)
            {
                for (int j = 0; j < tiles[i].Length; j++)
                {
                    if (tiles[i][j].playerIndex == player.playerIndex && tiles[i][j].port != null)
                    {
                        ports.Add(tiles[i][j].port.type);
                    }
                }
            }
            return ports.ToArray();
        }

        /// <summary>
        /// Returns whether a specified port resource type exists under a specified player's control
        /// </summary>
        /// <param name="tiles"></param>
        /// <param name="player"></param>
        /// <param name="resource"></param>
        /// <returns></returns>
        public static bool HasPort(this TileVertex[][] tiles, Player player, Resource.ResourceType resource)
        {
            Resource.ResourceType[] ports = GetPlayerPorts(tiles, player);

            foreach (Resource.ResourceType port in ports)
            {
                if (port == resource)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Gets every possible settlement a player could build at current board state.
        /// </summary>
        /// <param name="board"></param>
        /// <param name="player"></param>
        /// <returns>(int, int) array of vertex locations representing settlements the specified player can develop</returns>
        public static (int, int)[] GetPossibleSettlements(this Board board, Player player)
        {
            List<(int, int)> possible = new List<(int, int)>();
            foreach (TileVertex[] vArr in board.vertices)
            {
                foreach (TileVertex v in vArr)
                {
                    // Eliminate developed vertex

                    if (v.development > 0) continue;

                    // Eliminate vertex where adjacent vertex is developed

                    (int, int) vAbove = board.vertices.VertexAboveVertex(v.xDataIndex, v.yDataIndex);
                    if (vAbove.Valid() && board.vertices[vAbove.Item1][vAbove.Item2].development > 0) continue;

                    (int, int) vBelow = board.vertices.VertexBelowVertex(v.xDataIndex, v.yDataIndex);
                    if (vBelow.Valid() && board.vertices[vBelow.Item1][vBelow.Item2].development > 0) continue;

                    (int, int) vLeft = board.vertices.VertexLeftOfVertex(v.xDataIndex, v.yDataIndex);
                    if (vLeft.Valid() && board.vertices[vLeft.Item1][vLeft.Item2].development > 0) continue;

                    (int, int) vRight = board.vertices.VertexRightOfVertex(v.xDataIndex, v.yDataIndex);
                    if (vRight.Valid() && board.vertices[vRight.Item1][vRight.Item2].development > 0) continue;

                    // Eliminate vertex where a player-owned road is not adjacent

                    (int, int) rAbove = board.vertices.RoadAboveVertex(board.roads, v.xDataIndex, v.yDataIndex);
                    bool rAboveExists = rAbove.Valid() && board.roads[rAbove.Item1][rAbove.Item2].playerIndex == player.playerIndex;

                    (int, int) rBelow = board.vertices.RoadBelowVertex(board.roads, v.xDataIndex, v.yDataIndex);
                    bool rBelowExists = rBelow.Valid() && board.roads[rBelow.Item1][rBelow.Item2].playerIndex == player.playerIndex;

                    (int, int) rLeft = board.vertices.RoadLeftOfVertex(board.roads, v.xDataIndex, v.yDataIndex);
                    bool rLeftExists = rLeft.Valid() && board.roads[rLeft.Item1][rLeft.Item2].playerIndex == player.playerIndex;

                    (int, int) rRight = board.vertices.RoadRightOfVertex(board.roads, v.xDataIndex, v.yDataIndex);
                    bool rRightExists = rRight.Valid() && board.roads[rRight.Item1][rRight.Item2].playerIndex == player.playerIndex;

                    if (!(rAboveExists || rBelowExists || rLeftExists || rRightExists)) continue;

                    possible.Add((v.xDataIndex, v.yDataIndex));
                }
            }

            return possible.ToArray();
        }

        /// <summary>
        /// Gets every possible city a player could upgrade at current board state.
        /// </summary>
        /// <param name="board"></param>
        /// <param name="player"></param>
        /// <returns>(int, int) array of vertex locations representing cities the specified player can develop</returns>
        public static (int, int)[] GetPossibleCities(this Board board, Player player)
        {
            List<(int, int)> possible = new List<(int, int)>();
            foreach (TileVertex[] vArr in board.vertices)
            {
                foreach (TileVertex v in vArr)
                {
                    if (v.playerIndex == player.playerIndex && v.development == TileVertex.Development.Town)
                    {
                        possible.Add((v.xDataIndex, v.yDataIndex));
                    }
                }
            }

            return possible.ToArray();
        }

        /// <summary>
        /// Gets every possible road a player could build at current board state.
        /// </summary>
        /// <param name="board"></param>
        /// <param name="player"></param>
        /// <returns>(int, int) array of road locations representing roads the specified player can develop</returns>
        public static (int, int)[] GetPossibleRoads(this Board board, Player player)
        {
            List<(int, int)> possible = new List<(int, int)>();
            foreach (Road[] rArr in board.roads)
            {
                foreach (Road r in rArr)
                {
                    // If road already owned, do not add.
                    if (r.playerIndex != -1) continue;

                    // This while loop doesn't serve as a loop, but rather a construct to easily break out of to save calculation time.
                    bool adding = true;
                    while (true)
                    {
                        (int, int)[] adjacentVertices = board.roads.AdjacentVerticesToRoad(board.vertices, (r.xDataIndex, r.yDataIndex));

                        // If neighboring vertex owned, add.
                        int dir1VertexOwner = board.vertices[adjacentVertices[0].Item1][adjacentVertices[0].Item2].playerIndex;
                        if (dir1VertexOwner == player.playerIndex) break;

                        int dir2VertexOwner = board.vertices[adjacentVertices[1].Item1][adjacentVertices[1].Item2].playerIndex;
                        if (dir2VertexOwner == player.playerIndex) break;

                        // If unobstructed neighboring road owned, add.

                        if (dir1VertexOwner == -1)
                        {
                            (int, int) above1 = board.vertices.RoadAboveVertex(board.roads, adjacentVertices[0].Item1, adjacentVertices[0].Item2);
                            if (above1.Valid() && board.roads[above1.Item1][above1.Item2].playerIndex == player.playerIndex) break;

                            (int, int) below1 = board.vertices.RoadBelowVertex(board.roads, adjacentVertices[0].Item1, adjacentVertices[0].Item2);
                            if (below1.Valid() && board.roads[below1.Item1][below1.Item2].playerIndex == player.playerIndex) break;

                            (int, int) left1 = board.vertices.RoadLeftOfVertex(board.roads, adjacentVertices[0].Item1, adjacentVertices[0].Item2);
                            if (left1.Valid() && board.roads[left1.Item1][left1.Item2].playerIndex == player.playerIndex) break;

                            (int, int) right1 = board.vertices.RoadRightOfVertex(board.roads, adjacentVertices[0].Item1, adjacentVertices[0].Item2);
                            if (right1.Valid() && board.roads[right1.Item1][right1.Item2].playerIndex == player.playerIndex) break;
                        }

                        if (dir2VertexOwner == -1)
                        {
                            (int, int) above2 = board.vertices.RoadAboveVertex(board.roads, adjacentVertices[1].Item1, adjacentVertices[1].Item2);
                            if (above2.Valid() && board.roads[above2.Item1][above2.Item2].playerIndex == player.playerIndex) break;

                            (int, int) below2 = board.vertices.RoadBelowVertex(board.roads, adjacentVertices[1].Item1, adjacentVertices[1].Item2);
                            if (below2.Valid() && board.roads[below2.Item1][below2.Item2].playerIndex == player.playerIndex) break;

                            (int, int) left2 = board.vertices.RoadLeftOfVertex(board.roads, adjacentVertices[1].Item1, adjacentVertices[1].Item2);
                            if (left2.Valid() && board.roads[left2.Item1][left2.Item2].playerIndex == player.playerIndex) break;

                            (int, int) right2 = board.vertices.RoadRightOfVertex(board.roads, adjacentVertices[1].Item1, adjacentVertices[1].Item2);
                            if (right2.Valid() && board.roads[right2.Item1][right2.Item2].playerIndex == player.playerIndex) break;
                        }

                        adding = false;
                        break;
                    }

                    if (adding)
                    {
                        possible.Add((r.xDataIndex, r.yDataIndex));
                    }
                }
            }

            return possible.ToArray();
        }
    }
}
