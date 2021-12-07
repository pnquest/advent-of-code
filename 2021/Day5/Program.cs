using Core;

namespace Day5;

public static class Program
{
    public static void Main()
    {
        Part1();
        Part2();
        Part2BruteForce();
    }

    private static void Part2BruteForce()
    {
        LineSegment[] lines = File.ReadAllLines("./input.txt").Select(x => {
            var splt = x.Split(" -> ");
            var p1 = splt[0].Split(",");
            var p2 = splt[1].Split(",");

            return new LineSegment(new Point(int.Parse(p1[0]), int.Parse(p1[1])), new Point(int.Parse(p2[0]), int.Parse(p2[1])));
        }).ToArray();

        HashSet<Point> seenFirst = new();
        HashSet<Point> seenAFter = new();

        foreach(LineSegment line in lines)
        {
            Slope slope = line.GetSlope();

            Point cur = line.P1;

            while(cur != line.P2)
            {
                if(!seenFirst.Add(cur))
                {
                    seenAFter.Add(cur);
                }

                cur = cur + slope;
            }

            if (!seenFirst.Add(line.P2))
            {
                seenAFter.Add(line.P2);
            }
        }

        Console.WriteLine($"Part 2 Brute Force: {seenAFter.Count}");
    }

    private static void Part2()
    {
        LineSegment[] lines = File.ReadAllLines("./input.txt").Select(x => {
            var splt = x.Split(" -> ");
            var p1 = splt[0].Split(",");
            var p2 = splt[1].Split(",");

            return new LineSegment(new Point(int.Parse(p1[0]), int.Parse(p1[1])), new Point(int.Parse(p2[0]), int.Parse(p2[1])));
        }).ToArray();

        HashSet<Point> overlappingPoints = new();

        for (int i = 0; i < lines.Length - 1; i++)
        {
            for (int j = i + 1; j < lines.Length; j++)
            {
                foreach (Point o in lines[i].GetCommonPoints(lines[j]))
                {
                    overlappingPoints.Add(o);
                }
            }
        }

        Console.WriteLine($"Part 2: {overlappingPoints.Count}");
    }

    private static void Part1()
    {
        LineSegment[] lines = File.ReadAllLines("./input.txt").Select(x => {
            var splt = x.Split(" -> ");
            var p1 = splt[0].Split(",");
            var p2 = splt[1].Split(",");

            return new LineSegment(new Point(int.Parse(p1[0]), int.Parse(p1[1])), new Point(int.Parse(p2[0]), int.Parse(p2[1])));
        }).ToArray();

        HashSet<Point> overlappingPoints = new();

        lines = lines.Where(l => l.P1.X == l.P2.X || l.P1.Y == l.P2.Y).ToArray();

        for (int i = 0; i < lines.Length - 1; i++)
        {
            for (int j = i + 1; j < lines.Length; j++)
            {
                foreach (Point o in lines[i].GetCommonPoints(lines[j]))
                {
                    overlappingPoints.Add(o);
                }
            }
        }

        Console.WriteLine($"Part 1: {overlappingPoints.Count}");
    }
}
