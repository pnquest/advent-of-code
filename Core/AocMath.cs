using System.Numerics;

namespace Core;
public static class AocMath
{
    public static T CalculateGreatestCommonFactor<T>(T a, T b)
        where T: INumber<T>
    {
        while(b != T.Zero)
        {
            T temp = b;
            b = a % b;
            a = temp;
        }

        return a;
    }

    public static T CalculateGreatestCommonFactor<T>(params T[] values)
        where T : INumber<T>
    {
        T current = values[0];
        for(int i = 1; i < values.Length; i++)
        {
            current = CalculateGreatestCommonFactor(current, values[i]);
        }

        return current;
    }

    public static T CalculateLeastCommonMultiple<T>(T a, T b)
        where T: INumber<T>
    {
        if(a == T.Zero || b == T.Zero)
        {
            return T.Zero;
        }

        return a / CalculateGreatestCommonFactor(a, b) * b;
    }

    public static T CalculateLeastCommonMultiple<T>(params T[] values)
        where T : INumber<T>
    {
        if(values.Length == 1)
        {
            return values[0];
        }

        T curRresult = values[0];
        for(int i = 1; i < values.Length; i++)
        {
            curRresult = CalculateLeastCommonMultiple(curRresult, values[i]);
        }

        return curRresult;
    }
}
