using System.Reflection.Metadata.Ecma335;

namespace Day102022;

internal class Program
{
    static void Main(string[] args)
    {
        Instruction[] lines = File.ReadAllLines("./input.txt")
            .Select(l => {
                return l switch {
                    [.. string inp] when inp == "noop" => new Instruction(1, 0),
                    ['a', 'd', 'd', 'x', ' ', .. string qty] => new Instruction(2, int.Parse(qty)),
                    _ => throw new InvalidOperationException("Unknown instruction")
                };
            })
            .ToArray();

        Part1(lines);
        Part2(lines);
    }

    private static void Part2(Instruction[] lines)
    {
        Console.WriteLine("Part 2:");

        int crtPosition = 0;
        int spritePosition = 1;


        foreach (Instruction instr in lines)
        {
            for (int i = 0; i < instr.Cycles; i++)
            {
                int prevPosition = crtPosition;
                if (crtPosition >= spritePosition - 1 && crtPosition <= spritePosition + 1)
                {
                    Console.Write('#');
                }
                else
                {
                    Console.Write('.');
                }
                crtPosition = (crtPosition + 1) % 40;
                if (crtPosition < prevPosition)
                {
                    Console.WriteLine();
                }
            }

            spritePosition += instr.Number;
        }
    }

    private static void Part1(Instruction[] lines)
    {
        int value = 1;
        int cycle = 0;
        int nextCheckCycle = 60;

        int finalResult = 0;

        foreach (Instruction line in lines)
        {
            for (int i = 0; i < line.Cycles; i++)
            {
                cycle++;
                CheckCycle(value, cycle, ref nextCheckCycle, ref finalResult);
            }
            value += line.Number;
        }

        Console.WriteLine($"Part 1: {finalResult}");
    }

    private static void CheckCycle(int value, int cycle, ref int nextCheckCycle, ref int finalResult)
    {
        if (cycle == 20 || cycle == nextCheckCycle)
        {
            finalResult += cycle * value;
            if (cycle > 20)
            {
                nextCheckCycle += 40;
            }
        }
    }

    internal readonly record struct Instruction(int Cycles, int Number);
}
