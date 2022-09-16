using Catan.GameBoard;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

namespace Catan.GameBoard
{
    public static class BoardExtensions
    {
        /// <summary>
        /// Gets the vertex above the specified vertex.
        /// </summary>
        /// <param name="i"></param>
        /// <param name="j"></param>
        /// <returns></returns>
        public static (int, int) VertexAboveVertex(this TileVertex[][] vertices, int i, int j)
        {
            if (!vertices[i][j].up)
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
        /// Gets the vertex below the specified vertex.
        /// </summary>
        /// <param name="i"></param>
        /// <param name="j"></param>
        /// <returns></returns>
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
                TileVertex test = vertices[i+1][v];
                if (test != null)
                {
                    return (i + 1, v);
                }
            }
            catch { }

            return (-1, -1);
        }

        /// <summary>
        /// Gets the vertex to the left of the specified vertex.
        /// </summary>
        /// <param name="i"></param>
        /// <param name="j"></param>
        /// <returns></returns>
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
        /// Gets the vertex to the right of the specified vertex.
        /// </summary>
        /// <param name="i"></param>
        /// <param name="j"></param>
        /// <returns></returns>
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
        /// Converts a horizontal value x in row i0 to it's corresponding horizontal value in row i1
        /// </summary>
        /// <param name="vertices"></param>
        /// <param name="x"></param>
        /// <param name="i0"></param>
        /// <param name="i1"></param>
        /// <returns></returns>
        public static int ConvertVertical(this TileVertex[][] vertices, int x, int i0, int i1)
        {
            int ycoord = vertices[i0][x].yCoord;

            if (vertices[i1].Count() < 1)
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

        
        public static Tile TileAboveVertex(this TileVertex[][] vertices)
        {
            return null;
        }

        public static Tile TileBelowVertex(this TileVertex[][] vertices)
        {
            return null;
        }

        public static Tile TileRightOfVertex(this TileVertex[][] vertices)
        {
            return null;
        }

        public static Tile TileLeftOfVertex(this TileVertex[][] vertices)
        {
            return null;
        }
    }
}
