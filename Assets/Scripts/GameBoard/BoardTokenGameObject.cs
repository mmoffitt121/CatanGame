/// AUTHOR: Matthew Moffitt
/// FILENAME: BoardTokenGameObject.cs
/// SPECIFICATION: File containing board info
/// FOR: CS 3368 Introduction to Artificial Intelligence Section 001

using Catan.GameManagement;
using Catan.Players;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Catan.GameBoard
{
    public class BoardTokenGameObject : MonoBehaviour
    {
        public MeshRenderer meshRenderer;
        public int playerIndex = -1;
        public int xIndex;
        public int yIndex;
        public int xCoord;
        public int yCoord;

        public void OnMouseEnter()
        {
            if (playerIndex == -1) meshRenderer.enabled = true;
        }

        public void OnMouseExit()
        {
            if (playerIndex == -1) meshRenderer.enabled = false;
        }

        public void OnMouseDown()
        {
            GameObject.Find("Game Manager").GetComponent<InteractionManager>().BoardTokenClicked(this, xIndex, yIndex);
        }

        public virtual void SetPlayer(Player player)
        {
            playerIndex = player.playerIndex;
            meshRenderer.enabled = true;
            GetComponent<Renderer>().material.color = player.playerColor;
            meshRenderer.material.SetColor("_BaseColor", player.playerColor);
        }
    }
}

