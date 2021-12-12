namespace Day12;

public static class Program
{
    public static void Main()
    {
        Dictionary<string, Cave> caveMap = BuildCaves();
        Part1(caveMap);
        Part2(caveMap);
    }

    private static void Part1(Dictionary<string, Cave> caveMap)
    {
        List<Path> endPaths = ComputePaths(caveMap, false);

        Console.WriteLine($"Part 1: {endPaths.Count}");
    }

    private static void Part2(Dictionary<string, Cave> caveMap)
    {
        List<Path> endPaths = ComputePaths(caveMap, true);

        Console.WriteLine($"Part 2: {endPaths.Count}");
    }

    private static List<Path> ComputePaths(Dictionary<string, Cave> caveMap, bool allowDuplicates)
    {
        Cave start = caveMap["start"];
        Stack<Path> paths = new();
        List<Path> endPaths = new();

        Path? startPath = new(new(), new(), AllowDuplicate: allowDuplicates);
        (_, startPath) = startPath.TryAddCave(start);
        if(startPath != null)
        {
            paths.Push(startPath);
        }
        

        while (paths.Count > 0)
        {
            Path curPath = paths.Pop();
            Cave lastCave = curPath.GetLastCave();

            foreach (Cave nextCave in lastCave.Connections)
            {
                var (result, cloned) = curPath.TryAddCave(nextCave);
                switch (result)
                {
                    case Path.AddResult.Success when cloned is not null:
                        paths.Push(cloned);
                        break;

                    case Path.AddResult.IsEnd when cloned is not null:
                        endPaths.Add(cloned);
                        break;

                    case Path.AddResult.DuplicateSmallCave:
                        break; // do nothing on purpose
                }
            }
        }

        return endPaths;
    }

    private static Dictionary<string, Cave> BuildCaves()
    {
        Dictionary<string, Cave> caveMap = new();

        foreach (string line in File.ReadLines("./input.txt"))
        {
            string[] connections = line.Split('-');
            if (!caveMap.TryGetValue(connections[0], out Cave? left))
            {
                left = new(connections[0]);
                caveMap[connections[0]] = left;
            }

            if (!caveMap.TryGetValue(connections[1], out Cave? right))
            {
                right = new(connections[1]);
                caveMap.Add(connections[1], right);
            }

            left.Connections.Add(right);
            right.Connections.Add(left);
        }

        return caveMap;
    }
}

internal record Path(HashSet<Cave> SmallCavesVisited, List<Cave> PathCaves, bool FirstSmallDuplicateExhausted = false, bool AllowDuplicate = false)
{
    public Cave GetLastCave()
    {
        return PathCaves[^1];
    }

    public (AddResult, Path?) TryAddCave(Cave cave)
    {
        if(!cave.IsSmall || !SmallCavesVisited.Contains(cave))
        {
            return AddCave(cave, false);
        }
        else if(AllowDuplicate && !cave.IsEnd && !cave.IsStart && !FirstSmallDuplicateExhausted)
        {
            return AddCave(cave, true);
        }

        return (AddResult.DuplicateSmallCave, null);
    }

    private (AddResult, Path) AddCave(Cave cave, bool duplicateExhausted)
    {
        var pathCaves = PathCaves.ToList();
        pathCaves.Add(cave);
        var smallVisited = SmallCavesVisited.ToHashSet();
        if(cave.IsSmall)
        {
            smallVisited.Add(cave);
        }
        Path cloned;
        if(duplicateExhausted)
        {
            cloned = this with { PathCaves = pathCaves, SmallCavesVisited = smallVisited, FirstSmallDuplicateExhausted = true };
        }
        else
        {
            cloned = this with { PathCaves = pathCaves, SmallCavesVisited = smallVisited };
        }

        AddResult result = cave.IsEnd ? AddResult.IsEnd : AddResult.Success;

        return (result, cloned);
    }

    public override string ToString()
    {
        return string.Join(',', PathCaves);
    }

    public enum AddResult
    {
        Success,
        IsEnd,
        DuplicateSmallCave
    }
}

internal class Cave
{
    public string Name { get; }
    public bool IsSmall => char.IsLower(Name[0]);
    public bool IsEnd => Name == "end";
    public bool IsStart => Name == "start";
    public List<Cave> Connections { get; } = new();

    public Cave(string name)
    {
        Name = name;
    }

    public override bool Equals(object? obj)
    {
        return obj is Cave cave &&
               Name == cave.Name;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Name);
    }

    public override string? ToString()
    {
        return Name;
    }
}