namespace Day18;

public static class Program
{
    public static void Main()
    {
        SnailNumber[] numbers = File.ReadAllLines("./input.txt").Select(s => SnailNumber.Parse(s, out _)).ToArray();
        Part1(numbers);

        long maxValue = 0;

        for (int i = 0; i < numbers.Length; i++)
        {
            for(int j = 0; j < numbers.Length; j++)
            {
                if(i != j)
                {
                    long? mag = (numbers[i] + numbers[j]).GetMagnitude();
                    if(mag > maxValue)
                    {
                        maxValue = mag.Value;
                    }
                }
            }
        }

        Console.WriteLine($"Part 2: {maxValue}");
    }

    private static void Part1(SnailNumber[] numbers)
    {
        SnailNumber cur = numbers[0];
        for (int i = 1; i < numbers.Length; i++)
        {
            cur += numbers[i];
        }
        Console.WriteLine($"Part 1: {cur.GetMagnitude()}");
    }

    internal class SnailNumber
    {
        public int SingleValue { get; private set; }
        public SnailNumber? Left { get; private set; } 
        public SnailNumber? Right { get; private set; }
        public SnailNumber? Parent { get; private set; }
        public bool IsSingleValue => Left == null || Right == null;
        public bool IsPair => !IsSingleValue;

        public static SnailNumber Parse(ReadOnlySpan<char> input, out int used)
        {
            SnailNumber number = new();
            if (input[0] != '[') //this is a single value
            {
                int commaIndex = input.IndexOf(',');
                int closeIndex = input.IndexOf(']');

                int indexToUse = (commaIndex, closeIndex) switch {
                    (-1, _) => closeIndex,
                    (_, -1) => commaIndex,
                    _ => Math.Min(commaIndex, closeIndex)
                };

                number.SingleValue = int.Parse(input[..indexToUse]);
                used = indexToUse + 1;
                return number;
            }

            //otherwise its a pair
            number.Left = Parse(input[1..], out int usedLeft);
            number.Left.Parent = number;
            number.Right = Parse(input[(usedLeft + 1)..], out int usedRight);
            number.Right.Parent = number;
            used = usedLeft + usedRight + 2;
            return number;
        }

        public SnailNumber Clone()
        {
            return Clone(null);
        }

        private SnailNumber Clone(SnailNumber? parent)
        {
            if(IsSingleValue)
            {
                return new SnailNumber { SingleValue = SingleValue, Parent = parent };
            }

            var newNumber = new SnailNumber();
            newNumber.Left = Left?.Clone(newNumber);
            newNumber.Right = Right?.Clone(newNumber);
            newNumber.Parent = parent;
            return newNumber;
        }

        private static void Reduce(SnailNumber number)
        {
            bool didEither;

            do
            {
                didEither = false;

                if (Explode(number))
                {
                    didEither = true;
                    continue;
                }

                if (Split(number))
                {
                    didEither = true;
                }

            } while (didEither);
        }

        public static bool Split(SnailNumber number)
        {
            if (number.IsSingleValue && number.SingleValue >= 10 && number.Parent != null)
            {
                int left = (int)Math.Floor((double)number.SingleValue / 2);
                int right = (int)Math.Ceiling((double)number.SingleValue / 2);

                SnailNumber outer = new() { Parent = number.Parent };
                outer.Left = new SnailNumber { SingleValue = left, Parent = outer };
                outer.Right = new SnailNumber { SingleValue = right, Parent = outer };

                if (number.Parent.Left == number)
                {
                    number.Parent.Left = outer;
                }
                else if (number.Parent.Right == number)
                {
                    number.Parent.Right = outer;
                }

                return true;
            }

            if ((number.Left != null && Split(number.Left)) || (number.Right != null && Split(number.Right)))
            {
                return true;
            }

            return false;
        }

        public static bool Explode(SnailNumber number, int level = 0)
        {
            if (number.IsPair && level == 4 && (number.Left?.IsSingleValue ?? false) && (number.Right?.IsSingleValue ?? false))
            {
                SnailNumber? leftSingle = FindSingleValueToLeft(number);
                if (leftSingle != null)
                {
                    leftSingle.SingleValue += number.Left.SingleValue;
                }

                SnailNumber? rightSingle = FindSingleValueToRight(number);
                if (rightSingle != null)
                {
                    rightSingle.SingleValue += number.Right.SingleValue;
                }

                if (ReferenceEquals(number, number.Parent?.Left))
                {
                    number.Parent.Left = new SnailNumber { SingleValue = 0, Parent = number.Parent };
                }
                else if (ReferenceEquals(number, number.Parent?.Right))
                {
                    number.Parent.Right = new SnailNumber { SingleValue = 0, Parent = number.Parent };
                }

                return true;
            }
            else if (number.Left != null && Explode(number.Left, level + 1))
            {
                return true;
            }
            else if (number.Right != null && Explode(number.Right, level + 1))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private static SnailNumber? FindSingleValueToLeft(SnailNumber number)
        {
            if (number.Parent?.Left != null && number == number.Parent?.Right)
            {
                SnailNumber? left = FindChildSingleValuePreferRight(number.Parent.Left);
                if (left != null)
                {
                    return left;
                }
            }
            if (number.Parent != null)
            {
                return FindSingleValueToLeft(number.Parent);
            }

            return null;
        }

        private static SnailNumber? FindSingleValueToRight(SnailNumber number)
        {
            if (number.Parent?.Right != null && number == number.Parent?.Left)
            {
                SnailNumber? right = FindChildSingleValuePreferLeft(number.Parent.Right);
                if (right != null)
                {
                    return right;
                }
            }
            if (number.Parent != null)
            {
                return FindSingleValueToRight(number.Parent);
            }

            return null;
        }

        private static SnailNumber? FindChildSingleValuePreferLeft(SnailNumber parent)
        {
            if (parent.IsSingleValue)
            {
                return parent;
            }
            if (parent.Left != null)
            {
                return FindChildSingleValuePreferLeft(parent.Left);
            }
            if (parent.Right != null)
            {
                return FindChildSingleValuePreferLeft(parent.Right);
            }

            return null;
        }

        private static SnailNumber? FindChildSingleValuePreferRight(SnailNumber parent)
        {
            if (parent.IsSingleValue)
            {
                return parent;
            }
            if (parent.Right != null)
            {
                return FindChildSingleValuePreferRight(parent.Right);
            }
            if (parent.Left != null)
            {
                return FindChildSingleValuePreferRight(parent.Left);
            }

            return null;
        }

        public static SnailNumber operator +(SnailNumber left, SnailNumber right)
        {
            var summed = new SnailNumber();
            SnailNumber leftCloned = left.Clone();
            SnailNumber rightConed = right.Clone();
            leftCloned.Parent = summed;
            summed.Left = leftCloned;
            rightConed.Parent = summed;
            summed.Right = rightConed;
            Reduce(summed);
            return summed;
        }

        public override string ToString()
        {
            if (IsSingleValue)
            {
                return SingleValue.ToString();
            }

            return $"[{Left},{Right}]";
        }

        public long? GetMagnitude()
        {
            if (IsSingleValue)
            {
                return SingleValue;
            }

            return Left?.GetMagnitude() * 3 + Right?.GetMagnitude() * 2;
        }
    }
}