using Core;

namespace Day20;

public static class Program
{
    public static void Main()
    {
        Part1();
        int count = CountLights(50);
        Console.WriteLine($"Part 2: {count}");
    }

    private static void Part1()
    {
        int count = CountLights(2);
        Console.WriteLine($"Part 1: {count}");
    }

    private static int CountLights(int steps)
    {
        string[] lines = File.ReadAllLines("./input.txt");

        bool[] algo = lines[0].Select(c => c == '#').ToArray();

        bool[][] image = lines.Skip(2).Select(l => l.Select(c => c == '#').ToArray()).ToArray();

        bool infiniteAlternating = algo[0] && !algo[^1];

        for (int i = 0; i < steps; i++)
        {
            bool defaultValue = infiniteAlternating && i % 2 != 0;
            image = PadImage(image, defaultValue);

            bool[][] mappedImage = new bool[image.Length][];

            for (int y = 0; y < image.Length; y++)
            {
                mappedImage[y] = new bool[image[y].Length];
                for (int x = 0; x < image[y].Length; x++)
                {
                    IntPoint pt = new IntPoint(x, y);

                    int index = 0;
                    int numPosition = 9;
                    foreach (IntPoint neighbor in pt.GetRegion(int.MinValue, int.MaxValue, int.MinValue, int.MaxValue, true).OrderBy(p => p.Y).ThenBy(p => p.X))
                    {
                        numPosition--;
                        if (neighbor.X >= 0 && neighbor.X < image[0].Length && neighbor.Y >= 0 && neighbor.Y < image.Length)
                        {
                            index += (image[neighbor.Y][neighbor.X] ? 1 : 0) * (int)Math.Pow(2, numPosition);
                        }
                        else
                        {
                            index += (defaultValue ? 1 : 0) * (int)Math.Pow(2, numPosition);
                        }
                    }

                    mappedImage[y][x] = algo[index];
                }
            }

            image = mappedImage;
        }

        int count = image.SelectMany(s => s).Count(b => b);
        return count;
    }

    private static bool[][] PadImage(bool[][] image, bool paddingValue)
    {
        bool[][] newImage = new bool[image.Length + 2][];
        for (int y = 0; y < newImage.Length; y++)
        {
            newImage[y] = new bool[image[0].Length + 2];
            if(paddingValue)
            {
                for(int i = 0; i < newImage[y].Length; i++)
                {
                    newImage[y][i] = true;
                }
            }
            if(y - 1 >= 0 && y - 1 < image.Length)
            {
                Array.ConstrainedCopy(image[y - 1], 0, newImage[y], 1, image[y - 1].Length);
            }
        }

        return newImage;
    }
}
