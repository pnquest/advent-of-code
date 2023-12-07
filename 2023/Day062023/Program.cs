namespace Day062023;

internal class Program
{
    static void Main(string[] args)
    {
        string[] lines = File.ReadAllLines("./input.txt");
        Part1(lines);
        Part2(lines);
    }

    private static void Part2(string[] lines)
    {
        int time = int.Parse(lines[0].Split(':')[1].Replace(" ", string.Empty));
        long distance = long.Parse(lines[1].Split(':')[1].Replace(" ", string.Empty));

        int result = CalculateWays(time, distance);
        Console.WriteLine($"Part 2: {result}");
    }

    private static void Part1(string[] lines)
    {
        int[] times = lines[0].Split(':')[1].Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToArray();
        int[] distances = lines[1].Split(':')[1].Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToArray();

        List<int> ways = [];

        for (int i = 0; i < times.Length; i++)
        {
            int time = times[i];
            int distance = distances[i];
            int curWays = CalculateWays(time, distance);

            ways.Add(curWays);
        }

        Console.WriteLine($"Part 1: {ways.Aggregate(1, (a, w) => a * w)}");
    }

    private static int CalculateWays(long time, long distance)
    {
        bool isFullySymmetrical = time % 2 != 0;
        int curWays = 0;

        for (long cur = time / 2; cur >= 0; cur--)
        {
            if (cur * (time - cur) > distance)
            {
                curWays++;
            }
            else
            {
                break;
            }
        }

        curWays *= 2;
        if (!isFullySymmetrical)
        {
            curWays--;
        }

        return curWays;
    }
}
