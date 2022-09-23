using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Catan.Players;
using Catan.GameManagement;

namespace Catan.GameBoard
{
    public class TileVertexGameObject : BoardTokenGameObject
    {
        public GameObject port;
        public GameObject city;

        public override void SetPlayer(Player player)
        {
            base.SetPlayer(player);
            port.GetComponent<Renderer>().material.color = player.playerColor;
        }

        public void UpdateMesh()
        {
            switch (GameObject.Find("Board").GetComponent<Board>().vertices[xIndex][yIndex].development)
            {
                case TileVertex.Development.Undeveloped:
                    meshRenderer.enabled = false;
                    city.GetComponent<MeshRenderer>().enabled = false;
                    break;
                case TileVertex.Development.Town:
                    meshRenderer.enabled = true;
                    city.GetComponent<MeshRenderer>().enabled = false;
                    break;
                case TileVertex.Development.City:
                    meshRenderer.enabled = true;
                    city.GetComponent<MeshRenderer>().enabled = true;
                    break;
                default:
                    break;
            }
        }
    }
}