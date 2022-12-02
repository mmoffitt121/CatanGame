/// AUTHOR: Matthew Moffitt
/// FILENAME: Resource.cs
/// SPECIFICATION: File that manages resource data
/// FOR: CS 3368 Introduction to Artificial Intelligence Section 001

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Catan.ResourcePhase
{
    /// <summary>
    /// Represents a resource of a specific type and specific amount
    /// </summary>
    public class Resource
    {
        /// <summary>
        /// The type of resource
        /// </summary>
        public ResourceType type;
        /// <summary>
        /// The amount of resource
        /// </summary>
        public int amount;
        /// <summary>
        /// The texture for this specific resource
        /// </summary>
        public Texture2D texture;

        /// <summary>
        /// Enum representing the resource type
        /// </summary>
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

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="type"></param>
        /// <param name="amount"></param>
        public Resource(ResourceType type, int amount)
        {
            this.type = type;
            this.amount = amount;
        }

        /// <summary>
        /// Returns resource as a string containing amount and type
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return "(" + type.ToString() + ": " + amount + ")";
        }
    }
}
