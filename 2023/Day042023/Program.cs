using System.Collections.Frozen;

namespace Day042023;

internal class Program
{
    static void Main(string[] args)
    {
        ScratchCard[] cards = File.ReadLines("./input.txt")
            .Select(ParseCard)
            .ToArray();
        Part1(cards);

        var cardDict = cards.ToDictionary(d => d.Id, d => new CardCount(d));

        foreach (KeyValuePair<int, CardCount> cardCount in cardDict)
        {
            int matches = cardCount.Value.Card.CountMatches();
            int toScratch = cardCount.Value.Count - cardCount.Value.Scratched;
            if (matches > 0 && toScratch > 0)
            {
                for (int i = 1; i <= matches; i++)
                {
                    cardDict[cardCount.Key + i].Count += toScratch;
                }
            }
            cardCount.Value.Scratched += toScratch;
        }
        int totalCards = cardDict.Values.Sum(v => v.Count);
        Console.WriteLine($"Part 2: {totalCards}");
    }

    private static void Part1(ScratchCard[] cards)
    {
        long totalScore = 0;

        foreach (ScratchCard card in cards)
        {
            totalScore += card.ComputeScore();
        }

        Console.WriteLine($"Part 1: {totalScore}");
    }

    private static ScratchCard ParseCard(string input)
    {
        string[] inputSplit = input.Split(": ");
        int id = int.Parse(inputSplit[0].Split(' ', StringSplitOptions.RemoveEmptyEntries)[1]);

        string[] numberSplit = inputSplit[1].Split(" | ");

        var winners = numberSplit[0].Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToFrozenSet();
        var held = numberSplit[1].Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToFrozenSet();

        return new ScratchCard(id, winners, held);
    }

    private readonly record struct ScratchCard(int Id, FrozenSet<int> WinningNumbers, FrozenSet<int> HeldNumbers)
    {
        public int CountMatches()
        {
            return WinningNumbers.Intersect(HeldNumbers).Count();
        }
        public int ComputeScore()
        {
            int matches = CountMatches();

            return matches switch {
                0 => 0,
                1 => 1,
                _ => (int)Math.Pow(2, matches - 1)
            };
        }
    }

    private class CardCount(ScratchCard card, int count = 1, int scratched = 0)
    {
        public ScratchCard Card { get; } = card;
        public int Count { get; set; } = count;
        public int Scratched { get; set; } = scratched;
    }
}
