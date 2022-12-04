using System.Numerics;

namespace System.Linq;

public static class LinqExtensions
{
    public static void SetOrIncrement<T, N>(this IDictionary<T, N> input, T key, N count)
        where N : IAdditionOperators<N, N, N>
    {
        if (input.ContainsKey(key))
        {
            input[key] += count;
        }
        else
        {
            input[key] = count;
        }
    }

    public static IEnumerable<T> Repeat<T>(this IEnumerable<T> input, int times)
    {
        for (int i = 0; i < times; i++)
        {
            foreach (T item in input)
            {
                yield return item;
            }
        }
    }

    public static IEnumerable<O> Repeat<T, O>(this IEnumerable<T> input, int times, Func<T, int, O> transform)
    {
        for (int i = 0; i < times; i++)
        {
            foreach (T item in input)
            {
                yield return transform(item, i);
            }
        }
    }

    public static IEnumerable<T[]> PartitionBy<T>(this IEnumerable<T> input, Func<T, bool> partitionFunc, bool includePartitionBoundary = false)
    {
        var items = new List<T>();

        foreach (T item in input)
        {
            if (partitionFunc(item))
            {
                if (includePartitionBoundary)
                {
                    items.Add(item);
                }
                yield return items.ToArray();
                items.Clear();
            }
            else
            {
                items.Add(item);
            }
        }
    }
}
