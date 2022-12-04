namespace Day042022;

internal class Program
{
    static void Main(string[] args)
    {
        AssignmentPair[] assignments = File.ReadAllLines("./input.txt")
                    .Select(i => i.Split(','))
                    .Select(i => (i[0].Split('-'), i[1].Split('-')))
                    .Select(i => new AssignmentPair(new Assignment(int.Parse(i.Item1[0]), int.Parse(i.Item1[1])), new Assignment(int.Parse(i.Item2[0]), int.Parse(i.Item2[1]))))
                    .ToArray();

        Part1(assignments);
        Part2(assignments);

    }

    private static void Part2(AssignmentPair[] assignments)
    {
        int overlapped = assignments.Count(p => p.HasAnyOverlap());

        Console.WriteLine($"Part 2: {overlapped}");
    }

    private static void Part1(AssignmentPair[] assignments)
    {
        int contained = assignments
                    .Count(p => p.DoesOneContainTheOther());

        Console.WriteLine($"Part 1: {contained}");
    }

    private readonly record struct Assignment(int Start, int End)
    {
        public bool FullyContains(in Assignment other)
        {
            return Start <= other.Start && End >= other.End;
        }

        public bool Overlaps(in Assignment other)
        {
            return Start <= other.End && other.Start <= End;
        }
    }
    private readonly record struct AssignmentPair(Assignment First, Assignment Second)
    {
        public bool DoesOneContainTheOther()
        {
            return First.FullyContains(Second) || Second.FullyContains(First);
        }

        public bool HasAnyOverlap()
        {
            return First.Overlaps(Second);
        }
    }
}
