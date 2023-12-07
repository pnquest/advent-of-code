using Core;

namespace Day11;

public static class Program
{
    public static void Main()
    {
        Part1();
        Part2();
    }

    private static void Part2()
    {
        Octopus[][] input = File.ReadAllLines("./input.txt")
                    .Select((l, y) => l.Select((c, x) => new Octopus(c - '0', new IntPoint(x, y))).ToArray())
                    .ToArray();

        long curFlashes = 0;
        long counter = 0;
        while (curFlashes < 100)
        {
            counter++;
            curFlashes = 0;
            HashSet<Octopus> flashed = [];
            Stack<Octopus> toFlash = new();
            RaiseAllLevels(input, toFlash);
            curFlashes = HandleFlashes(input, 0, flashed, toFlash);
            ResetFlashed(flashed);
        }

        Console.WriteLine($"Part 2: {counter}");
    }

    private static void Part1()
    {
        Octopus[][] input = File.ReadAllLines("./input.txt")
            .Select((l, y) => l.Select((c, x) => new Octopus(c - '0', new IntPoint(x, y))).ToArray())
            .ToArray();

        long totalFlashes = 0;

        for (int i = 0; i < 100; i++)
        {
            HashSet<Octopus> flashed = [];
            Stack<Octopus> toFlash = new();
            RaiseAllLevels(input, toFlash);
            totalFlashes = HandleFlashes(input, totalFlashes, flashed, toFlash);
            ResetFlashed(flashed);
        }

        Console.WriteLine($"Part 1: {totalFlashes}");
    }

    private static void ResetFlashed(HashSet<Octopus> flashed)
    {
        foreach (Octopus hasFlashed in flashed)
        {
            hasFlashed.ResetLevel();
        }
    }

    private static long HandleFlashes(Octopus[][] input, long totalFlashes, HashSet<Octopus> flashed, Stack<Octopus> toFlash)
    {
        while (toFlash.Count > 0)
        {
            Octopus curFlash = toFlash.Pop();
            if (flashed.Add(curFlash))
            {
                totalFlashes++;
                foreach (IntPoint pt in curFlash.Neighbors)
                {
                    Octopus neighbor = input[pt.Y][pt.X];
                    if (neighbor.RaiseLevel())
                    {
                        toFlash.Push(input[pt.Y][pt.X]);
                    }
                }
            }
        }

        return totalFlashes;
    }

    private static void RaiseAllLevels(Octopus[][] input, Stack<Octopus> toFlash)
    {
        foreach (Octopus[] row in input)
        {
            foreach (Octopus octo in row)
            {
                if (octo.RaiseLevel())
                {
                    toFlash.Push(octo);
                }
            }
        }
    }
}

internal class Octopus
{
    public int Level { get; private set; }
    public IntPoint Position { get; }
    private Lazy<List<IntPoint>> _neighbors;
    public List<IntPoint> Neighbors => _neighbors.Value;

    public Octopus(int level, IntPoint position)
    {
        Level = level;
        Position = position;
        _neighbors = new(() => GetNeighbors());
    }

    public bool RaiseLevel()
    {
        int oldLevel = Level++;
        return oldLevel <= 9 && Level > 9;
    }

    public void ResetLevel()
    {
        Level = 0;
    }

    private List<IntPoint> GetNeighbors()
    {
        List<IntPoint> neighbors = new(8);

        Span<IntSlope> slopes = stackalloc IntSlope[] {
            new IntSlope(-1, -1),
            new IntSlope(-1, 0),
            new IntSlope(-1, 1),
            new IntSlope(0, -1),
            new IntSlope(0, 1),
            new IntSlope(1, -1),
            new IntSlope(1, 0),
            new IntSlope(1, 1)
        };

        foreach (IntSlope slope in slopes)
        {
            IntPoint potential = Position + slope;

            if (potential.X >= 0 && potential.X < 10 && potential.Y >= 0 && potential.Y < 10)
            {
                neighbors.Add(potential);
            }
        }

        return neighbors;
    }

    public override bool Equals(object? obj)
    {
        return obj is Octopus octopus &&
               Position.Equals(octopus.Position);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Position);
    }
}
