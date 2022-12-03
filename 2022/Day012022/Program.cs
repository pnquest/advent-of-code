namespace Day012022;

internal class Program
{
    static void Main(string[] args)
    {
        Part1();
        Part2();
    }

    private static void Part2()
    {
        IEnumerable<long> calories = GetElfCalories();
        Console.WriteLine($"Part 2: {calories.OrderByDescending(c => c).Take(3).Sum()}");
    }

    private static void Part1()
    {
        IEnumerable<long> calories = GetElfCalories();

        Console.WriteLine($"Part 1: {calories.Max()}");
    }

    private static IEnumerable<long> GetElfCalories()
    {
        string[] lines = File.ReadAllLines("./input.txt");

        return lines.PartitionBy(l => l == string.Empty)
            .Select(p => p.Select(long.Parse).Sum());
    }
}
