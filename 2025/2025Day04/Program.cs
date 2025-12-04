char[][] map = File.ReadAllLines("input.txt")
    .Select(s => s.ToCharArray())
    .ToArray();

Part1(map);
Part2(map);

static int GetSurroundCount(char[][] map, int x, int y)
{
    int count = 0;
    for (int xOffset = -1; xOffset <= 1; xOffset++)
    {
        int lookX = x + xOffset;

        if (lookX >= 0 && lookX < map[0].Length)
        {
            for (int yOffset = -1; yOffset <= 1; yOffset++)
            {
                if (xOffset == 0 && yOffset == 0)
                {
                    continue;
                }

                int lookY = y + yOffset;

                if (lookY >= 0 && lookY < map.Length)
                {
                    if (map[lookY][lookX] == '@')
                    {
                        count++;
                    }
                }
            }
        }

    }

    return count;
}

static void Part1(char[][] map)
{
    int total = 0;

    for (int x = 0; x < map[0].Length; x++)
    {
        for (int y = 0; y < map.Length; y++)
        {
            if (map[y][x] == '@')
            {
                if (GetSurroundCount(map, x, y) < 4)
                {
                    total++;
                }
            }
        }
    }

    Console.WriteLine($"Part 1: {total}");
}

static void Part2(char[][] map)
{
    int total = 0;
    int totalStart = 0;

    do
    {
        totalStart = total;

        for (int x = 0; x < map[0].Length; x++)
        {
            for (int y = 0; y < map.Length; y++)
            {
                if (map[y][x] == '@')
                {
                    if (GetSurroundCount(map, x, y) < 4)
                    {
                        total++;
                        map[y][x] = 'X';
                    }
                }
            }
        }
    } while(total > totalStart);

    Console.WriteLine($"Part 2: {total}");
}
