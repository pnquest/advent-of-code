using System.Collections.Immutable;

namespace Day072023;

internal class Program
{
    static void Main(string[] args)
    {
        Part1();
        Part2();
    }

    private static void Part2()
    {
        HandWithWild[] hands = File.ReadAllLines("./input.txt")
                            .Select(ParseLineWild)
                            .ToArray();

        long totalWinnings = hands.OrderDescending().Select((h, i) => (long)h.Bid * (hands.Length - i)).Sum();
        Console.WriteLine($"Part 2: {totalWinnings}");
    }

    private static void Part1()
    {
        Hand[] hands = File.ReadAllLines("./input.txt")
                    .Select(ParseLine)
                    .ToArray();

        long totalWinnings = hands.OrderDescending().Select((h, i) => (long)h.Bid * (hands.Length - i)).Sum();
        Console.WriteLine($"Part 1: {totalWinnings}");
    }

    private static HandWithWild ParseLineWild(string line)
    {
        string[] parts = line.Split(' ');
        int bid = int.Parse(parts[1]);

        return new HandWithWild(bid, parts[0].Select(c => new CardWithWild(c)).ToImmutableArray());
    }

    private static Hand ParseLine(string line)
    {
        string[] parts = line.Split(' ');
        int bid = int.Parse(parts[1]);

        return new Hand(bid, parts[0].Select(c => new Card(c)).ToImmutableArray());
    }

    private enum HandTypes
    {
        FiveOfAKind = 7,
        FourOfAKind = 6,
        FullHouse = 5,
        ThreeOfAKind = 4,
        TwoPair = 3,
        Pair = 2,
        HighCard = 1
    }

    private readonly record struct HandWithWild(int Bid, ImmutableArray<CardWithWild> Cards) : IComparable<HandWithWild>
    {
        public int CompareTo(HandWithWild other)
        {
            int rank = CalculateHandType().CompareTo(other.CalculateHandType());

            if (rank != 0)
            {
                return rank;
            }

            for (int i = 0; i < Cards.Length; i++)
            {
                int cardRank = Cards[i].CompareTo(other.Cards[i]);

                if (cardRank != 0)
                {
                    return cardRank;
                }
            }

            return 0;
        }

        public HandTypes CalculateHandType()
        {
            Dictionary<char, int> counts = Cards.GroupBy(g => g).ToDictionary(g => g.Key.Value, g => g.Count());

            if (counts.TryGetValue('J', out int count))
            {
                KeyValuePair<char, int>? biggest = null;
                foreach (KeyValuePair<char, int> pair in counts.Where(v => v.Key != 'J'))
                {
                    if (biggest == null || biggest.Value.Value < pair.Value)
                    {
                        biggest = pair;
                    }
                }

                if (biggest != null)
                {
                    counts[biggest.Value.Key] += count;
                    counts.Remove('J');
                }
            }

            return counts.Values.OrderByDescending(c => c).ToArray() switch {
                [5] => HandTypes.FiveOfAKind,
                [4, 1] => HandTypes.FourOfAKind,
                [3, 2] => HandTypes.FullHouse,
                [3, 1, 1] => HandTypes.ThreeOfAKind,
                [2, 2, 1] => HandTypes.TwoPair,
                [2, ..] => HandTypes.Pair,
                [1, 1, 1, 1, 1] => HandTypes.HighCard,
                _ => throw new InvalidOperationException("Not a valid hand")
            };
        }
    }

    private readonly record struct Hand(int Bid, ImmutableArray<Card> Cards) : IComparable<Hand>
    {
        public int CompareTo(Hand other)
        {
            int rank = CalculateHandType().CompareTo(other.CalculateHandType());

            if (rank != 0)
            {
                return rank;
            }

            for (int i = 0; i < Cards.Length; i++)
            {
                int cardRank = Cards[i].CompareTo(other.Cards[i]);

                if (cardRank != 0)
                {
                    return cardRank;
                }
            }

            return 0;
        }

        public HandTypes CalculateHandType()
        {
            Dictionary<Card, int> counts = Cards.GroupBy(g => g).ToDictionary(g => g.Key, g => g.Count());

            return counts.Values.OrderByDescending(c => c).ToArray() switch {
                [5] => HandTypes.FiveOfAKind,
                [4, 1] => HandTypes.FourOfAKind,
                [3, 2] => HandTypes.FullHouse,
                [3, 1, 1] => HandTypes.ThreeOfAKind,
                [2, 2, 1] => HandTypes.TwoPair,
                [2, ..] => HandTypes.Pair,
                [1, 1, 1, 1, 1] => HandTypes.HighCard,
                _ => throw new InvalidOperationException("Not a valid hand")
            };
        }
    }

    private readonly record struct CardWithWild(char Value) : IComparable<CardWithWild>
    {
        public int Rank => CalculateRank();

        public int CompareTo(CardWithWild other) => Rank.CompareTo(other.Rank);

        private int CalculateRank()
        {
            return Value switch {
                >= '2' and <= '9' => Value - '0',
                'T' => 10,
                'J' => 1,
                'Q' => 12,
                'K' => 13,
                'A' => 14,
                _ => throw new InvalidOperationException($"{Value} is not a valid card value")
            };
        }
    }

    private readonly record struct Card(char Value) : IComparable<Card>
    {
        public int Rank => CalculateRank();

        public int CompareTo(Card other) => Rank.CompareTo(other.Rank);

        private int CalculateRank()
        {
            return Value switch {
                >= '2' and <= '9' => Value - '0',
                'T' => 10,
                'J' => 11,
                'Q' => 12,
                'K' => 13,
                'A' => 14,
                _ => throw new InvalidOperationException($"{Value} is not a valid card value")
            };
        }
    }
}
