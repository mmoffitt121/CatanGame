using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Catan.ResourcePhase
{
    public class Resource
    {
        public ResourceType type;
        public int amount;

        public enum ResourceType
        {
            Wool,
            Grain,
            Wood,
            Brick,
            Ore
        }
    }
}
