using System.Numerics;

namespace Day19;

public static class Program
{
    public static void Main()
    {
        List<Scanner> scanners = GetScanners();

        BuildMap(scanners);

        Part1(scanners);
        Part2(scanners);
    }

    private static void BuildMap(List<Scanner> scanners)
    {
        scanners[0] = scanners[0].PinAt(0, 0, 0);

        Stack<Scanner> pinned = new();
        pinned.Push(scanners[0]);

        while (pinned.Count > 0 && scanners.Any(s => !s.IsPinned))
        {
            Scanner curPinned = pinned.Pop();
            Enumerable.Range(0, scanners.Count)
                .AsParallel()
                .ForAll(i => MatchOrientation(scanners, pinned, curPinned, i));
        }
    }

    private static void Part2(List<Scanner> scanners)
    {
        long maxDistance = 0;
        for (int i = 0; i < scanners.Count - 1; i++)
        {
            for (int j = i + 1; j < scanners.Count; j++)
            {
                Scanner left = scanners[i];
                Scanner right = scanners[j];

                long distance = GetManhattanDistance(left.ScannerPosition, right.ScannerPosition);

                if (maxDistance < distance)
                {
                    maxDistance = distance;
                }
            }
        }

        Console.WriteLine($"Part 2: {maxDistance}");
    }

    private static void Part1(List<Scanner> scanners)
    {
        int count = scanners.SelectMany(p => p.Beacons).Distinct().Count();
        Console.WriteLine($"Part 1: {count}");
    }

    private static void MatchOrientation(List<Scanner> scanners, Stack<Scanner> pinned, Scanner curPinned, int i)
    {
        bool isFound = false;
        if (!scanners[i].IsPinned)
        {
            foreach (Scanner orientation in scanners[i].IterateOrientations())
            {
                HashSet<Vector3> adjustments = new();
                foreach (Vector3 p in curPinned.Beacons)
                {
                    foreach (Vector3 o in orientation.Beacons)
                    {
                        Vector3 adjustment = p - o;
                        if (adjustments.Add(adjustment))
                        {
                            Scanner adjusted = orientation.Shift((int)adjustment.X, (int)adjustment.Y, (int)adjustment.Z);
                            int overlap = curPinned.CountOverlap(adjusted);
                            if (overlap >= 12)
                            {
                                Scanner? pin = adjusted.PinAt((int)adjustment.X, (int)adjustment.Y, (int)adjustment.Z);
                                scanners[i] = pin;
                                pinned.Push(pin);
                                isFound = true;
                                break;
                            }
                        }
                    }

                    if (isFound)
                    {
                        break;
                    }
                }

                if (isFound)
                {
                    break;
                }
            }
        }
    }

    public static long GetManhattanDistance(Vector3 left, Vector3 right)
    {
        return (long)(Math.Abs(left.X - right.X) + Math.Abs(left.Y - right.Y) + Math.Abs(left.Z - right.Z));
    }

    private static List<Scanner> GetScanners()
    {
        string[] lines = File.ReadAllLines("./input.txt");

        List<Scanner> scanners = new();

        List<Vector3>? cur = null;
        foreach (string lin in lines)
        {
            if (lin.StartsWith("---"))
            {
                if (cur != null)
                {
                    scanners.Add(new Scanner(cur));
                }

                cur = new();
            }
            else if (string.IsNullOrEmpty(lin))
            {
                continue;
            }
            else
            {
                int commaIndex = lin.IndexOf(',');
                int x = int.Parse(lin[..commaIndex]);
                var nxt = lin[(commaIndex + 1)..];
                commaIndex = nxt.IndexOf(',');
                int y = int.Parse(nxt[..commaIndex]);
                nxt = nxt[(commaIndex + 1)..];
                int z = int.Parse(nxt);
                cur?.Add(new Vector3(x, y, z));
            }
        }
        if(cur != null)
        {
            scanners.Add(new Scanner(cur));
        }
        
        return scanners;
    }
}

internal record Scanner(List<Vector3> Beacons, Vector3 ScannerPosition = new Vector3(), bool IsPinned = false)
{
    public Scanner PinAt(int x, int y, int z)
    {
        return this with { ScannerPosition = new Vector3(x, y, z), IsPinned = true };
    }

    public int CountOverlap(Scanner other)
    {
        return Beacons.Intersect(other.Beacons).Count();
    }

    public Scanner Shift(int x, int y, int z)
    {
        var transform = Matrix4x4.CreateTranslation(x, y, z);
        return this with { Beacons = Beacons.Select(b => Vector3.Transform(b, transform)).ToList() };
    }

    public IEnumerable<Scanner> IterateOrientations()
    {
        int[] angles = new int[] { 0, 90, 180, 270 };

        foreach(int x in angles)
        {
            Quaternion rotX = Quaternion.CreateFromAxisAngle(Vector3.UnitX, x * (float)Math.PI / 180);
            foreach(int y in angles)
            {
                Quaternion rotY = rotX * Quaternion.CreateFromAxisAngle(Vector3.UnitY, y * (float)Math.PI / 180);
                foreach (int z in angles)
                {
                    Quaternion rotZ = rotY * Quaternion.CreateFromAxisAngle(Vector3.UnitZ, z * (float)Math.PI / 180);
                    yield return this with {
                        Beacons = Beacons
                            .Select(z => Vector3.Transform(z, rotZ))
                            .Select(x => new Vector3(MathF.Round(x.X, 0, MidpointRounding.AwayFromZero), MathF.Round(x.Y, 0, MidpointRounding.AwayFromZero), MathF.Round(x.Z, 0, MidpointRounding.AwayFromZero)))
                            .ToList()
                    };
                }
            }
        }
    }
}