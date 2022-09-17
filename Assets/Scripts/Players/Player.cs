using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Catan.ResourcePhase;

namespace Catan.Players
{
    public class Player
    {
        public Color playerColor;

        public Color primaryUIColor;
        public Color secondaryUIColor;
        public string playerName;
        public int playerIndex;

        public Resource[] resources;
    }
}
