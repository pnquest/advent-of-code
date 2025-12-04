// See https://aka.ms/new-console-template for more information


using System.Text.RegularExpressions;

IEnumerable<Range> items = File.ReadAllText("./input.txt").Split(',').Select(x => {
    var rng = x.Split('-');

    return new Range {
        Min = long.Parse(rng[0]),
        Max = long.Parse(rng[1])
    };
});

List<long> invalidIdsPart1 = new();
List<long> invalidIdsPart2 = new();

List<long> invalidIdsPart1Regex = new();
List<long> invalidIdsPart2Regex = new();

foreach (Range rng in items)
{
    for(long i = rng.Min; i <= rng.Max; i++)
    {
        string idString = i.ToString();
        if (!ValidateIdPart1(idString))
        {
            invalidIdsPart1.Add(i);
        }

        if (!ValidateIdPart1Regex(idString))
        {
            invalidIdsPart1Regex.Add(i);
        }

        if (!ValidateIdPart2(idString))
        {
            invalidIdsPart2.Add(i);
        }

        if (!ValidateIdPart2Regex(idString))
        {
            invalidIdsPart2Regex.Add(i);
        }
    }
}

Console.WriteLine($"Part 1: {invalidIdsPart1.Sum()}");
Console.WriteLine($"Part 2: {invalidIdsPart2.Sum()}");

Console.WriteLine($"Part 1 Regex: {invalidIdsPart1Regex.Sum()}");
Console.WriteLine($"Part 2 Regex: {invalidIdsPart2Regex.Sum()}");

static bool ValidateIdPart1(ReadOnlySpan<char> id)
{
    if(id.Length % 2 != 0)
    {
        return true; //has to be even length to repeat
    }

    int halfwayIndex = (id.Length / 2 );

    ReadOnlySpan<char> first = id[..halfwayIndex];
    ReadOnlySpan<char> second = id[halfwayIndex..];

    return !first.SequenceEqual(second);
}

static bool ValidateIdPart1Regex(string id)
{
    return !Part1Regex().IsMatch(id);
}

static bool ValidateIdPart2Regex(string id)
{
    return !Part2Regex().IsMatch(id);
}

static bool ValidateIdPart2(ReadOnlySpan<char> id)
{
    char first = id[0];

    for(int i = 1; i < id.Length; i++)
    {
        int subLength = i;
        int remLength = id.Length;
        if (id[i] == first && id.Length % subLength == 0) //check to see if the length of the pattern is cleanly divisible by the total length.
        {
            int startIndex = subLength;
            ReadOnlySpan<char> firstSub = id[..subLength];
            remLength -= firstSub.Length;
            bool match = true;
            while(remLength > 0)
            {
                ReadOnlySpan<char> next = id[startIndex..(subLength + startIndex)];

                if(!firstSub.SequenceEqual(next))
                {
                    match = false;
                    break;
                }

                remLength -= next.Length;
                startIndex += subLength;
            }

            if(match)
            {
                return false;
            }
        }
    }

    return true;
}

internal readonly struct Range
{
    public required long Min { get; init; }
    public required long Max { get; init; }
}

internal partial class Program
{
    [GeneratedRegex(@"^(.+?)\1$")]
    private static partial Regex Part1Regex();

    [GeneratedRegex(@"^(.+?)\1+$")]
    private static partial Regex Part2Regex();
}
