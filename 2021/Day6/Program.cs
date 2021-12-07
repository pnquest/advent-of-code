namespace Day6;

public static class Program
{
    public static void Main()
    {
        Part1();
        Part2();
    }

    private static void Part1()
    {
        long result = RunSimulation(80);

        Console.WriteLine($"Part 1: {result}");
    }

    private static void Part2()
    {
        long result = RunSimulation(256);

        Console.WriteLine($"Part 2: {result}");
    }

    private static long RunSimulation(int days)
    {
        Dictionary<int, long>? buckets = Enumerable.Range(0, 9).ToDictionary(d => d, _ => 0L);

        File.ReadAllText("./input.txt").Split(",").Select(int.Parse).Aggregate(buckets, (b, v) => {
            b[v]++;
            return b;
        });

        for (int i = 0; i < days; i++)
        {
            Dictionary<int, long> bucketsNew = new(9);

            for (int j = 8; j >= 0; j--)
            {
                if (j > 0)
                {
                    bucketsNew[j - 1] = buckets[j];
                }
                else
                {
                    bucketsNew[8] = buckets[j];
                    bucketsNew[6] += buckets[j];
                }
            }

            buckets = bucketsNew;
        }

        long result = buckets.Values.Sum();
        return result;
    }
}
