using System.Collections.Frozen;

namespace Day022023;

internal class Program
{
    static void Main(string[] args)
    {
        Game[] games = File.ReadAllLines("./input.txt")
            .Select(ParseGame)
            .ToArray();

        Part1(games);
        Part2(games);
    }

    private static void Part2(Game[] games)
    {
        int totalPower = 0;
        foreach (Game game in games)
        {
            totalPower += CalculatePower(game);
        }

        Console.WriteLine($"Part 2: {totalPower}");
    }

    private static int CalculatePower(Game game)
    {
        var colors = new Dictionary<string, int>();

        foreach (Draw draw in game.Draws)
        {
            foreach (Cube cube in draw.Cubes)
            {
                if (!colors.TryGetValue(cube.Color, out int value))
                {
                    colors[cube.Color] = 0;
                }

                if (cube.Count > value)
                {
                    colors[cube.Color] = cube.Count;
                }
            }
        }

        return colors.Values.Aggregate(1, (acc, next) => acc * next);
    }

    private static void Part1(Game[] games)
    {
        var limits = new Dictionary<string, int> {
            ["red"] = 12,
            ["green"] = 13,
            ["blue"] = 14
        }.ToFrozenDictionary();

        List<int> legalIds = new();

        foreach (Game game in games)
        {
            bool isLegal = true;
            foreach (Draw draw in game.Draws)
            {
                foreach (Cube cube in draw.Cubes)
                {
                    if (limits[cube.Color] < cube.Count)
                    {
                        isLegal = false;
                    }
                }
            }

            if (isLegal)
            {
                legalIds.Add(game.Id);
            }
        }

        Console.WriteLine($"Part 1: {legalIds.Sum()}");
    }

    private readonly record struct Cube(string Color, int Count);
    private readonly record struct Draw(Cube[] Cubes);
    private readonly record struct Game(int Id, Draw[] Draws);

    private static Game ParseGame(string gameString)
    {
        string[] firstSplit = gameString.Split(": ");
        int id = int.Parse(firstSplit[0].Split(' ')[1]);

        string[] drawsSplit = firstSplit[1].Split("; ");

        var draws = new List<Draw>();

        foreach(string draw in drawsSplit)
        {
            var cubes = new List<Cube>();
            string[] cubeStrings = draw.Split(", ");

            foreach(string cube in cubeStrings)
            {
                string[] cubeSplit = cube.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                cubes.Add(new Cube(cubeSplit[1], int.Parse(cubeSplit[0])));
            }

            draws.Add(new Draw(cubes.ToArray()));
        }

        return new Game(id, draws.ToArray());
    }
}
