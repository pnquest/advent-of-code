// See https://aka.ms/new-console-template for more information

namespace Day1;

public static class Program
{
    public static void Main()
    {
        List<int> measures = File.ReadAllLines("./input.txt").Select(int.Parse).ToList();
        Part1(measures);
        Part2(measures);
    }

    private static void Part2(List<int> measures)
    {
        int prevWindow = measures.Take(3).Sum();
        int curWindow = prevWindow;

        Queue<int> nextWindow = new(measures.Take(3));

        int increases = 0;

        foreach (int measure in measures.Skip(3))
        {
            int onTheWayOut = nextWindow.Dequeue();
            curWindow += -onTheWayOut + measure;
            nextWindow.Enqueue(measure);

            if (prevWindow < curWindow)
            {
                increases++;
            }
            prevWindow = curWindow;
        }

        Console.WriteLine($"Part 2: {increases}");
    }

    private static void Part1(IEnumerable<int> measures)
    {
        int? prev = null;

        int increases = 0;

        foreach (int measure in measures)
        {
            if (measure > prev)
            {
                increases++;
            }

            prev = measure;
        }

        Console.WriteLine($"Part 1: {increases}");
    }
}
