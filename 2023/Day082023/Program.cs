using System.Collections.Frozen;

namespace Day082023;

internal class Program
{
    static void Main(string[] args)
    {
        string[] lines = File.ReadAllLines("./input.txt");

        string instructions = lines[0];

        FrozenDictionary<string, PathNode> nodes = lines
            .Skip(2)
            .Select(ParseNode)
            .ToFrozenDictionary(d => d.Location);

        Part1(instructions, nodes);

        var curNodes = nodes.Keys.Where(k => k.EndsWith('A')).ToDictionary(d => d, _ => new List<long>());

        long curSteps = 0;

        foreach(var dir in instructions.Repeat())
        {
            if(curNodes.All(n => n.Value.Count > 1))
            {
                break;
            }

            curSteps++;
            foreach(var node in curNodes)
            {

            }
        }
    }

    private static void Part1(string instructions, FrozenDictionary<string, PathNode> nodes)
    {
        int steps = 0;
        string curLocation = "AAA";

        foreach (char inst in instructions.Repeat())
        {
            if (curLocation == "ZZZ")
            {
                break;
            }

            steps++;
            PathNode node = nodes[curLocation];

            if (inst is 'L')
            {
                curLocation = node.Left;
            }
            else
            {
                curLocation = node.Right;
            }
        }

        Console.WriteLine($"Part 1: {steps}");
    }

    private static PathNode ParseNode(string input)
    {
        string[] sides = input.Split(" = ");
        string location = sides[0];

        string[] paths = sides[1].Trim('(', ')').Split(", ");

        return new PathNode(location, paths[0], paths[1]);
    }

    private readonly record struct PathNode(string Location, string Left, string Right);
}
