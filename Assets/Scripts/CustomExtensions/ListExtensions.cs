using System;
using System.Collections.Generic;

namespace CustomExtensions
{
    public static class ListExtensions
    {
        private static readonly Random rng = new Random();

        public static void Shuffle<T>(this IList<T> list)
        {
            int n = list.Count;
            while (n > 1) {
                n--;
                int k = rng.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }

        public static T Random<T>(this IList<T> list)
        {
            return list[UnityEngine.Random.Range(0, list.Count)];
        }
        
        public static T Random<T>(this T[] list)
        {
            return list[UnityEngine.Random.Range(0, list.Length)];
        }
    }
}
