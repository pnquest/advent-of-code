namespace Core;

public record struct IntPoint3(int X, int Y, int Z)
{
    public IntPoint3 Transform(int x, int y, int z)
    {
        return new IntPoint3(X + x, Y + y, Z + z);
    }
}
