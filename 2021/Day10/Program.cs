namespace Day10;

public static class Program
{
    public static void Main()
    {
        string[] lines = File.ReadLines("./input.txt").ToArray();
        Part1(lines);
        Part2(lines);
    }

    private static void Part2(string[] lines)
    {
        List<long> completeScores = new();

        foreach (string line in lines)
        {
            Stack<char> lineStack = new Stack<char>();

            bool wasValid = true;

            foreach (char c in line)
            {
                if (c is '(' or '[' or '{' or '<')
                {
                    lineStack.Push(c);
                }
                else if (DoesCloseMatch(lineStack.Peek(), c))
                {
                    lineStack.Pop();
                }
                else
                {
                    wasValid = false;
                    break;
                }
            }

            if (wasValid)
            {
                long rowCompleteScore = 0;

                while (lineStack.Count > 0)
                {
                    char curOpen = lineStack.Pop();
                    char properClose = GetMatchingClose(curOpen);

                    rowCompleteScore = rowCompleteScore * 5 + CalculateCompletionScore(properClose);
                }

                completeScores.Add(rowCompleteScore);
            }
        }

        long middleScore = CalculateMedian(completeScores);

        Console.WriteLine($"Part 2: {middleScore}");
    }

    private static void Part1(string[] lines)
    {
        long errorScore = 0;

        foreach (string line in lines)
        {
            Stack<char> lineStack = new Stack<char>();

            foreach (char c in line)
            {
                if (c is '(' or '[' or '{' or '<')
                {
                    lineStack.Push(c);
                }
                else if (DoesCloseMatch(lineStack.Peek(), c))
                {
                    lineStack.Pop();
                }
                else
                {
                    errorScore += CalculateErrorScore(c);
                    break;
                }
            }
        }

        Console.WriteLine($"Part 1: {errorScore}");
    }

    private static long CalculateMedian(IEnumerable<long> input)
    {
        long median;

        long[] inp = input.OrderBy(c => c).ToArray();

        if (inp.Length % 2 == 0)
        {
            median = (inp[inp.Length / 2] + inp[inp.Length / 2 - 1]) / 2;
        }
        else
        {
            median = inp[inp.Length / 2];
        }

        return median;
    }

    private static char GetMatchingClose(char open)
    {
        return open switch {
            '(' => ')',
            '[' => ']',
            '{' => '}',
            '<' => '>',
            _ => throw new ArgumentException("input not supported")
        };
    }

    private static bool DoesCloseMatch(char open, char close)
    {
        return GetMatchingClose(open) == close;
    }

    private static long CalculateCompletionScore(char illegalChar)
    {
        return illegalChar switch {
            ')' => 1,
            ']' => 2,
            '}' => 3,
            '>' => 4,
            _ => throw new ArgumentException("input not supported")
        };
    }

    private static long CalculateErrorScore(char illegalChar)
    {
        return illegalChar switch {
            ')' => 3,
            ']' => 57,
            '}' => 1197,
            '>' => 25137,
            _ => throw new ArgumentException("input not supported")
        };
    }
}
