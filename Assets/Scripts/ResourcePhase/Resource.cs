/// AUTHOR: Matthew Moffitt
/// FILENAME: Resource.cs
/// SPECIFICATION: File that manages resource data
/// FOR: CS 3368 Introduction to Artificial Intelligence Section 001

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Catan.ResourcePhase
{
    public class Resource
    {
        public ResourceType type;
        public int amount;
        public Texture2D texture;

        public enum ResourceType
        {
            Any,
            Wool,
            Grain,
            Wood,
            Brick,
            Ore,
            None
        }

        public Resource(ResourceType type, int amount)
        {
            this.type = type;
            this.amount = amount;
        }
    }
}
