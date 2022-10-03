using Catan.GameBoard;
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
            int x0 = tiles[i][j].xCoord;
            int y0 = (int)(tiles[i][j].yCoord + 1) * 2 + i % 2;

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

        public static bool Valid(this (int, int) point)
        {
            return point != (-1, -1);
        }
    }
}
