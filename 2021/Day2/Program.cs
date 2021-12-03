// See https://aka.ms/new-console-template for more information
namespace Day2;

public static class Program
{
    public static void Main()
    {
        IEnumerable<(string Direction, int Distance)>? moves = File.ReadAllLines("./input.txt").Select(l => l.Split(" ")).Select(a => (a[0], int.Parse(a[1]))).ToList();
        Part1(moves);
        Part2(moves);
    }

    private static void Part2(IEnumerable<(string Direction, int Distance)> moves)
    {
        int horizontal = 0;
        int vertical = 0;
        int aim = 0;

        foreach (var move in moves)
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

    private static void Part1(IEnumerable<(string Direction, int Distance)> moves)
    {
        int horizontal = 0;
        int vertical = 0;

        foreach (var move in moves)
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
