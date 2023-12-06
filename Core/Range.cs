using System.Numerics;

namespace Core;

public readonly record struct Range<T>(T Start, T End) where T: INumber<T>
{
    public Range<T>? GetOverlap(Range<T> other)
    {
        //part of this is between other on the left
        if(Start <= other.Start && End >= other.Start && End <= other.End)
        {
            return new Range<T>(other.Start, End);
        }

        //this completely contains other
        if(Start <= other.Start && End >= other.End)
        {
            return other;
        }

        // other completely contains this
        if(Start >= other.Start && End <= other.End)
        {
            return this;
        }

        //part of this is between other on the right
        if(Start >= other.Start && Start <= other.End && End >= other.End)
        {
            return new Range<T>(Start, other.End);
        }

        return null;
    }

    public IEnumerable<Range<T>> RemoveRange(Range<T> other)
    {
        if(other.Start < Start || other.End > End)
        {
            throw new ArgumentOutOfRangeException("This range must wholy contain other", nameof(other));
        }

        if(Start == other.Start && End == other.End)
        {
            yield break;
        }

        if(Start == other.Start) // the remainder is entirely on the right
        {
            yield return new Range<T>(other.End + T.One, End);
        }
        else if(End == other.End) // the remainder is entirely on the left
        {
            yield return new Range<T>(Start, other.Start - T.One);
        }
        else //the remainder is on both th eleft and the right
        {
            yield return new Range<T>(Start, other.Start - T.One);
            yield return new Range<T>(other.End + T.One, End);
        }
    }
}
