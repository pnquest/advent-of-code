using Core;
using Core.Pathfinding;

namespace Day122022;

internal class Program
{
    static void Main(string[] args)
    {
        IntPoint start = default;
        IntPoint end = default;

        AStarNode<int>[][] nodes = File.ReadAllLines("./input.txt")
            .Select((l, y) => l.Select((c, x) => {
                if (c == 'S')
                {
                    start = new IntPoint(x, y);
                    c = 'a';
                }
                else if (c == 'E')
                {
                    end = new IntPoint(x, y);
                    c = 'z';
                }

                return new AStarNode<int>(new IntPoint(x, y), 1, true, c - 'a');
            }).ToArray()).ToArray();

        Part1(start, end, nodes);
        Part2(end, nodes);
    }

    private static void Part2(IntPoint end, AStarNode<int>[][] nodes)
    {
        int curMin = int.MaxValue;
        foreach (AStarNode<int> node in nodes.SelectMany(n => n).Where(n => n.ExtraData == 0))
        {
            IntPoint curStart = node.Location;
            var pathFinder = new AStarPathFinder<int>(nodes, curStart, end, passableFunc: PassableFunction);
            int result = pathFinder.SolvePath().Count() - 1;
            if (result > 0 && result < curMin)
            {
                curMin = result;
            }
        }
        Console.WriteLine($"Part 2: {curMin}");
    }

    private static void Part1(IntPoint start, IntPoint end, AStarNode<int>[][] nodes)
    {
        var pathFinder = new AStarPathFinder<int>(nodes, start, end, passableFunc: PassableFunction);
        int result = pathFinder.SolvePath().Count() - 1;
        Console.WriteLine($"Part 1: {result}");
    }

    private static bool PassableFunction(AStarNode<int> from, AStarNode<int> to)
    {
        return (to.ExtraData - from.ExtraData) <= 1;
    }
}
