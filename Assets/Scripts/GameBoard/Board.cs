using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

namespace Catan.GameBoard
{
    /// <summary>
    ///     Class for holding, manipulating, and displaying game board information.
    /// </summary>
    /// <remarks>
    ///     The board is divided into three separate disjoint arrays containing tiles, vertices, and roads respectively.
    ///     Each array uses two forms of index: world coordinates, and data coordinates. World coordinates represent where each point is in the world. Data coordinates represent where each
    ///     point is stored in its array.
    /// </remarks>
    public class Board : MonoBehaviour
    {
        private readonly float H_POS_OFFSET = 2 * Mathf.Sqrt(3);
        private readonly float V_POS_OFFSET = 3;

        /// <summary>
        /// The active game tiles are stored here.
        /// </summary>
        public Tile[][] tiles;
        /// <summary>
        /// The active game vertices are stored here.
        /// </summary>
        public TileVertex[][] vertices;
        /// <summary>
        /// The active game roads are stored here.
        /// </summary>
        public Road[][] roads;

        public GameObject hexPrefab;
        public GameObject vertexPrefab;
        public GameObject roadPrefab;

        public GameObject tileHolder;
        public GameObject vertexHolder;
        public GameObject roadHolder;

        

        /// <summary>
        /// Clears the tile array and despawns all tiles in tile holder
        /// </summary>
        public void ClearTiles()
        {
            if (tiles == null)
            {
                return;
            }

            DespawnTiles();
            DespawnVertices();

            tiles = null;
        }

        /// <summary>
        /// Despawns all tiles in tile holder
        /// </summary>
        public void DespawnTiles()
        {
            GameObject tileHolder = GameObject.Find("Tile Holder");
            foreach (Transform child in tileHolder.transform)
            {
                GameObject.Destroy(child.gameObject);
            }
        }

        /// <summary>
        /// Despawns all vertices in tile holder
        /// </summary>
        public void DespawnVertices()
        {
            GameObject tileHolder = GameObject.Find("Vertex Holder");
            foreach (Transform child in tileHolder.transform)
            {
                GameObject.Destroy(child.gameObject);
            }
        }

        public void PrintTest()
        {
            for (int i = 0; i < tiles.Length; i++)
            { 
                for (int j = 0; j < tiles[i].Length; j++)
                {
                    (int, int)[] t = tiles.GetSurroundingVertices(vertices, i, j);
                }
            }
        }

        /// <summary>
        /// Places tiles specified by this class' tile array. Responsible for display of tiles.
        /// </summary>
        public void PlaceTiles()
        {
            if (tiles == null || tiles[0] == null)
            {
                return;
            }

            int max = 0;
            for (int i = 0; i < tiles.Length; i++)
            {
                if (tiles[i].Length > max)
                {
                    max = tiles[i].Length;
                }
            }
            max += 2;

            for (int i = 0; i < tiles.Length; i++)
            {
                for (int j = 0; j < tiles[i].Length; j++)
                {
                    float horizontalOffset = i % 2 * H_POS_OFFSET / 2 + ((int)((tiles[i].Length - 1) / 2)) * H_POS_OFFSET;
                    GameObject createdTile = Instantiate(hexPrefab, new Vector3(i * V_POS_OFFSET, 0, j * H_POS_OFFSET - horizontalOffset), Quaternion.identity);

                    tiles[i][j].xCoord = i;
                    tiles[i][j].yCoord = j + (max - tiles[i].Length) / 2 - 1;
                    tiles[i][j].xDataIndex = i;
                    tiles[i][j].yDataIndex = j;

                    createdTile.name = "Tile(" + i + "," + j + ")";
                    createdTile.transform.GetChild(0).GetComponent<Renderer>().material.color = tiles[i][j].color;
                    createdTile.transform.parent = tileHolder.transform;

                    TileGameObject tileObject = createdTile.transform.GetChild(0).GetComponent<TileGameObject>();
                    tileObject.SetDiceValue(tiles[i][j].diceValue);
                    tileObject.SetRobber(tiles[i][j].robber);
                    tileObject.xIndex = i;
                    tileObject.yIndex = j;
                }
            }
        }

        /// <summary>
        /// Places TileVertex objects on each hexagon corner. Responsible for display of vertices.
        /// </summary>
        public void PlaceVertices()
        {
            if (vertices == null || vertices[0] == null)
            {
                return;
            }

            int max = 0;
            for (int i = 0; i < vertices.Length; i++)
            {
                if (vertices[i].Length > max)
                {
                    max = vertices[i].Length;
                }
            }
            max += 2;

            for (int i = 0; i < vertices.Length; i++)
            {
                for (int j = 0; j < vertices[i].Length; j++)
                {
                    // Accounts for vertical offset due to hexagonal shape
                    float verticalOffset = (vertices[i][j].up ? 1 : 0) - 2;

                    // Get indices of tiles above and below current line
                    int i0 = Math.Clamp(i - 1, 0, tiles.Length - 1);
                    int i1 = Math.Clamp(i, 0, tiles.Length - 1);

                    // Get max number of tiles between above and below current line
                    int count = Math.Max(tiles[i0].Length, tiles[i1].Length);

                    // If row above and row below are equal and parity of i is 0, shift one more half-hexagon to the left
                    int sameSizeShift = 0;
                    if (i0 != i1 && tiles[i0].Length == tiles[i1].Length && i1 % 2 == 0/*|| i0 > 0 && i0 == i1*/)
                    {
                        sameSizeShift = 1;
                    }

                    // Calculate parity of line
                    int parity = 0;
                    if (tiles[i0].Length > tiles[i1].Length || i0 > 0 && i0 == i1)
                    {
                        parity += 1;
                    }
                    // Calculate horizontal offset
                    float horizontalOffset = (count - (count + 1) % 2 + (i + parity) % 2 + sameSizeShift) * H_POS_OFFSET / 2;
                    
                    vertices[i][j].xCoord = i;
                    vertices[i][j].yCoord = j + max - (int)(max / 2) - (int)Math.Round(horizontalOffset / H_POS_OFFSET * 2);
                    vertices[i][j].xDataIndex = i;
                    vertices[i][j].yDataIndex = j;

                    float x = i * V_POS_OFFSET + verticalOffset;
                    float z = j * H_POS_OFFSET / 2 - horizontalOffset;

                    // Creates tile GameObject at position
                    GameObject createdVertex = Instantiate(vertexPrefab, new Vector3(x, 0, z), Quaternion.identity);
                    createdVertex.name = "Vertex(" + i + "," + j + ")";
                    createdVertex.transform.parent = vertexHolder.transform;

                    TileVertexGameObject vertexObject = createdVertex.GetComponent<TileVertexGameObject>();
                    vertexObject.xIndex = i;
                    vertexObject.yIndex = j;
                }
            }
        }

