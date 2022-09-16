using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Catan.Players;

namespace Catan.GameManagement
{
    public class GameManager : MonoBehaviour
    {
        public Player[] players;

        public int turn;
        public int phase;

        public GameObject UIManager;

        public void AdvanceTurn()
        {
            phase++;

            if (phase > 2)
            {
                phase = 0;
                turn++;
            }

            if (turn >= players.Length)
            {
                turn = 0;
            }
        }
    }
}
