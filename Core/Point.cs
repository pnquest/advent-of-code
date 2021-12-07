using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core;

public record struct Point(int X, int Y)
{
    public static Orientation GetOrientation(Point p1, Point p2, Point p3)
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
