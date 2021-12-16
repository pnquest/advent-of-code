namespace System.Linq;

public static class LinqExtensions
{
    public static void SetOrIncrement<T>(this IDictionary<T, int> input, T key, int count)
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

    public static void SetOrIncrement<T>(this IDictionary<T, long> input, T key, long count)
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
        for(int i = 0; i < times; i++)
        {
            foreach(T item in input)
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
}
