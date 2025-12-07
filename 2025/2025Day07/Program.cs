
using System.Numerics;
using System.Runtime.InteropServices;

char[][] map = File.ReadAllLines("./input.txt")
    .Select(s => s.ToCharArray())
    .ToArray();

Console.WriteLine($"Part 1: {RunBeams(map, false)}");

Console.WriteLine($"Part 2: {RunBeams(map, true)}");

static long RunBeams(char[][] map, bool isPart2)
{
    Queue<Beam> beams = [];
    Dictionary<BeamLocation, Beam> foundBeams = [];

    List<Beam> doneBeams = [];
    int splitCount = 0;
    for (int x = 0; x < map[0].Length; x++)
    {
        if (map[0][x] == 'S')
        {
            var first = new Beam(x, 0, null);
            beams.Enqueue(first);
            break;
        }
    }

    while (beams.Count > 0)
    {
        Beam beam = beams.Dequeue();
        if (beam.Y == map.Length - 1)
        {
            doneBeams.Add(beam);
        }
        else if (map[beam.Y + 1][beam.X] == '.')
        {
            if(GetOrCreate(beam.X, beam.Y + 1, beam, foundBeams, out Beam result))
            {
                beams.Enqueue(result);
            }
        }
        else //this will be a splitter
        {
            if (GetOrCreate(beam.X + 1, beam.Y + 1, beam, foundBeams, out Beam right))
            {
                beams.Enqueue(right);
            }

            if (GetOrCreate(beam.X - 1, beam.Y + 1, beam, foundBeams, out Beam left))
            {
                beams.Enqueue(left);
            }

            splitCount++;
        }
    }

    long doneCount = 0;

    foreach(var beam in doneBeams)
    {
        doneCount += beam.Count;
    }

    if(isPart2)
    {
        return doneBeams.Sum(d => d.Count);
    }
    else
    {
        return splitCount;
    }  
}

static bool GetOrCreate(int X, int Y, Beam parent, Dictionary<BeamLocation, Beam> dict, out Beam result)
{
    var loc = new BeamLocation(X, Y);
    ref Beam? beam = ref CollectionsMarshal.GetValueRefOrAddDefault(dict, loc, out bool exists);

    if(beam == null)
    {
        beam = new Beam(X, Y, parent);
        result = beam;
    }
    else
    {
        beam.Count += parent.Count;
        result = beam;
    }

    return !exists;
}

internal readonly record struct BeamLocation(int X, int Y);

internal class Beam(int x, int y, Beam? parent)
{
    public int X { get; set; } = x;
    public int Y { get; set; } = y;
    public long Count { get; set; } = parent?.Count ?? 1;
    public Beam? Parent { get; set; } = parent;
}
