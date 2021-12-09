using Core;

namespace Day9;

public static class Program
{
    public static void Main()
    {
        Part1();
        Part2();
    }

    private static void Part2()
    {
        Height[][]? map = File.ReadAllLines("./input.txt")
                    .Select((c, i) => c.ToCharArray().Select(c => c - '0').Select((c, j) => new Height { Value = c, Location = new IntPoint(j, i) }).ToArray()).ToArray();
        CalculateLowPoints(map);

        Span<int> lookAround = stackalloc int[] { -1, 0, 1 };
        List<int> basinSizes = new();
        HashSet<IntPoint> alreadyAdded = new();
        for (int i = 0; i < map.Length; i++)
        {
            for (int j = 0; j < map[i].Length; j++)
            {
                if(map[i][j].IsLowPoint)
                {
                    int basinSize = 1;
                    
                    Stack<Height> points = new Stack<Height>();
                    points.Push(map[i][j]);
                    if(!alreadyAdded.Add(map[i][j].Location))
                    {
                        continue;
                    }

                    while(points.Count > 0)
                    {
                        Height curPoint = points.Pop();

                        foreach(int lookX in lookAround)
                        {
                            foreach(int lookY in lookAround)
                            {
                                int newX = curPoint.Location.X + lookX;
                                int newY = curPoint.Location.Y + lookY;

                                if((lookX == 0 || lookY == 0) && newY >= 0 && newY < map.Length && newX >= 0 && newX < map[newY].Length && (newX != curPoint.Location.X || newY != curPoint.Location.Y))
                                {
                                    Height neighbor = map[newY][newX];

                                    if(neighbor.Value != 9 && alreadyAdded.Add(neighbor.Location))
                                    {
                                        basinSize++;
                                        points.Push(neighbor);
                                    }
                                }
                            }
                        }
                    }

                    basinSizes.Add(basinSize);
                }
            }
        }

        int answer = basinSizes.OrderByDescending(b => b).Take(3).Aggregate(1, (s, i) => s * i);

        Console.WriteLine($"Part 2: {answer}");
    }

    private static void Part1()
    {
        Height[][]? map = File.ReadAllLines("./input.txt")
                    .Select(c => c.ToCharArray().Select(c => c - '0').Select(c => new Height { Value = c }).ToArray()).ToArray();
        CalculateLowPoints(map);

        int answer = map.SelectMany(m => m).Where(m => m.IsLowPoint).Sum(m => m.ComputeScore());

        Console.WriteLine($"Part 1: {answer}");
    }

    private static void CalculateLowPoints(Height[][] map)
    {
        Span<int> lookAround = stackalloc int[] { -1, 0, 1 };

        for (int i = 0; i < map.Length; i++)
        {
            for (int j = 0; j < map[i].Length; j++)
            {
                bool isMin = true;
                Height curVal = map[i][j];

                foreach (int lookX in lookAround)
                {
                    foreach (int lookY in lookAround)
                    {
                        int newX = i + lookX;
                        int newY = j + lookY;

                        if ((lookX == 0 || lookY == 0) && newX >= 0 && newX < map.Length && newY >= 0 && newY < map[i].Length && (newX != i || newY != j))
                        {
                            if (curVal.Value > map[newX][newY].Value)
                            {
                                isMin = false;
                                break;
                            }
                        }
                    }

                    if (!isMin)
                    {
                        break;
                    }
                }

                curVal.IsLowPoint = isMin;
            }
        }
    }

    private class Height
    {
        public int Value { get; init; }
        public IntPoint Location { get; init; }
        public bool IsLowPoint { get; set; }

        public int ComputeScore() => Value + 1;
    }
}
