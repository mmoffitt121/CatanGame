/// AUTHOR: Matthew Moffitt
/// FILENAME: Utilities.cs
/// SPECIFICATION: File containing helpful utilities for general use
/// FOR: CS 3368 Introduction to Artificial Intelligence Section 001

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Catan.Players;

namespace Catan.Util
{
    /// <summary>
    /// Utilities tha thelp with game calculations
    /// </summary>
    public static class Utilities
    {
        /// <summary>
        /// Selects the max value by a specified selector
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="arr"></param>
        /// <param name="selector"></param>
        /// <returns></returns>
        public static T SelectMax<T>(this T[] arr, Func<T, int> selector)
        {
            int max = 0;
            int maxIndex = 0;
            for (int i = 0; i < arr.Length; i++)
            {
                int p = selector(arr[i]);
                if (p > max)
                {
                    max = p;
                    maxIndex = i;
                }
            }
            return arr[maxIndex];
        }

        /// <summary>
        /// Returns the first value that returns true for a specified condition
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="arr"></param>
        /// <param name="selector"></param>
        /// <returns></returns>
        public static T FirstTrue<T>(this T[] arr, Func<T, bool> selector)
        {
            foreach (T item in arr)
            {
                if (selector(item))
                {
                    return item;
                }
            }

            return default(T);
        }
    }
}
