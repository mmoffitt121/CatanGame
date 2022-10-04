/// AUTHOR: Alex Rizzo, Matthew Moffitt
/// FILENAME: ResourceDistributor.cs
/// SPECIFICATION: File that distributes resources to players
/// FOR: CS 3368 Introduction to Artificial Intelligence Section 001

using Catan.GameBoard;
using Catan.GameManagement;
using Catan.Players;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

/// <summary>
/// Class responsible for distributing resources to all players.
/// </summary>
public static class ResourceDistributor
{
    /// <summary>
    /// Distributes resources among players for a rolled dice value.
    /// </summary>
    /// <param name="board"></param>
    /// <param name="players"></param>
    /// <param name="diceValue"></param>
    public static void DistributeResources(this Board board, Player[] players, int diceValue)
    {
        for (int i = 0; i < board.tiles.Length; i++)
        {
            for (int j = 0; j < board.tiles[i].Length; j++)
            {
                if (board.tiles[i][j].diceValue == diceValue && !board.tiles[i][j].robber)
                {
                    (int, int)[] sVertices = board.tiles.GetSurroundingVertices(board.vertices, i, j);
                    for (int k = 0; k < sVertices.Length; k++)
                    {
                        if (sVertices[k] != (-1, -1) && board.vertices[sVertices[k].Item1][sVertices[k].Item2].playerIndex != -1)
                        {
                            players[board.vertices[sVertices[k].Item1][sVertices[k].Item2].playerIndex].AddResource(
                                board.tiles[i][j].resourceType, 
                                (int)board.vertices[sVertices[k].Item1][sVertices[k].Item2].development
                                );
                        }
                    }
                }
            }
        }
    }

    /// <summary>
    /// Specifies a certain vertex and distributes to the specified player the resources of the tiles surrounding that vertex.
    /// </summary>
    /// <param name="board"></param>
    /// <param name="players"></param>
    /// <param name="vertex"></param>
    public static void DistributeResourcesFromVertex(this Board board, Player[] players, (int, int) vertex)
    {
        (int, int) above = board.vertices.TileAboveVertex(board.tiles, vertex.Item1, vertex.Item2);
        (int, int) below = board.vertices.TileBelowVertex(board.tiles, vertex.Item1, vertex.Item2);
        (int, int) left = board.vertices.TileLeftOfVertex(board.tiles, vertex.Item1, vertex.Item2);
        (int, int) right = board.vertices.TileRightOfVertex(board.tiles, vertex.Item1, vertex.Item2);

        GameManager gm = GameObject.Find("Game Manager").GetComponent<GameManager>();
        if (above != (-1, -1)) gm.currentPlayer.AddResource(board.tiles[above.Item1][above.Item2].resourceType, 1);
        if (below != (-1, -1)) gm.currentPlayer.AddResource(board.tiles[below.Item1][below.Item2].resourceType, 1);
        if (left != (-1, -1)) gm.currentPlayer.AddResource(board.tiles[left.Item1][left.Item2].resourceType, 1);
        if (right != (-1, -1)) gm.currentPlayer.AddResource(board.tiles[right.Item1][right.Item2].resourceType, 1);
    }
}
