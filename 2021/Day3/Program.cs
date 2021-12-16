// See https://aka.ms/new-console-template for more information

namespace Day3;

public static class Program
{
    public static void Main()
    {
        int length = 0;
        uint[] values = File.ReadAllLines("./input.txt").Select(s => {
            length = s.Length;
            return Convert.ToUInt32(s, 2);
        }).ToArray();
        Part1(values, length);
        Part2(values, length);
    }

    private static void Part2(uint[] values, int length)
    {
        List<uint> o2List = values.ToList();
        List<uint> cO2List = values.ToList();
        uint o2Rating = GetRating(o2List, true, length);
        uint co2Rating = GetRating(cO2List, false, length);

        Console.WriteLine($"Part 2: {o2Rating * co2Rating}");
    }

    private static uint GetRating(List<uint> list, bool mostCommon, int length)
    {
        for (int i = length - 1; i >= 0; i--)
        {
            if (list.Count > 1)
            {
                BitCounter counter = new(i);
                foreach (uint value in list)
                {
                    counter.CountValue(value);
                }

                uint toKeep;

                if (mostCommon)
                {
                    if (counter.Ones >= counter.Zeros)
                    {
                        toKeep = counter.Mask;
                    }
                    else
                    {
                        toKeep = 0;
                    }
                }
                else
                {
                    if (counter.Ones < counter.Zeros)
                    {
                        toKeep = counter.Mask;
                    }
                    else
                    {
                        toKeep = 0;
                    }
                }

                list.RemoveAll(v => (v & counter.Mask) != toKeep);
            }
            else
            {
                return list.Single();
            }
        }

        return list.Single();
    }

    private static void Part1(uint[] values, int length)
    {
        uint mask = Convert.ToUInt32(new string(Enumerable.Repeat('1', length).ToArray()), 2);

        Dictionary<int, BitCounter> counters = new(12);

        foreach (uint value in values)
        {
            for (int i = 0; i < length; i++)
            {
                if (!counters.TryGetValue(i, out BitCounter? counter))
                {
                    counter = new BitCounter(i);
                    counters[i] = counter;
                }

                counter.CountValue(value);
            }
        }

        uint gammaRate = 0;

        for (int i = 0; i < length; i++)
        {
            gammaRate += counters[i].OutputMostCommon();
        }

        uint epsilonRate = ~gammaRate & mask;

        Console.WriteLine($"Part 1: {gammaRate * epsilonRate}");
    }

    internal class BitCounter
    {
        public int Offset { get; }
        public uint Mask { get; }

        public uint Ones { get; private set; } = 0;
        public uint Zeros { get; private set; } = 0;

        public BitCounter(int offset)
        {
            Offset = offset;
            Mask = (uint)1 << offset;
        }

        public void CountValue(uint value)
        {
            if (GetMaskedResult(value) != 0)
            {
                Ones++;
            }
            else
            {
                Zeros++;
            }
        }

        public uint GetMaskedResult(uint value)
        {
            return value & Mask;
        }

        public uint OutputMostCommon()
        {
            return Ones >= Zeros
                ? Mask
                : 0;
        }
    }
}
