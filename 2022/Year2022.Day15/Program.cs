using Core;
using Pidgin;
using System.Collections.Generic;

namespace Year2022.Day15;

internal class Program
{
    static void Main(string[] args)
    {
        Part1();

        Dictionary<IntPoint, Position> sensors = GetData();

        (Position s, IntRectangle)[] sensorRanges = sensors.Values
            .Where(s => s.Type == Position.PositionType.Sensor)
            .Select(s => (s, ConvertSensorToRectangle(s)))
            .ToArray();

        HashSet<IntPoint> existingBeacons = sensors.Values
            .Where(s => s.Type == Position.PositionType.Sensor && s.NearestBeacon.HasValue)
            .Select(s => s.NearestBeacon!.Value)
            .ToHashSet();

        IntPoint solution = sensorRanges
            .SelectMany(s => TraceOuterEdge(s.s, s.Item2))
            .Where(p => p.X >= 0 && p.X <= 4000000 && p.Y >= 0 && p.Y <= 4000000)
            .Skip(10)
            .First(p => !existingBeacons.Contains(p) && !sensors.ContainsKey(p) && sensorRanges.All(s => !s.Item2.ContainsPoint(p)));

        long result = (long)solution.X * 4000000L + solution.Y;
        Console.WriteLine($"Part 2: {result}");
    }

    private static IEnumerable<IntPoint> TraceOuterEdge(Position s, IntRectangle rect)
    {
        var biggerRect = new IntRectangle(rect.TopLeft with { Y = rect.TopLeft.Y - 1 },
            rect.TopRight with { X = rect.TopRight.X + 1 },
            rect.BottomLeft with { X = rect.BottomLeft.X - 1 },
            rect.BottomRight with { Y = rect.BottomRight.Y + 1 });

        foreach(IntPoint p in biggerRect.IterateBorder()) 
        {
            yield return p;
        }
    }

    private static IntRectangle ConvertSensorToRectangle(Position p)
    {
        int distance = p.Coordinates.CalculateManhattenDistanceTo(p.NearestBeacon!.Value);

        return new IntRectangle(p.Coordinates with { Y = p.Coordinates.Y - distance },
            p.Coordinates with { X = p.Coordinates.X + distance },
            p.Coordinates with { X = p.Coordinates.X - distance },
            p.Coordinates with { Y = p.Coordinates.Y + distance });
    }

    private static void Part1()
    {
        Dictionary<IntPoint, Position> sensors = GetData();

        int result = CountNonBeaconsInRow(sensors, 2000000);
        Console.WriteLine($"Part 1: {result}");
    }

    private static Dictionary<IntPoint, Position> GetData()
    {
        Parser<char, int> coordinateParser = Parser.String("x=").Or(Parser.String("y=")).Then(Parser.Num);
        var coordinatePairParser = Parser.Map((x, _, y) => new IntPoint(x, y), coordinateParser, Parser.String(", "), coordinateParser);

        Parser<char, Position> fullParser = Parser.String("Sensor at ").Then(Parser.Map((s, b) => new Position {
            Coordinates = s,
            NearestBeacon = b,
            Type = Position.PositionType.Sensor
        }, coordinatePairParser.Before(Parser.String(": closest beacon is at ")), coordinatePairParser));


        Dictionary<IntPoint, Position> sensors = File.ReadAllLines("./input.txt")
            .Select(l => fullParser.ParseOrThrow(l))
            .ToDictionary(p => p.Coordinates);

        Dictionary<IntPoint, Position> toAdd = [];

        foreach (Position p in sensors.Values)
        {
            if (p.NearestBeacon.HasValue && !toAdd.ContainsKey(p.NearestBeacon.Value))
            {
                toAdd[p.NearestBeacon.Value] = new Position {
                    Coordinates = p.NearestBeacon.Value,
                    Type = Position.PositionType.Beacon
                };
            }
        }

        foreach (KeyValuePair<IntPoint, Position> pair in toAdd)
        {
            sensors.Add(pair.Key, pair.Value);
        }

        return sensors;
    }

    private static int CountNonBeaconsInRow(Dictionary<IntPoint, Position> sensors, int searchY)
    {
        HashSet<IntPoint> result = [];
        foreach (Position sensor in sensors.Values.Where(s => s.Type == Position.PositionType.Sensor))
        {
            int distance = sensor.Coordinates.CalculateManhattenDistanceTo(sensor.NearestBeacon!.Value);

            int startY = sensor.Coordinates.Y - distance;
            int endY = sensor.Coordinates.Y + distance;

            if (startY <= searchY && endY >= searchY)
            {
                int remainingDistance = distance - Math.Abs(sensor.Coordinates.Y - searchY);

                for (int x = sensor.Coordinates.X - remainingDistance; x <= sensor.Coordinates.X + remainingDistance; x++)
                {
                    var curPoint = new IntPoint(x, searchY);
                    if (!sensors.TryGetValue(curPoint, out Position? value) || value.Type == Position.PositionType.Sensor)
                    {
                        result.Add(curPoint);
                    }
                }
            }
        }

        return result.Count;
    }

    private static void MapUnknowns(Dictionary<IntPoint, Position> sensors)
    {
        Dictionary<IntPoint, Position> toAdd = [];

        foreach (Position sensor in sensors.Values.Where(s => s.Type == Position.PositionType.Sensor))
        {
            int distance = sensor.Coordinates.CalculateManhattenDistanceTo(sensor.NearestBeacon!.Value);

            int startY = sensor.Coordinates.Y - distance;
            int endY = sensor.Coordinates.Y + distance;

            for (int y = startY; y <= endY; y++)
            {
                int remainingDistance = distance - Math.Abs(sensor.Coordinates.Y - y);

                for (int x = sensor.Coordinates.X - remainingDistance; x <= sensor.Coordinates.X + remainingDistance; x++)
                {
                    var curPoint = new IntPoint(x, y);
                    if(!sensors.ContainsKey(curPoint))
                    {
                        toAdd[curPoint] = new Position {
                            Coordinates = curPoint,
                            Type = Position.PositionType.Empty
                        };
                    }
                }
            }
        }

        foreach(KeyValuePair<IntPoint, Position> p in toAdd)
        {
            sensors.Add(p.Key, p.Value);
        }
    }

    private static Position[][] BuildGrid(int maxX, int maxY, Dictionary<IntPoint, Position> sensors, HashSet<IntPoint> beacons)
    {
        Position[][] grid = new Position[maxY + 1][];

        for (int y = 0; y < grid.Length; y++)
        {
            grid[y] = new Position[maxX + 1];
            for (int x = 0; x < grid[y].Length; x++)
            {
                var curPoint = new IntPoint(x, y);
                if (sensors.TryGetValue(curPoint, out Position? s))
                {
                    grid[y][x] = s;
                }
                else if (beacons.Contains(curPoint))
                {
                    grid[y][x] = new Position {
                        Coordinates = curPoint,
                        Type = Position.PositionType.Beacon
                    };
                }
                else
                {
                    grid[y][x] = new Position {
                        Coordinates = curPoint,
                        Type = Position.PositionType.Unknown
                    };
                }
            }
        }

        return grid;
    }

    public class Position
    {
        public required IntPoint Coordinates { get; init; }
        public required PositionType Type {get; set;}
        public IntPoint? NearestBeacon { get; init; }

        public enum PositionType
        {
            Unknown,
            Empty,
            Beacon,
            Sensor
        }
    }
}
