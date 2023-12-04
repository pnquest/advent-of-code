using Core;
using System.Collections.Frozen;

namespace Day032023;

internal class Program
{
    static void Main(string[] args)
    {
        List<Number> numbers = new List<Number>();
        Dictionary<IntPoint, char> Symbols = new Dictionary<IntPoint, char>();
        ParseMap(numbers, Symbols);
        Part1(numbers, Symbols);
        Part2(numbers, Symbols);
    }

    private static void Part2(List<Number> numbers, Dictionary<IntPoint, char> Symbols)
    {
        FrozenDictionary<IntPoint, Number> numberLocations = numbers.SelectMany(n => n.Points, (n, p) => (n, p)).ToFrozenDictionary(d => d.p, d => d.n);

        long totalRatio = 0;
        foreach (IntPoint symbolPoint in Symbols.Where(s => s.Value == '*').Select(s => s.Key))
        {
            List<Number> neighbors = new List<Number>();

            foreach (var neighbor in symbolPoint.GetNeighbors(int.MinValue, int.MaxValue, int.MinValue, int.MaxValue, true))
            {
                if (numberLocations.TryGetValue(neighbor, out Number? val) && !neighbors.Contains(val))
                {
                    neighbors.Add(val);
                }
            }

            if (neighbors.Count == 2)
            {
                totalRatio += neighbors.Aggregate(1L, (a, n) => a * n.Value);
            }
        }

        Console.WriteLine($"Part 2: {totalRatio}");
    }

    private static void Part1(List<Number> numbers, Dictionary<IntPoint, char> Symbols)
    {
        long total = 0;

        foreach (Number number in numbers)
        {
            bool found = false;
            foreach (IntPoint point in number.Points)
            {
                if (found)
                {
                    break;
                }
                foreach (IntPoint neighbor in point.GetNeighbors(int.MinValue, int.MaxValue, int.MinValue, int.MaxValue, true))
                {
                    if (Symbols.ContainsKey(neighbor))
                    {
                        total += number.Value;
                        found = true;
                        break;
                    }
                }
            }
        }

        Console.WriteLine($"Part 1: {total}");
    }

    private static void ParseMap(List<Number> numbers, Dictionary<IntPoint, char> Symbols)
    {
        foreach (var line in File.ReadAllLines("./input.txt").Select((l, i) => (l, i)))
        {
            bool isInNumber = false;
            List<int> digits = new();
            List<IntPoint> points = new();
            for (int i = 0; i < line.l.Length; i++)
            {
                if (char.IsDigit(line.l[i]))
                {
                    isInNumber = true;
                    digits.Add(line.l[i] - '0');
                    points.Add(new IntPoint(i, line.i));
                }
                else if (isInNumber)
                {
                    isInNumber = false;
                    BuildNumber(numbers, digits, points);
                }

                if (!char.IsDigit(line.l[i]) && line.l[i] != '.')
                {
                    Symbols[new IntPoint(i, line.i)] = line.l[i];
                }
            }

            if(isInNumber)
            {
                isInNumber = false;
                BuildNumber(numbers, digits, points);
            }
        }
    }

    private static void BuildNumber(List<Number> numbers, List<int> digits, List<IntPoint> points)
    {
        int num = 0;
        for (int j = 0; j < digits.Count; j++)
        {
            num += digits[j] * (int)Math.Pow(10, digits.Count - 1 - j);
        }

        numbers.Add(new Number(num, points.ToFrozenSet()));
        digits.Clear();
        points.Clear();
    }

    private record Number(long Value, FrozenSet<IntPoint> Points);
}
