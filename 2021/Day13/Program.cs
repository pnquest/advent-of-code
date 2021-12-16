using Core;

namespace Day13;

public static class Program
{
    public static void Main()
    {
        Part1();
        Part2();
    }

    public static void Part2()
    {
        List<FoldInstruction> foldInstructions;
        bool[][] bools;
        SetUpMapAndFolds(out foldInstructions, out bools);

        foreach (FoldInstruction foldInstruction in foldInstructions)
        {
            ApplyFold(ref bools, foldInstruction);
        }

        Console.WriteLine("Part 2:");
        foreach (bool[] line in bools)
        {
            Console.WriteLine();
            foreach (bool col in line)
            {
                Console.Write(col ? '#' : '.');
            }
        }
    }

    private static void Part1()
    {
        List<FoldInstruction> foldInstructions;
        bool[][] bools;
        SetUpMapAndFolds(out foldInstructions, out bools);

        FoldInstruction inst = foldInstructions[0];

        ApplyFold(ref bools, inst);

        int result = bools.SelectMany(b => b).Count(b => b);
        Console.WriteLine($"Part 1: {result}");
    }

    private static void SetUpMapAndFolds(out List<FoldInstruction> foldInstructions, out bool[][] bools)
    {
        string[] lines = File.ReadAllLines("./input.txt");

        List<IntPoint> points = new();
        foldInstructions = new();
        bool foundBlank = false;
        for (int i = 0; i < lines.Length; i++)
        {
            if (!foundBlank)
            {
                if (lines[i] == string.Empty)
                {
                    foundBlank = true;
                }
                else
                {
                    string[] split = lines[i].Split(',');
                    points.Add(new IntPoint(int.Parse(split[0]), int.Parse(split[1])));
                }
            }
            else
            {
                string axis = lines[i].Substring(11, 1);
                int index = int.Parse(lines[i].Substring(13));
                foldInstructions.Add(new FoldInstruction(axis, index));
            }
        }

        int maxX = points.Max(p => p.X);
        int maxY = points.Max(p => p.Y);

        bools = new bool[maxY + 1][];
        for (int i = 0; i < bools.Length; i++)
        {
            bools[i] = new bool[maxX + 1];
        }

        foreach (IntPoint point in points)
        {
            bools[point.Y][point.X] = true;
        }
    }

    private static void ApplyFold(ref bool[][] bools, FoldInstruction instruction)
    {
        if (instruction.Axis == "x")
        {
            FoldAlongX(ref bools, instruction.Index);
        }
        else
        {
            FoldAlongY(ref bools, instruction.Index);
        }
    }

    private static void FoldAlongY(ref bool[][] points, int index)
    {
        for (int y = index + 1; y < points.Length; y++)
        {
            for (int x = 0; x < points[y].Length; x++)
            {
                if (points[y][x])
                {
                    int newY = index - (y - index);
                    points[newY][x] = true;
                }
            }
        }
        Array.Resize(ref points, index);
    }

    private static void FoldAlongX(ref bool[][] points, int index)
    {
        for (int y = 0; y < points.Length; y++)
        {
            for (int i = index + 1; i < points[y].Length; i++)
            {
                if (points[y][i])
                {
                    int newIndex = index - (i - index);
                    points[y][newIndex] = true;
                }
            }
            Array.Resize(ref points[y], index);
        }
    }

    internal record struct FoldInstruction(string Axis, int Index);
}