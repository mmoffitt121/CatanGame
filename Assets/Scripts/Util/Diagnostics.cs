using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Catan.Util
{
    /// <summary>
    /// Diagnostics class that helps with debugging
    /// </summary>
    public static class Diagnostics
    {
        /// <summary>
        /// Prints array to string
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="arr"></param>
        public static void Print<T>(this T[] arr)
        {
            Debug.Log(arr.MakeString());
        }

        /// <summary>
        /// Prints double jointed array to string
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="arr"></param>
        public static void Print<T>(this T[][] arr)
        {
            Debug.Log(arr.MakeString());
        }

        /// <summary>
        /// Turns array to string
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="arr"></param>
        /// <returns></returns>
        public static string MakeString<T>(this T[] arr)
        {
            string sb = "";

            foreach (T t in arr)
            {
                sb = sb + "(" + t.ToString() + ")";
            }

            return sb;
        }

        /// <summary>
        /// Turns double jointed array to string
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="arr"></param>
        /// <returns></returns>
        public static string MakeString<T>(this T[][] arr)
        {
            string sb = "";

            foreach (T[] t in arr)
            {
                sb = sb + t.MakeString();
            }

            return sb;
        }
    }
}