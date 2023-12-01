namespace Day012023;

internal class Program
{
    static void Main(string[] args)
    {
        string[] lines = File.ReadAllLines("./input.txt");
        int result = lines.Sum(GetNumberFromLinePart1);
        Console.WriteLine($"Part 1: {result}");

        int result2 = lines.Sum(GetNumberFromLinePart2);
        Console.WriteLine($"Part 2: {result2}");
    }

    private static int GetNumberFromLinePart1(string line)
    {
        char? firstDigit = null;
        char? finalDigit = null;

        foreach(char c in line)
        {
            if(char.IsDigit(c)) 
            {
                firstDigit ??= c;
                finalDigit = c;
            }
        }

        return int.Parse(new string(new[] { firstDigit!.Value, finalDigit!.Value }));
    }

    private static int GetNumberFromLinePart2(string line)
    {
        int? firstDigit = null;
        int? lastDigit = null;

        int offset = -1;
        while(++offset < line.Length)
        {
            int remainder = Math.Min(5, line.Length - offset);
            ReadOnlySpan<char> span = line.AsSpan()[offset..(offset + remainder)];
            int? foundDigit = null;
            if (char.IsDigit(span[0]))
            {
                foundDigit = span[0] - '0';
            }
            else if (span.StartsWith("one".AsSpan()))
            {
                foundDigit = 1;
            }
            else if(span.StartsWith("two".AsSpan()))
            {
                foundDigit = 2;
            }
            else if(span.StartsWith("three".AsSpan()))
            {
                foundDigit = 3;
            }
            else if(span.StartsWith("four".AsSpan()))
            {
                foundDigit = 4;
            }
            else if(span.StartsWith("five".AsSpan()))
            {
                foundDigit = 5;
            }
            else if(span.StartsWith("six".AsSpan()))
            {
                foundDigit = 6;
            }
            else if(span.StartsWith("seven".AsSpan()))
            {
                foundDigit = 7;
            }
            else if(span.StartsWith("eight".AsSpan()))
            {
                foundDigit = 8;
            }
            else if(span.StartsWith("nine".AsSpan()))
            {
                foundDigit = 9;
            }

            if(foundDigit != null)
            {
                firstDigit ??= foundDigit;
                lastDigit = foundDigit;
            }
        }

        return firstDigit!.Value * 10 + lastDigit!.Value;
    }
        
}
