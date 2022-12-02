/// AUTHOR: Matthew Moffitt
/// FILENAME: TileLabelGameObject.cs
/// SPECIFICATION: File containing board info
/// FOR: CS 3368 Introduction to Artificial Intelligence Section 001

using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Catan.GameBoard
{
    /// <summary>
    /// GameObject for the label on each tile object
    /// </summary>
    public class TileLabelGameObject : MonoBehaviour
    {
        private void OnMouseDown()
        {
            transform.parent.parent.GetChild(0).GetComponent<TileGameObject>().OnMouseDown();
        }
    }
}

