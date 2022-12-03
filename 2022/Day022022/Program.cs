namespace Day022022;

internal class Program
{
    static void Main(string[] args)
    {
        string[] lines = File.ReadAllLines("./input.txt");
        Part1(lines);
        Part2(lines);
    }

    private static void Part2(string[] lines)
    {
        long totalScore = lines
                    .Select(l => CalculateRoundFromDesired(MapChoice(l[0]), MapResult(l[2])))
                    .Sum(r => (long)r.RoundScore);

        Console.WriteLine($"Part 2: {totalScore}");
    }

    private static void Part1(string[] lines)
    {
        long totalScore = lines
                    .Select(l => new Round(MapChoice(l[0]), MapChoice(l[2])))
                    .Sum(r => (long)r.RoundScore);

        Console.WriteLine($"Part 1: {totalScore}");
    }

    static internal Choice MapChoice(char c)
    {
        return c switch 
        {
            'A' or 'X' => Choice.Rock,
            'B' or 'Y' => Choice.Paper,
            'C' or 'Z' => Choice.Scisors,
            _ => throw new ArgumentException("Not supported", nameof(c))
        };
    }

    static internal RoundResult MapResult(char c)
    {
        return c switch 
        {
            'X' => RoundResult.Loss,
            'Y' => RoundResult.Draw,
            'Z' => RoundResult.Win,
            _ => throw new ArgumentException("Invalid value", nameof(c))
        };
    }

    static internal Round CalculateRoundFromDesired(Choice opponent, RoundResult desiredResult)
    {
        Choice you = (opponent, desiredResult) switch {
            (Choice opp, RoundResult.Draw) => opp,
            (Choice.Rock, RoundResult.Loss) => Choice.Scisors,
            (Choice.Rock, RoundResult.Win) => Choice.Paper,
            (Choice.Paper, RoundResult.Loss) => Choice.Rock,
            (Choice.Paper, RoundResult.Win) => Choice.Scisors,
            (Choice.Scisors, RoundResult.Loss) => Choice.Paper,
            (Choice.Scisors, RoundResult.Win) => Choice.Rock,
            _ => throw new InvalidOperationException("Invalid combination")
        };

        return new Round(opponent, you);
    }

    internal enum Choice
    {
        Rock = 1,
        Paper = 2,
        Scisors = 3
    }

    internal enum RoundResult
    {
        Loss = 0,
        Draw = 3,
        Win = 6
    }

    internal readonly record struct Round(Choice Opponant, Choice You)
    {
        public int RoundScore => (int)You + (int)GetResult();

        public RoundResult GetResult()
        {
            return this switch 
            {
                _ when You == Opponant => RoundResult.Draw,
                { You: Choice.Rock, Opponant: Choice.Scisors } => RoundResult.Win,
                { You: Choice.Rock, Opponant: Choice.Paper } => RoundResult.Loss, 
                { You: Choice.Paper, Opponant: Choice.Rock } => RoundResult.Win, 
                { You: Choice.Paper, Opponant: Choice.Scisors } => RoundResult.Loss, 
                { You: Choice.Scisors, Opponant: Choice.Paper } => RoundResult.Win, 
                { You: Choice.Scisors, Opponant: Choice.Rock } => RoundResult.Loss,
                _ => throw new InvalidOperationException("Invalid combination")
            };
        }
    }
}
