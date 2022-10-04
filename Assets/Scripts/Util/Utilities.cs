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
    public static class Utilities
    {
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
