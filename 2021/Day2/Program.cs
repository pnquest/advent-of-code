// See https://aka.ms/new-console-template for more information
namespace Day2;

internal record struct Move(string Direction, int Distance);

public static class Program
{
    public static void Main()
    {
        IEnumerable<Move>? moves = File
            .ReadAllLines("./input.txt")
            .Select(l => l.Split(" "))
            .Select(a => new Move(a[0], int.Parse(a[1])))
            .ToList();

        Part1(moves);
        Part2(moves);
    }

    private static void Part2(IEnumerable<Move> moves)
    {
        int horizontal = 0;
        int vertical = 0;
        int aim = 0;

        foreach (Move move in moves)
        {
            switch (move.Direction)
            {
                case "down":
                    aim += move.Distance;
                    break;

                case "up":
                    aim -= move.Distance;
                    break;

                case "forward":
                    horizontal += move.Distance;
                    vertical += (aim * move.Distance);
                    break;
            }
        }

        Console.WriteLine($"Part 2: {vertical * horizontal}");
    }

    private static void Part1(IEnumerable<Move> moves)
    {
        int horizontal = 0;
        int vertical = 0;

        foreach (Move move in moves)
        {
            switch (move.Direction)
            {
                case "forward":
                    horizontal += move.Distance;
                    break;

                case "down":
                    vertical += move.Distance;
                    break;

                case "up":
                    vertical -= move.Distance;
                    break;
            }
        }

        Console.WriteLine($"Part 1: {horizontal * vertical}");
    }
}
