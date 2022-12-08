using Core;

namespace Day082022;

internal class Program
{
    static void Main(string[] args)
    {
        Tree[][] grid = File.ReadAllLines("./input.txt")
            .Select((f, y) => f.Select((c, x) => new Tree(new IntPoint(x, y), c - 48)).ToArray())
            .ToArray();

        Part1(grid);
        Part2(grid);
    }

    private static void Part2(Tree[][] grid)
    {
        int curScore = 0;

        for (int y = 0; y < grid.Length; y++)
        {
            for (int x = 0; x < grid[y].Length; x++)
            {
                curScore = ScoreTree(grid, curScore, y, x);
            }
        }

        Console.WriteLine($"Part 2: {curScore}");
    }

    private static int ScoreTree(Tree[][] grid, int curScore, int y, int x)
    {
        Tree cur = grid[y][x];
        int curX = x - 1;

        Span<int> seenCounts = stackalloc int[4];
        ScanLeft(grid, y, cur, curX, seenCounts);
        ScanRight(grid, y, x, cur, seenCounts);
        ScanUp(grid, y, x, cur, seenCounts);
        ScanDown(grid, y, x, cur, seenCounts);
        int score = seenCounts[0] * seenCounts[1] * seenCounts[2] * seenCounts[3];
        if (score > curScore)
        {
            curScore = score;
        }

        return curScore;
    }

    private static void ScanDown(Tree[][] grid, int y, int x, Tree cur, Span<int> seenCounts)
    {
        seenCounts[3] = 0;
        int curY = y + 1;
        while (curY < grid.Length)
        {
            Tree next = grid[curY++][x];
            seenCounts[3]++;
            if (next.Height >= cur.Height)
            {
                break;
            }
        }
    }

    private static void ScanUp(Tree[][] grid, int y, int x, in Tree cur, Span<int> seenCounts)
    {
        seenCounts[2] = 0;
        int curY = y - 1;
        while (curY >= 0)
        {
            Tree next = grid[curY--][x];
            seenCounts[2]++;
            if (next.Height >= cur.Height)
            {
                break;
            }
        }
    }

    private static void ScanRight(Tree[][] grid, int y, int x, in Tree cur, Span<int> seenCounts)
    {
        int curX;
        seenCounts[1] = 0;
        curX = x + 1;
        while (curX < grid[y].Length)
        {
            Tree next = grid[y][curX++];
            seenCounts[1]++;
            if (next.Height >= cur.Height)
            {
                break;
            }
        }
    }

    private static void ScanLeft(Tree[][] grid, int y, Tree cur, int curX, Span<int> seenCounts)
    {
        seenCounts[0] = 0;

        while (curX >= 0)
        {
            Tree next = grid[y][curX--];
            seenCounts[0]++;
            if (next.Height >= cur.Height)
            {
                break;
            }
        }
    }

    private static void Part1(Tree[][] grid)
    {
        var visiblePoints = new HashSet<IntPoint>();

        for (int y = 0; y < grid.Length; y++)
        {
            int highestSeen = -1;

            for (int x = 0; x < grid[y].Length; x++)
            {
                CheckTree(grid[y][x], visiblePoints, ref highestSeen);
            }

            highestSeen = -1;

            for (int x = grid[y].Length - 1; x >= 0; x--)
            {
                CheckTree(grid[y][x], visiblePoints, ref highestSeen);
            }
        }

        for (int x = 0; x < grid[0].Length; x++)
        {
            int highestSeen = -1;

            for (int y = 0; y < grid.Length; y++)
            {
                CheckTree(grid[y][x], visiblePoints, ref highestSeen);
            }

            highestSeen = -1;

            for (int y = grid.Length - 1; y >= 0; y--)
            {
                CheckTree(grid[y][x], visiblePoints, ref highestSeen);
            }
        }

        Console.WriteLine($"Part 1: {visiblePoints.Count}");
    }

    private static void CheckTree(in Tree cur, HashSet<IntPoint> visiblePoints, ref int highestSeen)
    {
        if (cur.Height > highestSeen)
        {
            highestSeen = cur.Height;
            visiblePoints.Add(cur.Pos);
        }
    }

    internal readonly record struct Tree(IntPoint Pos, int Height);
}
