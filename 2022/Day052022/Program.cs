using Pidgin;

namespace Day052022;

internal class Program
{
    static void Main(string[] args)
    {
        Part1();
        Part2();
    }

    private static void Part2()
    {
        Setup(out Stack<char>[] stacks, out Move[] inst);

        foreach (Move mv in inst)
        {
            Stack<char> frm = stacks[mv.From - 1];
            Stack<char> to = stacks[mv.To - 1];
            Stack<char> tmp = new Stack<char>(mv.Count);

            for (int i = 0; i < mv.Count; i++)
            {
                tmp.Push(frm.Pop());
            }

            while (tmp.Count > 0)
            {
                to.Push(tmp.Pop());
            }
        }

        Console.Write("Part 2: ");
        for (int i = 0; i < stacks.Length; i++)
        {
            Console.Write(stacks[i].Peek());
        }
        Console.WriteLine();
    }

    private static void Part1()
    {
        Setup(out Stack<char>[] stacks, out Move[] inst);

        foreach (Move mv in inst)
        {
            Stack<char> frm = stacks[mv.From - 1];
            Stack<char> to = stacks[mv.To - 1];

            for (int i = 0; i < mv.Count; i++)
            {
                to.Push(frm.Pop());
            }
        }

        Console.Write("Part 1: ");
        for (int i = 0; i < stacks.Length; i++)
        {
            Console.Write(stacks[i].Peek());
        }
        Console.WriteLine();
    }

    private static void Setup(out Stack<char>[] stacks, out Move[] inst)
    {
        IEnumerable<string> stateLines = File.ReadAllLines("./state.txt").Reverse();

        stacks = new Stack<char>[9];
        for (int i = 0; i < stacks.Length; i++)
        {
            stacks[i] = new();
        }

        foreach (string line in stateLines)
        {
            for (int i = 0; i < stacks.Length; i++)
            {
                char c = line[1 + (4 * i)];
                if (c != ' ')
                {
                    stacks[i].Push(c);
                }
            }
        }


        var parser = Parser.Map((cnt, frm, to) => new Move(cnt, frm, to),
            Parser.String("move ").Then(Parser.Num),
            Parser.String(" from ").Then(Parser.Num),
            Parser.String(" to ").Then(Parser.Num));

        inst = File.ReadAllLines("./input.txt")
            .Select(l => parser.ParseOrThrow(l))
            .ToArray();
    }

    internal readonly record struct Move(int Count, int From, int To);
}
