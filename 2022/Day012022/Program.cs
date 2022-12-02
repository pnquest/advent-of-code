namespace Day012022;

internal class Program
{
    static void Main(string[] args)
    {
        List<long> calories = GetElfCalories();

        Console.WriteLine($"Part 1: {calories.Max()}");
        Console.WriteLine($"Part 2: {calories.OrderByDescending(c => c).Take(3).Sum()}");
    }

    private static List<long> GetElfCalories()
    {
        string[] lines = File.ReadAllLines("./input.txt");

        var calories = new List<long>();

        long curElf = 0;

        foreach (string line in lines)
        {
            if (line == string.Empty)
            {
                calories.Add(curElf);
                curElf = 0;
            }
            else
            {
                curElf += long.Parse(line);
            }
        }

        return calories;
    }
}
