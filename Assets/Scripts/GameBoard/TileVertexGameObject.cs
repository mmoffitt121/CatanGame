/// AUTHOR: Matthew Moffitt
/// FILENAME: TileVertexGameObject.cs
/// SPECIFICATION: File containing tile gameobject info
/// FOR: CS 3368 Introduction to Artificial Intelligence Section 001

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
        public GameObject village;
        public GameObject city;

        public override void SetPlayer(Player player)
        {
            base.SetPlayer(player);
            port.GetComponent<Renderer>().material.color = player.playerColor;
            city.GetComponent<Renderer>().material.color = player.playerColor;
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
                    meshRenderer.enabled = false;
                    city.GetComponent<MeshRenderer>().enabled = true;
                    break;
                default:
                    break;
            }
        }

        public void Start()
        {
            if (GameObject.Find("Board").GetComponent<Board>().vertices[xIndex][yIndex].up)
            {
                int rotation = Random.Range(0, 3);
                village.transform.rotation = Quaternion.Euler(0, 60 + rotation * 120, 0);
                city.transform.rotation = Quaternion.Euler(0, 60 + rotation * 120, 0);
            }
            else
            {
                int rotation = Random.Range(0, 3);
                village.transform.rotation = Quaternion.Euler(0, rotation * 120, 0);
                city.transform.rotation = Quaternion.Euler(0, rotation * 120, 0);
            }
        }
    }
}