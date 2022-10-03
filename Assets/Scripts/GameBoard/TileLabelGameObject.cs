using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Catan.GameBoard
{
    public class TileLabelGameObject : MonoBehaviour
    {
        private void OnMouseDown()
        {
            transform.parent.parent.GetChild(0).GetComponent<TileGameObject>().OnMouseDown();
        }
    }
}