        /// <summary>
        /// Places Road objects on each hexagon side. Responsible for display of roads.
        /// </summary>
        public void PlaceRoads()
        {
            if (vertices == null || vertices[0] == null || roads == null || roads[0] == null)
            {
                return;
            }

            for (int i = 0; i < roads.Length; i++)
            {
                if (i % 2 == 0)
                {
                    for (int j = 0; j < roads[i].Length; j++)
                    {
                        Vector3 first = GameObject.Find("Vertex(" + i / 2 + "," + j + ")").transform.position;
                        Vector3 second = GameObject.Find("Vertex(" + i / 2 + "," + (j + 1) + ")").transform.position;

                        float x = (first.x + second.x) / 2;
                        float z = (first.z + second.z) / 2;

                        int angle = vertices[i / 2][j].up ? -1 : 1;

                        roads[i][j].xCoord = vertices[i/2][j].xCoord * 2;
                        roads[i][j].yCoord = vertices[i/2][j].yCoord;

                        GameObject createdRoad = Instantiate(roadPrefab, new Vector3(x, 0, z), Quaternion.Euler(0, 30 * angle, 0));
                        createdRoad.name = "Road(" + i + "," + j + ")";
                        createdRoad.transform.parent = roadHolder.transform;

                        RoadGameObject roadObject = createdRoad.GetComponent<RoadGameObject>();
                        roadObject.xIndex = i;
                        roadObject.yIndex = j;
                    }
                }
                else
                {
                    int parity = vertices[i / 2][0].up ? 1 : 0;
                    for (int j = 0; j < roads[i].Length * 2 - parity; j++)
                    {
                        (int, int) current = ((int)(i / 2), j);
                        (int, int) below = vertices.VertexBelowVertex(current.Item1, current.Item2);
                        if (below == (-1, -1))
                        {
                            continue;
                        }

                        Vector3 first = GameObject.Find("Vertex(" + current.Item1 + "," + current.Item2 + ")").transform.position;
                        Vector3 second = GameObject.Find("Vertex(" + below.Item1 + "," + below.Item2 + ")").transform.position;

                        float x = (first.x + second.x) / 2;
                        float z = (first.z + second.z) / 2;

                        roads[i][(int)(j / 2)].xCoord = i;
                        roads[i][(int)(j / 2)].yCoord = vertices[current.Item1][current.Item2].yCoord;

                        GameObject createdRoad = Instantiate(roadPrefab, new Vector3(x, 0, z), Quaternion.Euler(0, 90, 0));
                        createdRoad.name = "Road(" + i + "," + j + ")";
                        createdRoad.transform.parent = roadHolder.transform;

                        RoadGameObject roadObject = createdRoad.GetComponent<RoadGameObject>();
                        roadObject.xIndex = i;
                        roadObject.yIndex = (int)(j / 2);
                    }
                }
            }
        }

        public void PlacePorts()
        {
            for (int i = 0; i < vertices.Length; i++)
            {
                for (int j = 0; j < vertices[i].Length; j++)
                {
                    /*Debug.Log
                    (
                        vertices[i][j].xCoord + " " + 
                        vertices[i][j].yCoord + " " +
                        " Left: (" + vertices.RoadLeftOfVertex(roads, i, j) + ")," +
                        " Right: (" + vertices.RoadRightOfVertex(roads, i, j) + "), " +
                        " Up: (" + vertices.RoadAboveVertex(roads, i, j) + "), " +
                        " Down: (" + vertices.RoadBelowVertex(roads, i, j) + "), "
                    );*/
                }
            }
            for (int i = 0; i < vertices.Length; i++)
            {
                for (int j = 0; j < vertices[i].Length; j++)
                {
                    if (vertices[i][j].port != null)
                    {
                        GameObject child = GameObject.Find("Vertex(" + i + "," + j + ")").transform.GetChild(0).gameObject;
                        child.GetComponent<MeshRenderer>().enabled = true;
                        Vector3 pos = child.transform.parent.position + BoardExtensions.PolarToCartesian(vertices[i][j].port.direction, 20);
                        Vector3 ang = new Vector3(0, vertices[i][j].port.direction + 90, 0);
                        child.transform.SetPositionAndRotation(pos, Quaternion.Euler(ang));
                    }
                }
            }
        }
    }
}
