// See https://aka.ms/new-console-template for more information
using System.Data;
using System.Numerics;

int totals = File.ReadAllLines("input.txt")
    .Select(s => ComputeTotalPart1(s))
    .Sum();

Console.WriteLine($"Part 1: {totals}");

long totals2 = File.ReadAllLines("input.txt")
    .Select(s => ComputeTotalPart2(s))
    .Sum();

Console.WriteLine($"Part 2: {totals2}");

static long ComputeTotalPart2(ReadOnlySpan<char> line)
{
    int charsRemaining = 12;
    int curLimit = 0;
    long total = 0;

    while(charsRemaining > 0)
    {
        int curMax = -1;
        int maxIndex = -1;
        bool done = false;

        for(int i = curLimit; i <= line.Length - charsRemaining; i++)
        {
            int val = line[i] - 48;
            if(val == 9)
            {
                total += val * (long)Math.Pow(10, charsRemaining - 1);
                curLimit = i + 1;
                charsRemaining--;
                done = true;
                break;
            }
            if(val > curMax)
            {
                curMax = val;
                maxIndex = i;
            }
        }

        if(!done)
        {
            total += curMax * (long)Math.Pow(10, charsRemaining - 1);
            curLimit = maxIndex + 1;
            charsRemaining--;
        }
    }

    return total;
}

static int ComputeTotalPart1(ReadOnlySpan<char> line)
{
    int maxIndex = 0;
    int? maxValue = null;
    int? nextMax = null;

    for(int i = 0; i < line.Length - 1; i++)
    {
        int val = line[i] - 48;

        if(maxValue == null || maxValue < val)
        {
            maxIndex = i;
            maxValue = val;
            nextMax = null;
        }
        else if(nextMax == null || nextMax < val)
        {
            nextMax = val;
        }
    }

    int lastVal = line[^1] - 48;
    if(nextMax == null || lastVal > nextMax)
    {
        nextMax = lastVal;
    }

    return maxValue!.Value * 10 + nextMax!.Value;
}
