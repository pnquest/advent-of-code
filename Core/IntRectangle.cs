namespace Core;
public readonly record struct IntRectangle(IntPoint TopLeft, IntPoint TopRight, IntPoint BottomLeft, IntPoint BottomRight)
{
    public IEnumerable<IntPoint> IterateBorder()
    {
        IntSlope topSlope = TopLeft.CalculateUnitDirection(TopRight);
        var seen = new HashSet<IntPoint>();
        IntPoint cur = TopLeft;
        bool hitEnd = false;
        while(!hitEnd)
        {
            if(seen.Add(cur))
            {
                yield return cur;
            }

            if(cur == TopRight)
            {
                hitEnd = true;
            }

            cur += topSlope;
        }

        IntSlope rightSlope = TopRight.CalculateUnitDirection(BottomRight);
        cur = TopRight;
        hitEnd = false;

        while(!hitEnd) 
        {
            if (seen.Add(cur))
            {
                yield return cur;
            }

            if (cur == BottomRight)
            {
                hitEnd = true;
            }

            cur += rightSlope;
        }

        IntSlope bottomSlope = BottomRight.CalculateUnitDirection(BottomLeft);
        cur = BottomRight;
        hitEnd = false;

        while (!hitEnd)
        {
            if (seen.Add(cur))
            {
                yield return cur;
            }

            if (cur == BottomLeft)
            {
                hitEnd = true;
            }

            cur += bottomSlope;
        }

        IntSlope leftSlope = BottomLeft.CalculateUnitDirection(TopLeft);
        cur = BottomLeft;
        hitEnd = false;

        while (!hitEnd)
        {
            if (seen.Add(cur))
            {
                yield return cur;
            }

            if (cur == TopLeft)
            {
                hitEnd = true;
            }

            cur += leftSlope;
        }
    }

    private static double CalculateArea(IntPoint p1, IntPoint p2, IntPoint p3)
    {
        return Math.Abs((p1.X * (p2.Y - p3.Y)
                        + p2.X * (p3.Y - p1.Y)
                        + p3.X * (p1.Y - p2.Y)) / 2D);
    }

    public bool ContainsPoint(IntPoint point)
    {
        double a = CalculateArea(TopLeft, TopRight, BottomRight) + CalculateArea(TopLeft, BottomLeft, BottomRight);

        double a1 = CalculateArea(point, TopLeft, TopRight);
        double a2 = CalculateArea(point, TopRight, BottomRight);
        double a3 = CalculateArea(point, BottomRight, BottomLeft);
        double a4 = CalculateArea(point, TopLeft, BottomLeft);

        return a == (a1 + a2 + a3 + a4);
    }
}
