namespace Core;

public static class AoCConversions
{
    public static char[] ConvertHexStringToBinary(string hexString)
    {
        List<char> converted = [];
        foreach (char h in hexString)
        {
            char[] toAppend = h switch {
                '0' => ['0', '0', '0', '0'],
                '1' => ['0', '0', '0', '1'],
                '2' => ['0', '0', '1', '0'],
                '3' => ['0', '0', '1', '1'],
                '4' => ['0', '1', '0', '0'],
                '5' => ['0', '1', '0', '1'],
                '6' => ['0', '1', '1', '0'],
                '7' => ['0', '1', '1', '1'],
                '8' => ['1', '0', '0', '0'],
                '9' => ['1', '0', '0', '1'],
                'A' => ['1', '0', '1', '0'],
                'B' => ['1', '0', '1', '1'],
                'C' => ['1', '1', '0', '0'],
                'D' => ['1', '1', '0', '1'],
                'E' => ['1', '1', '1', '0'],
                'F' => ['1', '1', '1', '1'],
                _ => throw new InvalidOperationException("None of these are hex")
            };
            converted.AddRange(toAppend);
        }

        return [.. converted];
    }
}
