﻿using System.Diagnostics.CodeAnalysis;

namespace Core;

public record struct IntPoint(int X, int Y) : IParsable<IntPoint>
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

        List<IntPoint> neighbors = [];

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

        List<IntPoint> neighbors = [];

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

        List<IntPoint> neighbors = [];

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

        List<IntPoint> neighbors = [];

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

    public static IntPoint Parse(string s, IFormatProvider? provider)
    {
        int commaIndex = s.IndexOf(',');
        int x = int.Parse(s.AsSpan()[..commaIndex]);
        int y = int.Parse(s.AsSpan()[(commaIndex + 1)..]);
        return new IntPoint(x, y);
    }
    public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, [MaybeNullWhen(false)] out IntPoint result)
    {
        if(s == null)
        {
            result = default;
            return false;
        }

        try
        {
            result = Parse(s, null);
            return true;
        }
        catch
        {
            result = default;
            return false;
        }
    }

    public enum Orientation
    {
        Colinear,
        Clockwise,
        CouterClockwise
    }

    public static IntSlope operator +(in IntPoint pt, in IntPoint slp)
    {
        return new IntSlope(pt.X + slp.X, pt.Y + slp.Y);
    }

    public static IntSlope operator -(in IntPoint pt, in IntPoint slp)
    {
        return new IntSlope(pt.X - slp.X, pt.Y - slp.Y);
    }

    public IntSlope CalculateUnitDirection(in IntPoint other)
    {
        int x;
        if(other.X == X)
        {
            x = 0;
        }
        else if(other.X > X)
        {
            x = 1;
        }
        else
        {
            x = -1;
        }

        int y;
        if(other.Y == Y)
        {
            y = 0;
        }
        else if(other.Y > Y)
        {
            y = 1;
        }
        else
        {
            y = -1;
        }

        return new IntSlope(x, y);
    }
}
