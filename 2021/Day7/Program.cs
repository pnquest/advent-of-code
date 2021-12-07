namespace Day7;

public static class Program
{
    public static void Main()
    {
        int[] crabs = File.ReadAllText("./input.txt").Split(',').Select(int.Parse).OrderBy(i => i).ToArray();
        Part1(crabs);
        Part2(crabs);
    }

    private static void Part2(int[] crabs)
    {
        double average = crabs.Average();

        int high = (int)Math.Ceiling(average);
        int highSum = crabs.Sum(c => CalculateEscalatingFuelCost(c, high));

        int low = (int)Math.Floor(average);
        int lowSum = crabs.Sum(c => CalculateEscalatingFuelCost(c, low));

        if(highSum < lowSum)
        {
            Console.WriteLine($"Part 2: Best Target {high}; Cost: {highSum}");
        }
        else
        {
            Console.WriteLine($"Part 2: Best Target {low}; Cost: {lowSum}");
        }
    }

    private static int CalculateEscalatingFuelCost(int curPosition, int targetPosition)
    {
        int diff = Math.Abs(targetPosition - curPosition);
        return diff * (diff + 1) / 2;
    }

    private static void Part1(int[] crabs)
    {
        int median = CalculateMedian(crabs);

        int result = crabs.Select(c => Math.Abs(c - median)).Sum();

        Console.WriteLine($"Part 1: {result}");
    }

    private static int CalculateMedian(int[] crabs)
    {
        int median;

        if (crabs.Length % 2 == 0)
        {
            median = (crabs[crabs.Length / 2] + crabs[crabs.Length / 2 - 1]) / 2;
        }
        else
        {
            median = crabs[crabs.Length / 2];
        }

        return median;
    }
}