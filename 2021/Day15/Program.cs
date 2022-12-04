using Core.Pathfinding;

namespace Day15;

public static class Program
{
    public static void Main()
    {
        Part1();
        Part2();
    }

    private static void Part2()
    {
        int[][] ints = File.ReadAllLines("./input.txt").Select(l => l.Select(c => c - '0').ToArray()).ToArray();
        AStarNode[][] nodes = new AStarNode[ints.Length * 5][];

        for (int i = 0; i < nodes.Length; i++)
        {
            nodes[i] = new AStarNode[ints.Length * 5];
        }

        for (int y = 0; y < nodes.Length; y++)
        {
            for (int x = 0; x < nodes[y].Length; x++)
            {
                int mapped = ints[y % ints.Length][x % ints.Length];
                int loop = y / ints.Length + x / ints.Length;
                mapped = ModifyRisk(mapped, loop);
                nodes[y][x] = new AStarNode(new Core.IntPoint(x, y), mapped, true);
            }
        }

        AStarPathFinder pathFinder = new(nodes, new Core.IntPoint(0, 0), new Core.IntPoint(nodes[0].Length - 1, nodes.Length - 1));

        int answer = pathFinder.SolvePath().SkipLast(1).Sum(s => s.Node.Cost);

        Console.WriteLine($"Part 2: {answer}");
    }

    private static int ModifyRisk(int val, int loop)
    {
        int result = val + loop;

        if (result > 9)
        {
            return result - 9 * (result / 9);
        }

        return result;
    }

    private static void Part1()
    {
        AStarNode[][] nodes = File.ReadAllLines("./input.txt")
                    .Select((n, y) => n.Select((c, x) => new AStarNode(new Core.IntPoint(x, y), c - '0', true)).ToArray())
                    .ToArray();

        AStarPathFinder pathFinder = new(nodes, new Core.IntPoint(0, 0), new Core.IntPoint(nodes[0].Length - 1, nodes.Length - 1));

        int answer = pathFinder.SolvePath().SkipLast(1).Sum(s => s.Node.Cost);

        Console.WriteLine($"Part 1: {answer}");
    }
}
