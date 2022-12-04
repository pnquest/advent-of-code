namespace Day032022;

internal class Program
{
    private const int _lowerOffset = 1 - 'a';
    private const int _upperOffset = 27 - 'A';

    static void Main(string[] args)
    {
        string[] lines = File.ReadAllLines("./input.txt");
        Part1(lines);
        Part2(lines);

    }

    private static void Part2(string[] lines)
    {
        long result = lines.Chunk(3)
                    .SelectMany(l => l[0].Intersect(l[1]).Intersect(l[2]).ToHashSet())
                    .Sum(ConvertToScore);

        Console.WriteLine($"Part 2: {result}");
    }

    private static void Part1(string[] lines)
    {
        long result = lines
                    .SelectMany(l => l.AsSpan()[..(l.Length / 2)].Intersect(l.AsSpan()[(l.Length / 2)..]).ToHashSet())
                    .Sum(ConvertToScore);

        Console.WriteLine($"Part 1: {result}");
    }

    private static int ConvertToScore(char c)
        => char.IsLower(c) ? c + _lowerOffset : c + _upperOffset;
}
