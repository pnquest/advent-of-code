// See https://aka.ms/new-console-template for more information

string[] lines = File.ReadAllLines("./input.txt");

List<int> positions = new List<int>([50]);

int passZero = 0;

foreach (string line in lines)
{
    int cur = positions[^1];

    int change = ComputeDirection(line);

    if(change < 0)
    {
        passZero += -change / 100;
        if(cur != 0 && (-change % 100) >= cur)
        {
            passZero++;
        }

        cur += change;

        if(cur < 0)
        {
            int mult = -cur / 100;
            if (-cur % 100 > 0)
            {
                mult++;
            }
            cur += (mult * 100);
        }
    }
    else
    {
        cur += change;
        passZero += (cur / 100);
        cur %= 100;
    }
    
    positions.Add(cur);
}

Console.WriteLine($"Part 1: {positions.Count(c => c == 0)}");

Console.WriteLine($"Part 2: {passZero}");

static int ComputeDirection(ReadOnlySpan<char> input)
{
    int sign = input[0] switch {
        'R' => 1,
        'L' => -1,
        _ => throw new ArgumentOutOfRangeException(nameof(input), "Must start with R or L")
    };

    return int.Parse(input[1..]) * sign;
}
