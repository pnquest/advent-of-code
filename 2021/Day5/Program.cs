using Core;

namespace Day5;

public static class Program
{
    public static void Main()
    {
        Part1();
        var part2Start = DateTime.Now;
        Part2();
        var part2End = DateTime.Now;

        var bruteForceStart = DateTime.Now;
        Part2BruteForce();
        var bruteForceEnd = DateTime.Now;

        Console.WriteLine($"Part 2 perf difference: Geometry = {part2End - part2Start}; Brute Force = {bruteForceEnd - bruteForceStart}");
    }

    private static void Part2BruteForce()
    {
        IntLineSegment[] lines = File.ReadAllLines("./input.txt").Select(x => {
            var splt = x.Split(" -> ");
            var p1 = splt[0].Split(",");
            var p2 = splt[1].Split(",");

            return new IntLineSegment(new IntPoint(int.Parse(p1[0]), int.Parse(p1[1])), new IntPoint(int.Parse(p2[0]), int.Parse(p2[1])));
        }).ToArray();
        int seenAFter = BruteForceLines(lines);

        Console.WriteLine($"Part 2 Brute Force: {seenAFter}");
    }

    private static int BruteForceLines(IntLineSegment[] lines)
    {
        HashSet<IntPoint> seenFirst = new();
        HashSet<IntPoint> seenAFter = new();

        foreach (IntLineSegment line in lines)
        {
            IntSlope slope = line.GetSlope();

            IntPoint cur = line.P1;

            while (cur != line.P2)
            {
                if (!seenFirst.Add(cur))
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

        return seenAFter.Count;
    }

    private static void Part2()
    {
        IntLineSegment[] lines = File.ReadAllLines("./input.txt").Select(x => {
            var splt = x.Split(" -> ");
            var p1 = splt[0].Split(",");
            var p2 = splt[1].Split(",");

            return new IntLineSegment(new IntPoint(int.Parse(p1[0]), int.Parse(p1[1])), new IntPoint(int.Parse(p2[0]), int.Parse(p2[1])));
        }).ToArray();
        int overlappingPoints = CalculateOverlaps(lines);

        Console.WriteLine($"Part 2: {overlappingPoints}");
    }

    private static int CalculateOverlaps(IntLineSegment[] lines)
    {
        HashSet<IntPoint> overlappingPoints = new();

        for (int i = 0; i < lines.Length - 1; i++)
        {
            for (int j = i + 1; j < lines.Length; j++)
            {
                foreach (IntPoint o in lines[i].GetCommonPoints(lines[j]))
                {
                    overlappingPoints.Add(o);
                }
            }
        }

        return overlappingPoints.Count;
    }

    private static void Part1()
    {
        IntLineSegment[] lines = File.ReadAllLines("./input.txt").Select(x => {
            var splt = x.Split(" -> ");
            var p1 = splt[0].Split(",");
            var p2 = splt[1].Split(",");

            return new IntLineSegment(new IntPoint(int.Parse(p1[0]), int.Parse(p1[1])), new IntPoint(int.Parse(p2[0]), int.Parse(p2[1])));
        }).ToArray();

        HashSet<IntPoint> overlappingPoints = new();

        lines = lines.Where(l => l.P1.X == l.P2.X || l.P1.Y == l.P2.Y).ToArray();

        for (int i = 0; i < lines.Length - 1; i++)
        {
            for (int j = i + 1; j < lines.Length; j++)
            {
                foreach (IntPoint o in lines[i].GetCommonPoints(lines[j]))
                {
                    overlappingPoints.Add(o);
                }
            }
        }

        Console.WriteLine($"Part 1: {overlappingPoints.Count}");
    }
}
