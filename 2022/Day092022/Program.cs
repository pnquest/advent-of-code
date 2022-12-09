using Core;

namespace Day092022;

internal class Program
{
    static void Main(string[] args)
    {
        Move[] moves = File.ReadAllLines("./input.txt")
            .Select(l => new Move(GetDirection(l[0]), int.Parse(l.AsSpan()[2..])))
            .ToArray();
        Part1(moves);
        Part2(moves);
    }

    private static void Part2(Move[] moves)
    {
        IntPoint[] points = Enumerable.Repeat(new IntPoint(0, 0), 10).ToArray();

        var tailVisited = new HashSet<IntPoint>
        {
            points[9]
        };

        foreach (ref readonly Move move in moves.AsSpan())
        {
            IntSlope slp = CalculateSlope(move);
            for (int i = 0; i < move.Steps; i++)
            {
                points[0] += slp;

                for (int j = 1; j < points.Length; j++)
                {
                    if (!AreKnotsAdjacent(points[j - 1], points[j]))
                    {
                        MoveTail(points[j - 1], ref points[j], tailVisited, j == 9);
                    }
                }
            }
        }

        Console.WriteLine($"Part 2: {tailVisited.Count}");
    }

    private static void Part1(Move[] moves)
    {
        IntPoint curHead = new IntPoint(0, 0);
        IntPoint curTail = new IntPoint(0, 0);

        var tailVisited = new HashSet<IntPoint>
        {
            curTail
        };

        foreach (ref readonly Move move in moves.AsSpan())
        {
            IntSlope slp = CalculateSlope(move);

            for (int i = 0; i < move.Steps; i++)
            {
                curHead += slp;
                if (!AreKnotsAdjacent(curHead, curTail))
                {
                    MoveTail(curHead, ref curTail, tailVisited, true);
                }
            }
        }

        Console.WriteLine($"Part 1: {tailVisited.Count}");
    }

    private static void MoveTail(IntPoint curHead, ref IntPoint curTail, HashSet<IntPoint> tailVisited, bool isFinalKnot)
    {
        IntSlope diagSlp = (curHead - curTail).CalculateIntegerUnit();
        curTail += diagSlp;

        if (isFinalKnot)
        {
            tailVisited.Add(curTail);
        }
    }

    private static bool AreKnotsAdjacent(IntPoint curHead, IntPoint curTail) 
        => curTail == curHead 
            || curHead.GetNeighbors(int.MinValue, int.MaxValue, int.MinValue, int.MaxValue, true).Contains(curTail);

    public static IntSlope CalculateSlope(in Move mv)
    {
        return mv.Direction switch {
            Enumerations.Directions.Right => new IntSlope(1, 0),
            Enumerations.Directions.Left => new IntSlope(-1, 0),
            Enumerations.Directions.Up => new IntSlope(0, -1),
            Enumerations.Directions.Down => new IntSlope(0, 1),
            _ => throw new InvalidOperationException("InvalidMove")
        };
    }

    public static Enumerations.Directions GetDirection(char input)
    {
        return input switch 
        {
            'R' => Enumerations.Directions.Right,
            'U' => Enumerations.Directions.Up,
            'L' => Enumerations.Directions.Left,
            'D' => Enumerations.Directions.Down,
            _ => throw new ArgumentException("invalid input")
        };
    }

    public readonly record struct Move(Enumerations.Directions Direction, int Steps);
}
