using Core;

namespace Day17;

public static class Program
{
    public static void Main()
    {
        Part1();
        Part2();
    }

    private static void Part2()
    {
        const int maxY = 188;
        const int maxX = 70;

        const int minXBound = 48;
        const int maxXBound = 70;
        const int minYBound = -189;
        const int maxYBound = -148;

        int hitCount = 0;

        int curMinX = 1;

        for (int y = minYBound; y <= maxY; y++)
        {
            for (int x = curMinX; x <= maxX; x++)
            {
                IntPoint curPos = new IntPoint(0, 0);
                IntSlope curVelocity = new IntSlope(x, y);

                while (curPos.X <= maxXBound && curPos.Y >= minYBound)
                {
                    if (curPos.X >= minXBound && curPos.X <= maxXBound && curPos.Y >= minYBound && curPos.Y <= maxYBound)
                    {
                        hitCount++;
                        break;
                    }

                    curPos += curVelocity;

                    if (curVelocity.X == 0 && curPos.X < minXBound)
                    {
                        curMinX = x + 1;
                        break;
                    }

                    curVelocity = new IntSlope(Math.Max(0, curVelocity.X - 1), curVelocity.Y - 1);
                }
            }
        }

        Console.WriteLine($"Part 2: {hitCount}");
    }

    private static void Part1()
    {
        int curPosition = 0;
        int curAcc = 188;

        while (curAcc > 0)
        {
            curPosition += curAcc--;
        }

        Console.WriteLine($"Part 1: {curPosition}");
    }
}
