namespace Core;

public record struct LineSegment(Point P1, Point P2)
{
    public IEnumerable<Point> GetCommonPoints(LineSegment other)
    {
        Point.Orientation o1 = Point.GetOrientation(P1, P2, other.P1);
        Point.Orientation o2 = Point.GetOrientation(P1, P2, other.P2);
        Point.Orientation o3 = Point.GetOrientation(other.P1, other.P2, P1);
        Point.Orientation o4 = Point.GetOrientation(other.P1, other.P2, P2);

        if(o1 != o2 && o3 != o4)
        {
            Point? intersect = GetIntersect(other);
            if(intersect != null)
            {
                yield return intersect.Value;
            }
        }
        else if(o1 == Point.Orientation.Colinear && IsOnSegment(other.P1))
        {
            Slope slp = other.GetSlope();
            Point? cur = null;
            do
            {
                if(cur== null)
                {
                    cur = other.P1;
                }
                else
                {
                    cur = cur + slp;
                }

                if(IsOnSegment(cur.Value))
                {
                    yield return cur.Value;
                }
                else
                {
                    yield break;
                }
            } while (cur != other.P2);
        }
        else if(o2 == Point.Orientation.Colinear && IsOnSegment(other.P2))
        {
            Slope slp = other.GetSlope();
            Point? cur = null;
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
        else if(o3 == Point.Orientation.Colinear && other.IsOnSegment(P1))
        {
            Slope slp = GetSlope();
            Point? cur = null;
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
        else if(o4 == Point.Orientation.Colinear && other.IsOnSegment(P2))
        {
            Slope slp = GetSlope();
            Point? cur = null;
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

    public Slope GetSlope()
    {
        int tempX = P2.X - P1.X;
        int tempY = P2.Y - P1.Y;

        if(tempX == 0 )
        {
            return new Slope(0, tempY > 0 ? 1 : -1);
        }

        if(tempY == 0)
        {
            return new Slope(tempX > 0 ? 1 : -1, 0);
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

        return new Slope(tempX, tempY);
    }

    public bool IsOnSegment(Point point)
    {
        return point.X <= Math.Max(P1.X, P2.X)
            && point.X >= Math.Min(P1.X, P2.X)
            && point.Y <= Math.Max(P1.Y, P2.Y)
            && point.Y >= Math.Min(P1.Y, P2.Y);
    }

    private Point? GetIntersect(LineSegment other)
    {
        int otherA, otherB, otherC, thisA, thisB, thisC, det;
        det = CalculateDeterminant(other, out otherA, out otherB, out otherC, out thisA, out thisB, out thisC);

        if (det == 0)
        {
            return null;
        }

        int intX = (otherB * thisC - thisB * otherC) / det;
        int intY = (thisA * otherC - otherA * thisC) / det;

        if (Math.Min(P1.X, P2.X) <= intX
            && Math.Max(P1.X, P2.X) >= intX
            && Math.Min(P1.Y, P2.Y) <= intY
            && Math.Max(P1.Y, P2.Y) >= intY)
        {
            return new Point(intX, intY);
        }

        return null;
    }

    private int CalculateDeterminant(LineSegment other, out int otherA, out int otherB, out int otherC, out int thisA, out int thisB, out int thisC)
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