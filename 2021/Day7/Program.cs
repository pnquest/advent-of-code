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
        int minVal = crabs.Min();
        int maxVal = crabs.Max();

        int curBest = minVal;
        int curCost = int.MaxValue;

        for (int i = minVal; i <= maxVal; i++)
        {
            int cost = crabs.Select(c => CalculateEscalatingFuelCost(c, i)).Sum();

            //if the cost has improved, it is the new best. If we have started getting worse, we have already found the answer, so break.
            if (cost < curCost)
            {
                curBest = i;
                curCost = cost;
            }
            else
            {
                break;
            }
        }

        Console.WriteLine($"Part 2: Best Position {curBest}; Best Cost: {curCost}");
    }

    private static int CalculateEscalatingFuelCost(int curPosition, int targetPosition)
    {
        return Enumerable.Range(0, Math.Abs(targetPosition - curPosition) + 1).Sum();
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