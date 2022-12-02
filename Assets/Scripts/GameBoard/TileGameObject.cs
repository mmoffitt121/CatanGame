/// AUTHOR: Matthew Moffitt
/// FILENAME: TileGameObject.cs
/// SPECIFICATION: File containing board info
/// FOR: CS 3368 Introduction to Artificial Intelligence Section 001

using Catan.GameManagement;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Catan.GameBoard
{
    /// <summary>
    /// GameObject representing a tile in the game
    /// </summary>
    public class TileGameObject : MonoBehaviour
    {
        /// <summary>
        /// The text on the tile
        /// </summary>
        public TextMeshProUGUI text;
        /// <summary>
        /// The mesh of the tile
        /// </summary>
        public GameObject mesh;
        /// <summary>
        /// The token renderer on top of the tile
        /// </summary>
        public MeshRenderer tokenRenderer;
        /// <summary>
        /// The mesh of the robber on this tile
        /// </summary>
        public MeshRenderer robberRenderer;

        /// <summary>
        /// The dice value of the tile
        /// </summary>
        public int diceValue;
        /// <summary>
        /// Whether or not a robber is on this space
        /// </summary>
        public bool robber;

        /// <summary>
        /// x coordinate in DATA FORM
        /// </summary>
        public int xIndex;
        /// <summary>
        /// y coordinate in DATA FORM
        /// </summary>
        public int yIndex;
        /// <summary>
        /// x coordinate in POSITIONAL FORM
        /// </summary>
        public int xCoord;
        /// <summary>
        /// y coordinate in POSITIONAL FORM
        /// </summary>
        public int yCoord;

        public void OnMouseDown()
        {
            GameObject.Find("Game Manager").GetComponent<InteractionManager>().TileClicked(this, xIndex, yIndex);
        }

        /// <summary>
        /// Sets the dice value of this tile
        /// </summary>
        /// <param name="value"></param>
        public void SetDiceValue(int value)
        {
            diceValue = value;
            UpdateDiceValueText();
        }

        /// <summary>
        /// Updates the visible text representing the dice value
        /// </summary>
        public void UpdateDiceValueText()
        {
            text.text = diceValue > 0 ? diceValue.ToString() : "";
            tokenRenderer.enabled = diceValue > 0;
        }

        /// <summary>
        /// Sets whether or not the robber is on the tile
        /// </summary>
        /// <param name="hasRobber"></param>
        public void SetRobber(bool hasRobber)
        {
            robber = hasRobber;
            UpdateRobberMesh();
        }

        /// <summary>
        /// Updates the robber mesh
        /// </summary>
        public void UpdateRobberMesh()
        {
            robberRenderer.enabled = robber;
        }

        /// <summary>
        /// Changes appearance of tile game object
        /// </summary>
        /// <param name="type"></param>
        public void SetAppearance(Tile.TileType type)
        {
            mesh = Instantiate(GameObject.Find("Tile Asset Holder").transform.GetChild((int)type)).gameObject;
            mesh.transform.position = transform.position;
            mesh.transform.rotation = Quaternion.Euler(0, 30 + 60 * UnityEngine.Random.Range(0, 6), 0);
            mesh.transform.parent = transform;
        }
    }
}