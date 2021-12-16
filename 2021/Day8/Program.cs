using System.Collections;

namespace Day8;

public static class Program
{
    public static void Main()
    {
        Part1();

        Dictionary<char, Segments> segments = new();

        List<string> outputs = new();

        IEnumerable<InputOutput> rows = File.ReadAllLines("./input.txt")
                    .Select(s => s.Split(" | "))
                    .Select(s => new InputOutput(s[0], s[1]));

        long result = rows.Sum(r => r.ComputeSegments());

        Console.WriteLine($"Part 2: {result}");
    }

    private record InputOutput(string input, string output)
    {
        private Dictionary<char, Segments> _segments = new();

        public long ComputeSegments()
        {
            string[] output = this.output.Split(" ");
            string[] digits = input.Split(" ").Concat(output).ToArray();
            GetPossibilitesFromLength(digits);

            bool isSolved = _segments.Values.All(s => CountBits(s) == 1);

            while (!_segments.Values.All(s => CountBits(s) == 1))
            {
                bool changed = EliminateViaExclusiveGroups();

                //finally if that doesn't work anymore resort to brute forcing the remaining possibilities
                if (!changed)
                {
                    foreach (var possiblity in EnumeratePossibilities(_segments))
                    {
                        if (digits.All(c => ConvertStringToDigit(c, possiblity) != -1))
                        {
                            _segments = possiblity;
                            isSolved = true;
                            break;
                        }
                    }

                    break;
                }
            }

            if (!isSolved)
            {
                throw new InvalidOperationException("problem");
            }

            int value = 0;
            for (int i = output.Length - 1; i >= 0; i--)
            {
                int digit = ConvertStringToDigit(output[i], _segments);
                value += digit * (int)Math.Pow(10, output.Length - 1 - i);
            }

            return value;
        }

        private bool EliminateViaExclusiveGroups()
        {
            bool changed = false;
            Dictionary<Segments, (int, List<char>)> flipped = new();

            foreach (var pair in _segments)
            {
                if (!flipped.TryGetValue(pair.Value, out (int, List<char>) val))
                {
                    val = (CountBits(pair.Value), new List<char> { pair.Key });
                    flipped[pair.Value] = val;
                }
                else
                {
                    val.Item2.Add(pair.Key);
                }
            }

            foreach (var pair in flipped)
            {
                if (pair.Value.Item1 == pair.Value.Item2.Count)
                {
                    Segments removemask = (Segments.Top | Segments.TopRight | Segments.TopLeft | Segments.Center | Segments.BottomRight | Segments.BottomLeft | Segments.Bottom) ^ pair.Key;
                    foreach (var pair2 in flipped.Where(f => f.Key != pair.Key))
                    {
                        foreach (char c in pair2.Value.Item2)
                        {
                            Segments updated = _segments[c] & removemask;

                            if (updated != _segments[c])
                            {
                                changed = true;
                                _segments[c] &= removemask;
                            }
                        }
                    }
                }
            }

            return changed;
        }

        private void GetPossibilitesFromLength(string[] digits)
        {
            foreach (string digit in digits)
            {
                Segments segments = GetPossibleSegmentsFromLength(digit.Length);
                foreach (char c in digit)
                {
                    if (!_segments.TryGetValue(c, out Segments seg))
                    {
                        _segments[c] = segments;
                    }
                    else
                    {
                        var reduced = seg & segments;
                        _segments[c] = reduced;
                    }
                }
            }
        }
    }

    private static int ConvertStringToDigit(string digit, Dictionary<char, Segments> segmentMap)
    {
        Segments segment = Segments.None;

        foreach (char c in digit)
        {
            segment |= segmentMap[c];
        }

        return ConvertSegmentsToNumber(segment);
    }

    private static int CountBits(Segments seg)
    {
        return new BitArray(new[] { (int)seg }).OfType<bool>().Count(c => c);
    }

