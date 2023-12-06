using Core;
using System.Collections.Frozen;
using System.Net;

namespace Day052023;

internal class Program
{
    static void Main(string[] args)
    {
        string[] lines = File.ReadAllLines("./input.txt");

        long[] seeds = lines[0]
            .Split(": ")[1]
            .Split(' ', StringSplitOptions.RemoveEmptyEntries)
            .Select(long.Parse)
            .ToArray();

        List<Map> maps = new List<Map>();
        ParseMap(lines, maps);

        Part1(seeds, maps);
        Part2Fast(seeds, maps);
    }

    private static void Part2Fast(long[] seeds, List<Map> maps)
    {
        var startingRanges = seeds.Chunk(2).Select(s => new Range<long>(s[0], s[0] + s[1] - 1)).ToArray();
        List<long> results = new();
        foreach (var range in startingRanges)
        {
            var runQueue = new Queue<Range<long>>();
            runQueue.Enqueue(range);
            var toAdd = new List<Range<long>>();
            foreach (var map in maps)
            {
                while (runQueue.Count > 0)
                {
                    var cur = runQueue.Dequeue();
                    toAdd.AddRange(map.Apply(cur));
                }
                foreach (var add in toAdd)
                {
                    runQueue.Enqueue(add);
                }
                toAdd.Clear();
            }

            while (runQueue.Count > 0)
            {
                results.Add(runQueue.Dequeue().Start);
            }
        }

        Console.WriteLine($"Part 2: {results.Min()}");
    }

    private static int ParseMap(string[] lines, List<Map> maps)
    {
        int i = 2;
        while (i < lines.Length)
        {
            string name = lines[i++].Trim(':');
            List<MapItem> items = new List<MapItem>();
            while (i < lines.Length && lines[i] != string.Empty)
            {
                var lineItems = lines[i++].Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(long.Parse).ToArray();
                items.Add(new MapItem(lineItems[1], lineItems[0], lineItems[2]));
            }
            i++;//skip blank line between maps

            maps.Add(new Map(name, items.ToFrozenSet()));
        }

        return i;
    }

    private static void Part1(long[] seeds, List<Map> maps)
    {
        long result = seeds.Select(s => {
            long cur = s;
            foreach (var map in maps)
            {
                cur = map.Apply(cur);
            }

            return cur;
        }).Min(s => s);

        Console.WriteLine($"Part 1: {result}");
    }

    private readonly record struct MapItem(long Source, long Destination, long Length);
    private readonly record struct Map(string Name, FrozenSet<MapItem> MapItems)
    {
        public IEnumerable<Range<long>> Apply(Range<long> r)
        {
            List<Range<long>> remainders = new();
            var rangeQueue = new Queue<Range<long>>();
            rangeQueue.Enqueue(r);
            while (rangeQueue.Count > 0)
            {
                var cur = rangeQueue.Dequeue();
                bool mapped = false;
                foreach (var item in MapItems)
                {
                    var itemRange = new Range<long>(item.Source, item.Source + item.Length - 1);
                    var overlap = cur.GetOverlap(itemRange);
                    if (overlap != null)
                    {
                        foreach(var other in cur.RemoveRange(overlap.Value))
                        {
                            rangeQueue.Enqueue(other);
                        }
                        yield return new Range<long>(overlap.Value.Start - item.Source + item.Destination, overlap.Value.End - item.Source + item.Destination);
                        mapped = true;
                        break;
                    }
                }
                if (!mapped)
                {
                    yield return cur;
                }
            }
        }

        public long Apply(long input)
        {
            foreach(var item in MapItems)
            {
                long distance = input - item.Source;
                if(distance >= 0 && distance <= item.Length)
                {
                    return item.Destination + distance;
                }
            }

            return input;
        }

        public decimal Apply(decimal input)
        {
            foreach (var item in MapItems)
            {
                decimal distance = input - item.Source;
                if (distance >= 0 && distance <= item.Length)
                {
                    return item.Destination + distance;
                }
            }

            return input;
        }
    }
}
