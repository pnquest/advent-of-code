// See https://aka.ms/new-console-template for more information

string[][] grid = File.ReadAllLines("./input.txt")
    .Select(l => l.Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries))
    .ToArray();

Part1(grid);

Part2();

static void Part1(string[][] grid)
{
    long part1Result = 0;

    for (int x = 0; x < grid[0].Length; x++)
    {
        bool? isMult = null;
        long accumulator = 0;
        for (int y = grid.Length - 1; y >= 0; y--)
        {
            if (isMult == null)
            {
                isMult = grid[y][x] == "*";
                if (isMult == true)
                {
                    accumulator = 1;
                }
            }
            else if (isMult == true)
            {
                accumulator *= long.Parse(grid[y][x]);
            }
            else
            {
                accumulator += long.Parse(grid[y][x]);
            }
        }

        part1Result += accumulator;
    }

    Console.WriteLine($"Part 1: {part1Result}");
}

static void Part2()
{
    char[][] grid2 = File.ReadAllLines("./input.txt")
        .Select(l => l.ToCharArray())
        .ToArray();

    long part2Result = 0;


    int problemStart = 0;
    int problemEnd = grid2[^1].AsSpan()[1..].IndexOfAnyExcept(' ') - 1;
    bool shouldContinue = true;
    bool isLast = false;
    while (shouldContinue)
    {
        if (isLast)
        {
            shouldContinue = false;
        }

        bool isMult = grid2[^1][problemStart] == '*';
        long accumulator = isMult ? 1 : 0;

        List<char> chars = [];
        for (int x = problemEnd; x >= problemStart; x--)
        {
            chars.Clear();
            for (int y = 0; y < grid2.Length - 1; y++)
            {
                char cur = grid2[y][x];

                if (cur != ' ')
                {
                    chars.Add(cur);
                }
            }

            long number = 0;

            for (int i = 0; i < chars.Count; i++)
            {
                number += (chars[^(i + 1)] - '0') * (long)Math.Pow(10, i);
            }

            if (isMult)
            {
                accumulator *= number;
            }
            else
            {
                accumulator += number;
            }
        }

        part2Result += accumulator;

        if (!isLast)
        {
            problemStart = problemEnd + 2;
            problemEnd = grid2[^1].AsSpan()[(problemStart + 1)..].IndexOfAnyExcept(' ') + problemStart - 1;

            if (problemEnd < problemStart)
            {
                problemEnd = grid2[0].Length - 1;
                isLast = true;
            }
        }
    }

    Console.WriteLine($"Part 2: {part2Result}");
}
