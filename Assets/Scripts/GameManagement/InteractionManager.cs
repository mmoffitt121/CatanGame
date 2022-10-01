using Catan.GameBoard;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Catan.GameManagement
{
    public class InteractionManager : MonoBehaviour
    {
        public GameManager gameManager;
        public Board board;
        public void BoardTokenClicked(BoardTokenGameObject obj, int i, int j)
        {
            if (gameManager.phase == 2)
            {
                if (obj is TileVertexGameObject)
                {
                    board.vertices[i][j].playerIndex = gameManager.turn;
                    obj.SetPlayer(gameManager.currentPlayer);
                    board.vertices[i][j].AdvanceDevelopment();
                    ((TileVertexGameObject)obj).UpdateMesh();
                    gameManager.UpdateScores();
                }
                if (obj is RoadGameObject)
                {
                    board.roads[i][j].playerIndex = gameManager.turn;
                    obj.SetPlayer(gameManager.currentPlayer);
                    gameManager.UpdateScores();
                }
            }
        }

        public void TileClicked(TileGameObject obj, int i, int j)
        {
            if (gameManager.movingRobber)
            {
                board.tiles[gameManager.robberLocation.Item1][gameManager.robberLocation.Item2].robber = false;
                board.tiles[i][j].robber = true;

                GameObject.Find("Tile(" + gameManager.robberLocation.Item1 + "," + gameManager.robberLocation.Item2 + ")").transform.GetChild(0).GetComponent<TileGameObject>().SetRobber(false);
                GameObject.Find("Tile(" + i + "," + j + ")").transform.GetChild(0).GetComponent<TileGameObject>().SetRobber(true);

                gameManager.robberLocation = (i, j);
            }
        }

        public void DumpVertexInfo(int i, int j)
        {
            Debug.Log("Vertex ( " + board.vertices[i][j].xCoord + " " + board.vertices[i][j].yCoord + " ): "
                + "\nData Index: " + board.vertices[i][j].xDataIndex + " " + board.vertices[i][j].yDataIndex
                + "\nAbove: " + board.vertices.TileAboveVertex(board.tiles, i, j)
                + "\nBelow: " + board.vertices.TileBelowVertex(board.tiles, i, j)
                + "\nTo Right: " + board.vertices.TileRightOfVertex(board.tiles, i, j)
                + "\nTo Left: " + board.vertices.TileLeftOfVertex(board.tiles, i, j)
                );
        }
    }
}
