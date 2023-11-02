using System;
using System.Collections.Generic;
using System.Linq;
using Algorithms.RogueSharp.Random;

namespace Utilities
{
    public static class RandomUtilities
    {
        public static IEnumerable<T> GetNRandomItems<T>(this List<T> source, IRandom random, int count)
        {
            for (int i = 0; i < count; i++)
            { 
                yield return source.GetRandomItem(random);
            }
        }

        public static T GetRandomItem<T>(this List<T> source, IRandom random)
        {
            return source[random.Next(0, source.Count - 1)];
        }

        public static T GetRandomItem<T>(this List<T> source)
        {
            return source.GetRandomItem(new DotNetRandom());
        }

        public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> source)
        {
            return source.OrderBy(x => Guid.NewGuid());
        }

        public static T RandomEnumValue<T>(IRandom random)
        {
            var v = Enum.GetValues(typeof(T));
            return (T)v.GetValue(random.Next(v.Length - 1));
        }
    }
}