    private static int ConvertSegmentsToNumber(Segments segments)
    {
        return segments switch {
            Segments.Top | Segments.TopRight | Segments.TopLeft | Segments.BottomRight | Segments.BottomLeft | Segments.Bottom => 0,
            Segments.TopRight | Segments.BottomRight => 1,
            Segments.Top | Segments.TopRight | Segments.Center | Segments.BottomLeft | Segments.Bottom => 2,
            Segments.Top | Segments.TopRight | Segments.Center | Segments.BottomRight | Segments.Bottom => 3,
            Segments.TopRight | Segments.TopLeft | Segments.Center | Segments.BottomRight => 4,
            Segments.Top | Segments.TopLeft | Segments.Center | Segments.BottomRight | Segments.Bottom => 5,
            Segments.Top | Segments.TopLeft | Segments.Center | Segments.BottomLeft | Segments.BottomRight | Segments.Bottom => 6,
            Segments.Top | Segments.TopRight | Segments.BottomRight => 7,
            Segments.Top | Segments.TopRight | Segments.TopLeft | Segments.Center | Segments.BottomLeft | Segments.BottomRight | Segments.Bottom => 8,
            Segments.Top | Segments.TopRight | Segments.TopLeft | Segments.Center | Segments.BottomRight | Segments.Bottom => 9,
            _ => -1
        };
    }

    private static Segments GetPossibleSegmentsFromLength(int length)
    {
        return length switch {
            6 => Segments.Top | Segments.TopLeft | Segments.TopRight | Segments.BottomRight | Segments.BottomLeft | Segments.Bottom | Segments.Center,
            2 => Segments.TopRight | Segments.BottomRight,
            5 => Segments.Top | Segments.TopRight | Segments.Center | Segments.BottomLeft | Segments.Bottom | Segments.BottomRight | Segments.TopLeft,
            4 => Segments.TopLeft | Segments.TopRight | Segments.Center | Segments.BottomRight,
            3 => Segments.Top | Segments.TopRight | Segments.BottomRight,
            7 => Segments.Top | Segments.TopRight | Segments.TopLeft | Segments.Center | Segments.BottomLeft | Segments.BottomRight | Segments.Bottom,
            _ => throw new ArgumentException("Count is not possible")
        };
    }

    private static IEnumerable<Dictionary<char, Segments>> EnumeratePossibilities(Dictionary<char, Segments> input)
    {
        return from a in EnumerateSegments(input['a'])
               from b in EnumerateSegments(input['b'])
               from c in EnumerateSegments(input['c'])
               from d in EnumerateSegments(input['d'])
               from e in EnumerateSegments(input['e'])
               from f in EnumerateSegments(input['f'])
               from g in EnumerateSegments(input['g'])
               where new[] { a, b, c, d, e, f, g }.Distinct().Count() == 7
               select new Dictionary<char, Segments> {
                   ['a'] = a,
                   ['b'] = b,
                   ['c'] = c,
                   ['d'] = d,
                   ['e'] = e,
                   ['f'] = f,
                   ['g'] = g
               };
    }

    private static IEnumerable<Segments> EnumerateSegments(Segments input)
    {
        foreach (Segments mask in Enum.GetValues<Segments>())
        {
            if ((int)(input & mask) > 0)
            {
                yield return mask;
            }
        }
    }

    [Flags]
    private enum Segments
    {
        None = 0x00,
        Top = 0x01,
        TopLeft = 0x02,
        TopRight = 0x04,
        Center = 0x08,
        BottomLeft = 0x10,
        BottomRight = 0x20,
        Bottom = 0x40,
    }

    private static void Part1()
    {
        int result = File.ReadAllLines("./input.txt")
                    .Select(s => s.Split(" | ")[1].Split(" "))
                    .SelectMany(s => s)
                    .Count(c => c.Length is 2 or 4 or 3 or 7);

        Console.WriteLine($"Part 1: {result}");
    }
}
