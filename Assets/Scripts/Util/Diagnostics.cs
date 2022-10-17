using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Catan.Util
{
    public static class Diagnostics
    {
        public static void Print<T>(this T[] arr)
        {
            Debug.Log(arr.MakeString());
        }

        public static void Print<T>(this T[][] arr)
        {
            Debug.Log(arr.MakeString());
        }

        public static string MakeString<T>(this T[] arr)
        {
            string sb = "";

            foreach (T t in arr)
            {
                sb = sb + "(" + t.ToString() + ")";
            }

            return sb;
        }

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