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
                }
                if (obj is RoadGameObject)
                {
                    board.roads[i][j].playerIndex = gameManager.turn;
                    obj.SetPlayer(gameManager.currentPlayer);
                }
            }
        }
    }
}
