// See https://aka.ms/new-console-template for more information
string[] lines = File.ReadAllLines("./input.txt");

List<Range> ranges = [];

long index = 0;

while (lines[index] != string.Empty)
{
    ranges.Add(Range.Parse(lines[index++]));
}

int freshCount = 0;

for(++index;index < lines.Length; index++)
{
    long value = long.Parse(lines[index]);

    foreach (Range range in ranges)
    {
        if(range.Contains(value))
        {
            freshCount++;
            break;
        }
    }
}

Console.WriteLine($"Part 1: {freshCount}");

List<Range> part2Ranges = [ranges[0]];

foreach(Range range in ranges[1..])
{
    Queue<Range> rangesToAdd = new([range]);

    foreach(Range existing in part2Ranges)
    {
        int queueLength = rangesToAdd.Count;

        for(int i = 0; i < queueLength; i++)
        {
            Range toAdd = rangesToAdd.Dequeue();
            
            foreach(Range split in toAdd.RemoveOverlap(existing))
            {
                rangesToAdd.Enqueue(split); 
            }
        }
    }

    part2Ranges.AddRange(rangesToAdd);
}

Console.WriteLine($"Part 2: {part2Ranges.Sum(r => r.CountRange())}");

readonly record struct Range(long Min, long Max)
{
    public static Range Parse(ReadOnlySpan<char> input)
    {
        int dashIndex = input.IndexOf('-');

        long min = long.Parse(input[..dashIndex]);
        long max = long.Parse(input[(dashIndex + 1)..]);

        return new Range(min, max);
    }

    public bool Contains(long value)
    {
        return value >= Min && value <= Max;
    }

    public IEnumerable<Range> RemoveOverlap(Range other)
    {
        //create a new range based on this one where anything that overlaps it is removed. If this range is fully contained by the other, then return default
        if(other.Min <= Min && other.Max >= Max)
        {
            yield break;
        }

        if(other.Max < Min || other.Min > Max)
        {
            yield return this;
            yield break;
        }

        // Case 1: other cuts off the left side of this one.
        if(other.Min <= Min && other.Max < Max)
        {
            yield return new Range(other.Max + 1, Max);
        }

        //Case 2: other cuts off the right side of this one.
        if(other.Max > Min && other.Max >= Max)
        {
            yield return new Range(Min, other.Min - 1);
        }

        //case 3: other takes a chunk out of the middle of this one

        if(other.Min > Min && other.Max < Max)
        {
            yield return new Range(Min, other.Min - 1);
            yield return new Range(other.Max + 1, Max);
        }
    }

    public long CountRange()
    {
        return Max - Min + 1;
    }
}
