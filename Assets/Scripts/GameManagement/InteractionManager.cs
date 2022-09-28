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
                }
                if (obj is RoadGameObject)
                {
                    board.roads[i][j].playerIndex = gameManager.turn;
                    obj.SetPlayer(gameManager.currentPlayer);
                }
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
