using Core;
using Pidgin;

namespace Day112022;

internal class Program
{
    static void Main(string[] args)
    {
        Part1();
        Part2();
    }

    private static void Part2()
    {
        Monkey[] monkeys = ParseMonkeys();
        long lcm = AocMath.CalculateLeastCommonMultiple(monkeys.Select(m => m.Test.DivisibleWhen).ToArray());
        long result = CalculateResult(monkeys, 10000, lcm, true);
        Console.WriteLine($"Part 2: {result}");
    }

    private static void Part1()
    {
        Monkey[] monkeys = ParseMonkeys();
        long result = CalculateResult(monkeys, 20, 3, false);

        Console.WriteLine($"Part 1: {result}");
    }

    private static long CalculateResult(Monkey[] monkeys, long rounds, long divisor, bool normalizeToTarget)
    {
        Dictionary<long, long> inspections = new();

        for (long r = 0; r < rounds; r++)
        {
            foreach (Monkey m in monkeys)
            {
                while (m.Items.Count > 0)
                {
                    inspections.SetOrIncrement(m.Name, 1);
                    long item = m.Items.Dequeue();
                    long newVal = m.Operation.ApplyOperation(item);

                    if(normalizeToTarget)
                    {
                        newVal %= divisor;
                    }
                    else
                    {
                        newVal /= divisor;
                    }
                    Monkey newTarget = monkeys[m.Test.CalculateNewTarget(newVal)];

                    newTarget.Items.Enqueue(newVal);
                }
            }
        }

        long result = inspections.Values.OrderByDescending(v => v).Take(2).Aggregate(1L, (a, v) => a * v);
        return result;
    }

    private static Monkey[] ParseMonkeys()
    {
        string text = File.ReadAllText("./input.txt");

        Parser<char, long> monkeyNameParser = Parser.String("Monkey ").Then(Parser.LongNum.Before(Parser.Char(':').Before(Parser.EndOfLine)));
        Parser<char, Queue<long>> monkeyItemsParser = Parser.Whitespaces
            .Then(Parser.String("Starting items: ")
                    .Then(Parser.LongNum.SeparatedAndOptionallyTerminatedAtLeastOnce(Parser.String(", ")).Map(v => new Queue<long>(v)).Before(Parser.EndOfLine)));
        Parser<char, string> numOrOldParser = Parser.Num.Map(i => i.ToString()).Or(Parser.String("old"));
        Parser<char, Operation> operationParser = Parser.Whitespaces.Then(Parser.String("Operation: new = ")
            .Then(Parser.Map((n1, o, n2) => new Operation(n1, o, n2), numOrOldParser.Before(Parser.Whitespace), Parser.AnyCharExcept().Before(Parser.Whitespace), numOrOldParser))
            .Before(Parser.EndOfLine));

        Parser<char, long> testStartParser = Parser.Whitespaces.Then(Parser.String("Test: divisible by ").Then(Parser.LongNum.Before(Parser.EndOfLine)));
        Parser<char, long> testWhenParser = Parser.Whitespaces.Then(Parser.String("If ").Then(Parser.String("true: ").Or(Parser.String("false: "))).Then(Parser.String("throw to monkey ")).Then(Parser.LongNum.Before(Parser.EndOfLine)));
        Parser<char, Test> testParser = Parser.Map((s, w) => new Test(s, w[0], w[1]), testStartParser, testWhenParser.Repeat(2).Map(w => w.ToArray()));

        Parser<char, Monkey> monkeyParser = Parser.Map((n, i, o, t) => new Monkey(n, i, o, t), monkeyNameParser, monkeyItemsParser, operationParser, testParser);

        Parser<char, Monkey[]> monkeysParser = monkeyParser.SeparatedAndOptionallyTerminatedAtLeastOnce(Parser.EndOfLine).Map(m => m.ToArray());

        Monkey[] monkeys = monkeysParser.ParseOrThrow(text);
        return monkeys;
    }

    internal readonly record struct Operation(string Number1, char Operator, string Number2)
    {
        public long ApplyOperation(long old)
        {
            long n1 = Number1 == "old"
                ? old
                : long.Parse(Number1);

            long n2 = Number2 == "old"
                ? old
                : long.Parse(Number2);

            return Operator switch {
                '+' => (n1 + n2),
                '*' => n1 * n2,
                _ => throw new InvalidOperationException("Unknown operator")
            };
        }
    };

    internal readonly record struct Test(long DivisibleWhen, long TrueTarget, long FalseTarget)
    {
        public long CalculateNewTarget(long inputValue)
        {
            if(inputValue % DivisibleWhen == 0)
            {
                return TrueTarget;
            }

            return FalseTarget;
        }
    };
    internal record Monkey(long Name, Queue<long> Items, Operation Operation, Test Test);
}
