using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Catan.GameBoard
{
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
        public GameObject hexPrefab;
        public GameObject vertexPrefab;

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

        /// <summary>
        /// Places tiles specified by this class' tile array. Responsible for display of tiles.
        /// </summary>
        public void PlaceTiles()
        {
            if (tiles == null || tiles[0] == null)
            {
                return;
            }

            for (int i = 0; i < tiles.Length; i++)
            {
                for (int j = 0; j < tiles[i].Length; j++)
                {
                    float horizontalOffset = i % 2 * H_POS_OFFSET / 2 + ((int)((tiles[i].Length - 1) / 2)) * H_POS_OFFSET;
                    GameObject createdTile = Instantiate(hexPrefab, new Vector3(i * V_POS_OFFSET, 0, j * H_POS_OFFSET - horizontalOffset), Quaternion.identity);
                    createdTile.name = "Tile(" + i + "," + j + ")";
                    createdTile.transform.GetChild(0).GetComponent<Renderer>().material.color = tiles[i][j].color;
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

                    // Creates tile GameObject at position
                    GameObject createdTile = Instantiate(vertexPrefab, new Vector3(i * V_POS_OFFSET + verticalOffset, 0, j * H_POS_OFFSET / 2 - horizontalOffset), Quaternion.identity);

                    createdTile.name = "Vertex(" + i + "," + j + ")";
                    //createdTile.transform.GetChild(0).GetComponent<Renderer>().material.color = Color.blue;
                }
            }
        }
    }
}
