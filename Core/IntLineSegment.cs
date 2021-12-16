namespace Core;

public record struct IntLineSegment(IntPoint P1, IntPoint P2)
{
    /// <summary>
    /// Gets all the points of overlap or intersection between this line segment and the other.
    /// </summary>
    /// <remarks>These are built assuming the lines are not continuous. So any point of intersection that is between 2 integer points will be discareded</remarks>
    /// <param name="other">The other line segment to check</param>
    /// <returns>All integer points the lines have in common</returns>
    public IEnumerable<IntPoint> GetCommonPoints(IntLineSegment other)
    {
        IntPoint.Orientation o1 = IntPoint.GetOrientation(P1, P2, other.P1);
        IntPoint.Orientation o2 = IntPoint.GetOrientation(P1, P2, other.P2);
        IntPoint.Orientation o3 = IntPoint.GetOrientation(other.P1, other.P2, P1);
        IntPoint.Orientation o4 = IntPoint.GetOrientation(other.P1, other.P2, P2);

        if (o1 != o2 && o3 != o4)
        {
            IntPoint? intersect = GetIntersect(other);
            if (intersect != null)
            {
                yield return intersect.Value;
            }
        }
        else if (o1 == IntPoint.Orientation.Colinear && IsOnSegment(other.P1))
        {
            IntSlope slp = other.GetSlope();
            IntPoint? cur = null;
            do
            {
                if (cur == null)
                {
                    cur = other.P1;
                }
                else
                {
                    cur = cur + slp;
                }

                if (IsOnSegment(cur.Value))
                {
                    yield return cur.Value;
                }
                else
                {
                    yield break;
                }
            } while (cur != other.P2);
        }
        else if (o2 == IntPoint.Orientation.Colinear && IsOnSegment(other.P2))
        {
            IntSlope slp = other.GetSlope();
            IntPoint? cur = null;
            do
            {
                if (cur == null)
                {
                    cur = other.P2;
                }
                else
                {
                    cur = cur - slp;
                }

                if (IsOnSegment(cur.Value))
                {
                    yield return cur.Value;
                }
                else
                {
                    yield break;
                }
            } while (cur != other.P1);
        }
        else if (o3 == IntPoint.Orientation.Colinear && other.IsOnSegment(P1))
        {
            IntSlope slp = GetSlope();
            IntPoint? cur = null;
            do
            {
                if (cur == null)
                {
                    cur = P1;
                }
                else
                {
                    cur = cur + slp;
                }

                if (IsOnSegment(cur.Value))
                {
                    yield return cur.Value;
                }
                else
                {
                    yield break;
                }
            } while (cur != P2);
        }
        else if (o4 == IntPoint.Orientation.Colinear && other.IsOnSegment(P2))
        {
            IntSlope slp = GetSlope();
            IntPoint? cur = null;
            do
            {
                if (cur == null)
                {
                    cur = P2;
                }
                else
                {
                    cur = cur - slp;
                }

                if (IsOnSegment(cur.Value))
                {
                    yield return cur.Value;
                }
                else
                {
                    yield break;
                }
            } while (cur != P1);
        }
    }

    public IntSlope GetSlope()
    {
        int tempX = P2.X - P1.X;
        int tempY = P2.Y - P1.Y;

        if (tempX == 0)
        {
            return new IntSlope(0, tempY > 0 ? 1 : -1);
        }

        if (tempY == 0)
        {
            return new IntSlope(tempX > 0 ? 1 : -1, 0);
        }

        while (tempX % 2 == 0 && tempY % 2 == 0)
        {
            tempX /= 2;
            tempY /= 2;
        }

        if (tempX % tempY == 0)
        {
            tempX /= Math.Abs(tempY);
            tempY /= Math.Abs(tempY);
        }
        else if (tempY % tempX == 0)
        {
            tempX /= Math.Abs(tempX);
            tempY /= Math.Abs(tempX);
        }

        return new IntSlope(tempX, tempY);
    }

    public bool IsOnSegment(IntPoint point)
    {
        return point.X <= Math.Max(P1.X, P2.X)
            && point.X >= Math.Min(P1.X, P2.X)
            && point.Y <= Math.Max(P1.Y, P2.Y)
            && point.Y >= Math.Min(P1.Y, P2.Y);
    }

    private IntPoint? GetIntersect(IntLineSegment other)
    {
        int det = CalculateDeterminant(other, out int otherA, out int otherB, out int otherC, out int thisA, out int thisB, out int thisC);

        if (det == 0)
        {
            return null;
        }

        int intX = (otherB * thisC - thisB * otherC) / det;
        int intY = (thisA * otherC - otherA * thisC) / det;

        var canditdate = new IntPoint(intX, intY);

        //we need to re-check this because we are using integers. The actual point could be x.5, y.34
        //which would get truncated to x,y.
        //So we need to fully check again that the result point is colinear with and within the range of both lines.

        if (IntPoint.GetOrientation(P1, P2, canditdate) == IntPoint.Orientation.Colinear
            && IsOnSegment(canditdate)
            && IntPoint.GetOrientation(other.P1, other.P2, canditdate) == IntPoint.Orientation.Colinear
            && other.IsOnSegment(canditdate))
        {
            return new IntPoint(intX, intY);
        }

        return null;
    }

    private int CalculateDeterminant(IntLineSegment other, out int otherA, out int otherB, out int otherC, out int thisA, out int thisB, out int thisC)
    {
        otherA = other.P2.Y - other.P1.Y;
        otherB = other.P1.X - other.P2.X;
        otherC = otherA * other.P1.X + otherB * other.P1.Y;
        thisA = P2.Y - P1.Y;
        thisB = P1.X - P2.X;
        thisC = thisA * P1.X + thisB * P1.Y;
        return thisA * otherB - otherA * thisB;
    }

}