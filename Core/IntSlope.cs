namespace Core;

public record struct IntSlope(int X, int Y)
{
    public static IntPoint operator +(IntPoint pt, IntSlope slp)
    {
        return new IntPoint(pt.X + slp.X, pt.Y + slp.Y);
    }

    public static IntPoint operator -(IntPoint pt, IntSlope slp)
    {
        return new IntPoint(pt.X - slp.X, pt.Y - slp.Y);
    }
}