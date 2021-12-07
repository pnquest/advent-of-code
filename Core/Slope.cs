namespace Core;

public record struct Slope(int X, int Y)
{
    public static Point operator +(Point pt, Slope slp)
    {
        return new Point(pt.X + slp.X, pt.Y + slp.Y);
    }

    public static Point operator -(Point pt, Slope slp)
    {
        return new Point(pt.X - slp.X, pt.Y - slp.Y);
    }
}