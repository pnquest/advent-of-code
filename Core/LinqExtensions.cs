namespace System.Linq;

public static class LinqExtensions
{
    public static void SetOrIncrement<T>(this IDictionary<T, int> input, T key, int count)
    {
        if(input.ContainsKey(key))
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
}
