namespace Core;

public static class AoCConversions
{
    public static char[] ConvertHexStringToBinary(string hexString)
    {
        List<char> converted = new List<char>();
        foreach (char h in hexString)
        {
            char[] toAppend = h switch {
                '0' => new[] { '0', '0', '0', '0' },
                '1' => new[] { '0', '0', '0', '1' },
                '2' => new[] { '0', '0', '1', '0' },
                '3' => new[] { '0', '0', '1', '1' },
                '4' => new[] { '0', '1', '0', '0' },
                '5' => new[] { '0', '1', '0', '1' },
                '6' => new[] { '0', '1', '1', '0' },
                '7' => new[] { '0', '1', '1', '1' },
                '8' => new[] { '1', '0', '0', '0' },
                '9' => new[] { '1', '0', '0', '1' },
                'A' => new[] { '1', '0', '1', '0' },
                'B' => new[] { '1', '0', '1', '1' },
                'C' => new[] { '1', '1', '0', '0' },
                'D' => new[] { '1', '1', '0', '1' },
                'E' => new[] { '1', '1', '1', '0' },
                'F' => new[] { '1', '1', '1', '1' },
                _ => throw new InvalidOperationException("None of these are hex")
            };
            converted.AddRange(toAppend);
        }

        return converted.ToArray();
    }
}
