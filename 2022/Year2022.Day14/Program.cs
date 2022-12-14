using Core;

namespace Year2022.Day14;

internal class Program
{
    private static readonly IntSlope _down = new(0, 1);
    private static readonly IntSlope _downLeft = new(-1, 1);
    private static readonly IntSlope _downRight = new(1, 1);

    static void Main(string[] args)
    {
        Part1();
        Part2();
    }

    private static void Part2()
    {
        SetupPuzzle(out HashSet<IntPoint> occupied, out int _, out int _, out int maxY);
        long grains = 0;
        bool blockedOrigin = false;
        int bottomY = maxY + 2;
        var origin = new IntPoint(500, 0);
        while (!blockedOrigin)
        {
            IntPoint curGrain = origin;

            while (true)
            {
                bool movedGrain = TryMoveGrain(ref curGrain, occupied, bottomY);
                if (!movedGrain && curGrain != origin)
                {
                    occupied.Add(curGrain);
                    break;
                }
                else if (!movedGrain && curGrain == origin)
                {
                    occupied.Add(curGrain);
                    blockedOrigin = true;
                    break;
                }
            }

            grains++;
        }

        Console.WriteLine($"Part 2: {grains}");
    }

    private static void Part1()
    {
        SetupPuzzle(out HashSet<IntPoint> occupied, out int minX, out int maxX, out int maxY);

        long grains = 0;
        bool goneOffMap = false;

        while (!goneOffMap)
        {
            var curGrain = new IntPoint(500, 0);

            while (true)
            {
                if (!TryMoveGrain(ref curGrain, occupied))
                {
                    occupied.Add(curGrain);
                    break;
                }

                if (curGrain.X < minX || curGrain.X > maxX || curGrain.Y > maxY)
                {
                    goneOffMap = true;
                    break;
                }
            }

            if (!goneOffMap)
            {
                grains++;
            }
        }

        Console.WriteLine($"Part 1: {grains}");
    }

    private static bool TryMoveGrain(ref IntPoint curGrain, IntSlope direction, HashSet<IntPoint> occupied, int floorY)
    {
        IntPoint newPosition = curGrain + direction;
        if (!occupied.Contains(newPosition) && newPosition.Y != floorY)
        {
            curGrain = newPosition;
            return true;
        }
        return false;
    }

    private static bool TryMoveGrain(ref IntPoint curGrain, HashSet<IntPoint> occupied, int floorY)
    {
        if (TryMoveGrain(ref curGrain, _down, occupied, floorY))
        {
            return true;
        }
        else if (TryMoveGrain(ref curGrain, _downLeft, occupied, floorY))
        {
            return true;
        }
        else if (TryMoveGrain(ref curGrain, _downRight, occupied, floorY))
        {
            return true;
        }

        return false;
    }

    private static bool TryMoveGrain(ref IntPoint curGrain, HashSet<IntPoint> occupied)
    {
        if (!occupied.Contains(curGrain + _down))
        {
            curGrain += _down;
            return true;
        }
        else if (!occupied.Contains(curGrain + _downLeft))
        {
            curGrain += _downLeft;
            return true;
        }
        else if (!occupied.Contains(curGrain + _downRight))
        {
            curGrain += _downRight;
            return true;
        }

        return false;
    }

    private static void SetupPuzzle(out HashSet<IntPoint> occupied, out int minX, out int maxX, out int maxY)
    {
        occupied = new HashSet<IntPoint>();
        int curMinX = int.MaxValue;
        int curMaxX = int.MinValue;
        int curMaxY = int.MinValue;
        foreach (string line in File.ReadAllLines("./input.txt"))
        {
            IntPoint[] splits = line.Split(" -> ").Select(s => {
                var pt = IntPoint.Parse(s, null);
                CheckMaximums(ref curMinX, ref curMaxX, ref curMaxY, pt);
                return pt;
            }).ToArray();

            for (int i = 1; i < splits.Length; i++)
            {
                IntSlope slope = (splits[i] - splits[i - 1]).CalculateIntegerUnit();
                IntPoint cur = splits[i - 1];
                while (true)
                {
                    occupied.Add(cur);
                    if (cur == splits[i])
                    {
                        break;
                    }
                    cur += slope;
                }
            }
        }

        minX = curMinX;
        maxX = curMaxX;
        maxY = curMaxY;
    }

    private static void CheckMaximums(ref int minX, ref int maxX, ref int maxY, IntPoint cur)
    {
        if (cur.X > maxX)
        {
            maxX = cur.X;
        }
        if (cur.X < minX)
        {
            minX = cur.X;
        }
        if (cur.Y > maxY)
        {
            maxY = cur.Y;
        }
    }
}
