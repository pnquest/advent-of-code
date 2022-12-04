using System.Numerics;

namespace Day22;

public static class Program
{
    public static void Main()
    {
        HashSet<Vector3> on = new();

        string[] lines = File.ReadAllLines("./input.txt");

        Cube filter = new Cube { MinX = -50, MaxX = 50, MinY = -50, MaxY = 50, MinZ = -50, MaxZ = 50 };

        foreach (string lin in lines)
        {
            bool state = lin[..2] == "on";

            ReadOnlySpan<char> remain = lin.AsSpan(lin.IndexOf('=') + 1);

            int sepIndex = remain.IndexOf('.');
            int xMin = int.Parse(remain[..sepIndex]);
            remain = remain[(sepIndex + 2)..];

            sepIndex = remain.IndexOf(',');
            int xMax = int.Parse(remain[..sepIndex]);
            remain = remain[(sepIndex + 3)..];

            sepIndex = remain.IndexOf('.');
            int yMin = int.Parse(remain[..sepIndex]);
            remain = remain[(sepIndex + 2)..];

            sepIndex = remain.IndexOf(',');
            int yMax = int.Parse(remain[..sepIndex]);
            remain = remain[(sepIndex + 3)..];

            sepIndex = remain.IndexOf('.');
            int zMin = int.Parse(remain[..sepIndex]);
            remain = remain[(sepIndex + 2)..];

            int zMax = int.Parse(remain);

            Cube? cube = new() { State = state, MinX = xMin, MaxX = xMax, MinY = yMin, MaxY = yMax, MinZ = zMin, MaxZ = zMax };

            cube = filter.ComputeIntersectingCube(cube);

            if (cube == null)
            {
                continue;
            }

            for (int x = cube.MinX; x <= cube.MaxX; x++)
            {
                for (int y = cube.MinY; y <= cube.MaxY; y++)
                {
                    for (int z = cube.MinZ; z <= cube.MaxZ; z++)
                    {
                        Vector3 vec = new(x, y, z);
                        if (cube.State)
                        {
                            on.Add(vec);
                        }
                        else
                        {
                            on.Remove(vec);
                        }
                    }
                }
            }
        }

        Console.WriteLine($"Part 1 Slow: {on.Count}");
        Part1Fast();
    }

    private static void Part1Fast()
    {
        LinkedList<Cube> cubes = new();

        string[] lines = File.ReadAllLines("./input.txt");

        int i = 1;

        Cube filter = new Cube { MinX = -50, MaxX = 50, MinY = -50, MaxY = 50, MinZ = -50, MaxZ = 50 };

        foreach (string lin in lines)
        {
            bool state = lin[..2] == "on";

            ReadOnlySpan<char> remain = lin.AsSpan(lin.IndexOf('=') + 1);

            int sepIndex = remain.IndexOf('.');
            int xMin = int.Parse(remain[..sepIndex]);
            remain = remain[(sepIndex + 2)..];

            sepIndex = remain.IndexOf(',');
            int xMax = int.Parse(remain[..sepIndex]);
            remain = remain[(sepIndex + 3)..];

            sepIndex = remain.IndexOf('.');
            int yMin = int.Parse(remain[..sepIndex]);
            remain = remain[(sepIndex + 2)..];

            sepIndex = remain.IndexOf(',');
            int yMax = int.Parse(remain[..sepIndex]);
            remain = remain[(sepIndex + 3)..];

            sepIndex = remain.IndexOf('.');
            int zMin = int.Parse(remain[..sepIndex]);
            remain = remain[(sepIndex + 2)..];

            int zMax = int.Parse(remain);

            Cube? cube = new() { State = state, MinX = xMin, MaxX = xMax, MinY = yMin, MaxY = yMax, MinZ = zMin, MaxZ = zMax };

            cube = filter.ComputeIntersectingCube(cube);

            Console.WriteLine($"---------- Step {i++} -----------");

            if (cube == null)
            {
                continue;
            }

            if (cube.State)
            {
                LinkedList<Cube> remaining = new();
                remaining.AddLast(cube);
                LinkedListNode<Cube>? cur = cubes.First;
                LinkedListNode<Cube>? rem = remaining.First;

                while (cur != null)
                {
                    while (rem != null)
                    {
                        Cube? intersect;
                        if (cur != null && cur.Value.DoesCompletelyContain(rem.Value))
                        {
                            LinkedListNode<Cube> tmpRem = rem;
                            rem = rem.Next;
                            remaining.Remove(tmpRem);
                        }
                        else if (cur != null && rem.Value.DoesCompletelyContain(cur.Value))
                        {
                            LinkedListNode<Cube> tmpCur = cur;
                            cur = cur.Next;
                            cubes.Remove(tmpCur);
                        }
                        else if (cur != null && (intersect = cur.Value.ComputeIntersectingCube(rem.Value)) != null)
                        {
                            foreach (Cube newRem in rem.Value.RemoveOverlap(intersect))
                            {
                                remaining.AddAfter(rem, newRem);
                            }
                            LinkedListNode<Cube> tmpRem = rem;
                            rem = rem.Next;
                            remaining.Remove(tmpRem);
                        }
                        else
                        {
                            rem = rem.Next;
                        }
                    }
                    cur = cur?.Next;
                    rem = remaining.First;
                }

                foreach (Cube r in remaining)
                {
                    cubes.AddLast(r);
                    Console.WriteLine($"Adding {r}");
                }
            }
            else
            {
                LinkedListNode<Cube>? cur = cubes.First;

                while (cur != null)
                {
                    if (cube.DoesCompletelyContain(cur.Value))
                    {
                        LinkedListNode<Cube> temp = cur;
                        cur = cur.Next;
                        cubes.Remove(temp);
                    }
                    else
                    {
                        foreach (Cube rem in cur.Value.RemoveOverlap(cube))
                        {
                            cubes.AddBefore(cur, rem);
                        }

                        LinkedListNode<Cube> temp = cur;
                        cur = cur.Next;
                        cubes.Remove(temp);
                    }
                }
            }
        }

        long result = cubes.Sum(c => c.ComputeArea());

        Console.WriteLine($"Part 1: {result}");
    }
}

