using System.Reflection.Metadata.Ecma335;

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

    public IntSlope CalculateIntegerUnit()
    {
        return this switch 
        { 
            { X: > 0, Y: > 0 } => new IntSlope(1, 1), 
            { X: > 0, Y: < 0 } => new IntSlope(1, -1), 
            { X: > 0, Y: 0 } => new IntSlope(1, 0), 
            { X: 0, Y: > 0 } => new IntSlope(0, 1), 
            { X: 0, Y: < 0 } => new IntSlope(0, -1), 
            { X: 0, Y: 0 } => new IntSlope(0, 0), 
            { X: < 0, Y: > 0 } => new IntSlope(-1, 1), 
            { X: < 0, Y: < 0 } => new IntSlope(-1, -1), 
            { X: < 0, Y: 0 } => new IntSlope(-1, 0),
        };
    }
}
