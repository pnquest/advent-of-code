namespace Core;

public record struct IntPoint3(int X, int Y, int Z)
{
    public IntPoint3 Transform(int x, int y, int z)
    {
        return new IntPoint3(X + x, Y + y, Z + z);
    }

    public double ComputeDistance(IntPoint3 other)
    {
        return Math.Sqrt(Math.Pow(other.X - X, 2) + Math.Pow(other.Y - Y, 2) + Math.Pow(other.Z - Z, 2));
    }

    public static IntPoint3 Parse(ReadOnlySpan<char> input)
    {
        Span<Range> ranges = stackalloc Range[3];

        input.Split(ranges, ',');

        return new IntPoint3(int.Parse(input[ranges[0]]), int.Parse(input[ranges[1]]), int.Parse(input[ranges[2]]));
    }
}
