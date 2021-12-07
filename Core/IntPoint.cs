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

    public enum Orientation
    {
        Colinear,
        Clockwise,
        CouterClockwise
    }
}
