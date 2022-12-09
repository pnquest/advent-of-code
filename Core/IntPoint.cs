namespace Core;

public record struct IntPoint(int X, int Y)
{
    public static Orientation GetOrientation(IntPoint p1, IntPoint p2, IntPoint p3)
    {
        int val = (p2.Y - p1.Y) * (p3.X - p2.X) - (p2.X - p1.X) * (p3.Y - p2.Y);

        return val switch {
            0 => Orientation.Colinear,
            > 0 => Orientation.Clockwise,
            < 0 => Orientation.CouterClockwise
        };
    }

    public int CalculateManhattenDistanceTo(IntPoint other)
    {
        return Math.Abs(X - other.X) + Math.Abs(Y - other.Y);
    }

    public IEnumerable<IntPoint> GetNeighbors(int minX, int maxX, int minY, int maxY, bool includeDiagonals = false)
    {
        return includeDiagonals
            ? GetDiagonalNeighbors(minX, maxX, minY, maxY)
            : GetOrthogonalNeighbors(minX, maxX, minY, maxY);
    }

    private IEnumerable<IntPoint> GetDiagonalNeighbors(int minX, int maxX, int minY, int maxY)
    {
        ReadOnlySpan<IntSlope> slopes = stackalloc IntSlope[] {
            new IntSlope(-1, -1),
            new IntSlope(-1, 0),
            new IntSlope(-1, 1),
            new IntSlope(0, -1),
            new IntSlope(0, 1),
            new IntSlope(1, -1),
            new IntSlope(1, 0),
            new IntSlope(1, 1)
        };

        List<IntPoint> neighbors = new();

        for (int i = 0; i < slopes.Length; i++)
        {
            IntPoint candidate = this + slopes[i];
            if (candidate.X >= minX && candidate.X <= maxX && candidate.Y >= minY && candidate.Y <= maxY)
            {
                neighbors.Add(candidate);
            }
        }

        return neighbors;
    }

    private IEnumerable<IntPoint> GetOrthogonalNeighbors(int minX, int maxX, int minY, int maxY)
    {
        ReadOnlySpan<IntSlope> slopes = stackalloc IntSlope[] {
            new IntSlope(-1, 0),
            new IntSlope(0, -1),
            new IntSlope(0, 1),
            new IntSlope(1, 0)
        };

        List<IntPoint> neighbors = new();

        for (int i = 0; i < slopes.Length; i++)
        {
            IntPoint candidate = this + slopes[i];
            if (candidate.X >= minX && candidate.X <= maxX && candidate.Y >= minY && candidate.Y <= maxY)
            {
                neighbors.Add(candidate);
            }
        }

        return neighbors;
    }

    public IEnumerable<IntPoint> GetRegion(int minX, int maxX, int minY, int maxY, bool includeDiagonals = false)
    {
        return includeDiagonals
            ? GetDiagonalRegion(minX, maxX, minY, maxY)
            : GetOrthogonalRegion(minX, maxX, minY, maxY);
    }

    private IEnumerable<IntPoint> GetDiagonalRegion(int minX, int maxX, int minY, int maxY)
    {
        ReadOnlySpan<IntSlope> slopes = stackalloc IntSlope[] {
            new IntSlope(-1, -1),
            new IntSlope(-1, 0),
            new IntSlope(-1, 1),
            new IntSlope(0, -1),
            new IntSlope(0, 0),
            new IntSlope(0, 1),
            new IntSlope(1, -1),
            new IntSlope(1, 0),
            new IntSlope(1, 1)
        };

        List<IntPoint> neighbors = new();

        for (int i = 0; i < slopes.Length; i++)
        {
            IntPoint candidate = this + slopes[i];
            if (candidate.X >= minX && candidate.X <= maxX && candidate.Y >= minY && candidate.Y <= maxY)
            {
                neighbors.Add(candidate);
            }
        }

        return neighbors;
    }

    private IEnumerable<IntPoint> GetOrthogonalRegion(int minX, int maxX, int minY, int maxY)
    {
        ReadOnlySpan<IntSlope> slopes = stackalloc IntSlope[] {
            new IntSlope(-1, 0),
            new IntSlope(0, -1),
            new IntSlope(0, 0),
            new IntSlope(0, 1),
            new IntSlope(1, 0)
        };

        List<IntPoint> neighbors = new();

        for (int i = 0; i < slopes.Length; i++)
        {
            IntPoint candidate = this + slopes[i];
            if (candidate.X >= minX && candidate.X <= maxX && candidate.Y >= minY && candidate.Y <= maxY)
            {
                neighbors.Add(candidate);
            }
        }

        return neighbors;
    }

    public enum Orientation
    {
        Colinear,
        Clockwise,
        CouterClockwise
    }

    public static IntSlope operator +(IntPoint pt, IntPoint slp)
    {
        return new IntSlope(pt.X + slp.X, pt.Y + slp.Y);
    }

    public static IntSlope operator -(IntPoint pt, IntPoint slp)
    {
        return new IntSlope(pt.X - slp.X, pt.Y - slp.Y);
    }
}

internal record struct CacheKey(int Item1, int Item2, int Item3, int Item4);