internal class Cube
{
    public bool State { get; init; }
    public int MinX { get; init; }
    public int MaxX { get; init; }
    public int MinY { get; init; }
    public int MaxY { get; init; }
    public int MinZ { get; init; }
    public int MaxZ { get; init; }

    public long ComputeArea()
    {
        return (long)(Math.Abs(MaxX - MinX) + 1) * (Math.Abs(MaxY - MinY) + 1) * (Math.Abs(MaxZ - MinZ) + 1);
    }

    public bool DoesCompletelyContain(Cube other)
    {
        return MinX <= other.MinX
            && MaxX >= other.MaxX
            && MinY <= other.MinY
            && MaxY >= other.MaxY
            && MinZ <= other.MinZ
            && MaxZ >= other.MaxZ;
    }

    public IEnumerable<Cube> RemoveOverlap(Cube toRemove)
    {
        Cube? intersect = ComputeIntersectingCube(toRemove);
        if (intersect == null)
        {
            yield return this;
            yield break;
        }

        int tmpMinX = MinX;
        int tmpMaxX = MaxX;
        int tmpMinY = MinY;
        int tmpMaxY = MaxY;
        int tmpMinZ = MinZ;
        int tmpMaxZ = MaxZ;

        if (intersect.MinX > tmpMinX)
        {
            yield return new Cube { State = State, MinX = tmpMinX, MaxX = intersect.MinX - 1, MinY = tmpMinY, MaxY = tmpMaxY, MaxZ = tmpMaxZ, MinZ = tmpMinZ };
            tmpMinX = intersect.MinX;
        }

        if (intersect.MaxX < tmpMaxX)
        {
            yield return new Cube { State = State, MinX = intersect.MaxX + 1, MaxX = tmpMaxX, MinY = tmpMinY, MaxY = tmpMaxY, MinZ = tmpMinZ, MaxZ = tmpMaxZ };
            tmpMaxX = intersect.MaxX;
        }

        if (intersect.MinY > tmpMinY)
        {
            yield return new Cube { State = State, MinX = tmpMinX, MaxX = tmpMaxX, MinY = tmpMinY, MaxY = intersect.MinY - 1, MaxZ = tmpMaxZ, MinZ = tmpMinZ };
            tmpMinY = intersect.MinY;
        }

        if (intersect.MaxY < tmpMaxY)
        {
            yield return new Cube { State = State, MinX = tmpMinX, MaxX = tmpMaxX, MinY = intersect.MaxY + 1, MaxY = tmpMaxY, MinZ = tmpMinZ, MaxZ = tmpMaxZ };
            tmpMaxY = intersect.MaxY;
        }

        if (intersect.MinZ > tmpMinZ)
        {
            yield return new Cube { State = State, MinX = tmpMinX, MaxX = tmpMaxX, MinY = tmpMinY, MaxY = tmpMaxY, MaxZ = intersect.MinZ - 1, MinZ = tmpMinZ };
        }
        if (intersect.MaxZ < tmpMaxZ)
        {
            yield return new Cube { State = State, MinX = tmpMinX, MaxX = tmpMaxX, MinY = tmpMinY, MaxY = tmpMaxY, MinZ = intersect.MaxZ + 1, MaxZ = tmpMaxZ };
        }
    }

    public Cube? ComputeIntersectingCube(Cube? other)
    {
        if (other == null)
        {
            return null;
        }

        //cubes have no overlap
        if (MinX > other.MaxX || MaxX < other.MinX || MinY > other.MaxY || MaxY < other.MinY || MinZ > other.MaxZ || MaxZ < other.MinZ)
        {
            return null;
        }

        int overlapMinX = Math.Max(MinX, other.MinX);
        int overlapMaxX = Math.Min(MaxX, other.MaxX);
        int overlapMinY = Math.Max(MinY, other.MinY);
        int overlapMaxY = Math.Min(MaxY, other.MaxY);
        int overlapMinZ = Math.Max(MinZ, other.MinZ);
        int overlapMaxZ = Math.Min(MaxZ, other.MaxZ);

        return new Cube {
            State = other.State,
            MinX = overlapMinX,
            MaxX = overlapMaxX,
            MinY = overlapMinY,
            MaxY = overlapMaxY,
            MinZ = overlapMinZ,
            MaxZ = overlapMaxZ
        };
    }

    public override string ToString()
    {
        return $"{MinX}..{MaxX}, {MinY}..{MaxY}, {MinZ}..{MaxZ}";
    }
}
