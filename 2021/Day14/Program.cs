namespace Day14;

public static class Program
{
    public static void Main()
    {
        Part1();
        Part2();
    }

    private static void Part2()
    {
        RunProcessFast(40, out KeyValuePair<char, long> max, out KeyValuePair<char, long> min, out long result);

        Console.WriteLine($"Part 2: Min Char {min.Key} ({min.Value}); Max Char {max.Key} ({max.Value}) -- Result = {result}");
    }

    private static void Part1()
    {
        RunProcessFast(10, out KeyValuePair<char, long> max, out KeyValuePair<char, long> min, out long result);
        Console.WriteLine($"Part 1: Min Char {min.Key} ({min.Value}); Max Char {max.Key} ({max.Value}) -- Result = {result}");
    }

    private static void RunProcessFast(int cycles, out KeyValuePair<char, long> max, out KeyValuePair<char, long> min, out long result)
    {
        string[] lines = File.ReadAllLines("./input.txt");

        Dictionary<char, long> counts = lines[0].GroupBy(c => c).ToDictionary(c => c.Key, c => c.LongCount());
        Dictionary<string, long> pairCounts = [];

        for (int i = 1; i < lines[0].Length; i++)
        {
            string pair = new(new[] { lines[0][i - 1], lines[0][i] });
            pairCounts.SetOrIncrement(pair, 1);
        }

        Dictionary<string, char> singleRecipies = lines.Skip(2).ToDictionary(s => s[..2], s => s[^1]);

        for (int i = 0; i < cycles; i++)
        {
            Dictionary<string, long> newPairs = [];

            foreach (KeyValuePair<string, long> pair in pairCounts)
            {
                char charToAdd = singleRecipies[pair.Key];
                counts.SetOrIncrement(charToAdd, pair.Value);

                string newPair = new(new[] { pair.Key[0], charToAdd });
                newPairs.SetOrIncrement(newPair, pair.Value);

                string otherNewPair = new(new[] { charToAdd, pair.Key[1] });
                newPairs.SetOrIncrement(otherNewPair, pair.Value);
            }

            pairCounts = newPairs;
        }

        max = counts.MaxBy(c => c.Value);
        min = counts.MinBy(c => c.Value);
        result = max.Value - min.Value;
    }
}
