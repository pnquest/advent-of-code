namespace Day21;

public static class Program
{
    public static void Main()
    {
        Part1();
        Part2();
    }

    private static void Part2()
    {
        Dictionary<GameState, decimal> gameStateDictionary = new() {
            [new GameState(9, 5, 0, 0)] = 1
        };

        decimal p1Wins = 0;
        decimal p2Wins = 0;

        int playerTurn = 0;

        while (gameStateDictionary.Count > 0)
        {
            Dictionary<GameState, decimal> updatedState = RunTurn(gameStateDictionary, ref p1Wins, ref p2Wins, playerTurn);

            playerTurn = (playerTurn + 1) % 2;
            gameStateDictionary = updatedState;
        }

        Console.WriteLine($"Part 2: {Math.Max(p1Wins, p2Wins)}");
    }

    private static Dictionary<GameState, decimal> RunTurn(Dictionary<GameState, decimal> gameStateDictionary, ref decimal p1Wins, ref decimal p2Wins, int playerTurn)
    {
        Dictionary<GameState, decimal> updatedState = [];

        foreach (KeyValuePair<GameState, decimal> stateSet in gameStateDictionary)
        {
            if (playerTurn == 0)
            {
                p1Wins = CreatePlayer1States(p1Wins, updatedState, stateSet);
            }
            else
            {
                p2Wins = CreatePlayer2States(p2Wins, updatedState, stateSet);
            }
        }

        return updatedState;
    }

    private static decimal CreatePlayer2States(decimal p2Wins, Dictionary<GameState, decimal> updatedState, KeyValuePair<GameState, decimal> stateSet)
    {
        foreach (GameState state in stateSet.Key.Player2Rolls())
        {
            if (state.Player2Score >= 21)
            {
                p2Wins += stateSet.Value;
            }
            else
            {
                updatedState.SetOrIncrement(state, stateSet.Value);
            }
        }

        return p2Wins;
    }

    private static decimal CreatePlayer1States(decimal p1Wins, Dictionary<GameState, decimal> updatedState, KeyValuePair<GameState, decimal> stateSet)
    {
        foreach (GameState state in stateSet.Key.Player1Rolls())
        {
            if (state.Player1Score >= 21)
            {
                p1Wins += stateSet.Value;
            }
            else
            {
                updatedState.SetOrIncrement(state, stateSet.Value);
            }
        }

        return p1Wins;
    }

    private static void Part1()
    {
        int numRolls = 0;

        int nextNumber = 1;

        int curPlayer = 0;
        int[] playerPositions = new[] { 9, 5 };
        int[] playerScores = new[] { 0, 0 };

        while (playerScores.All(s => s < 1000))
        {
            int playerRoll = Roll3Times(ref nextNumber);
            numRolls += 3;

            playerPositions[curPlayer] = (playerPositions[curPlayer] + playerRoll) % 10;
            playerScores[curPlayer] += (playerPositions[curPlayer] + 1);

            curPlayer = (curPlayer + 1) % 2;
        }

        Console.WriteLine($"Part 1: {playerScores.Min() * numRolls}");
    }

    private static int Roll3Times(ref int nextNumber)
    {
        int result = 0;

        for (int i = 0; i < 3; i++)
        {
            result += nextNumber++;

            if (nextNumber > 100)
            {
                nextNumber = 1;
            }
        }

        return result;
    }
}

internal record struct GameState(int Player1Position, int Player2Position, int Player1Score, int Player2Score)
{
    public IEnumerable<GameState> Player1Rolls()
    {
        for (int d1 = 1; d1 <= 3; d1++)
        {
            for (int d2 = 1; d2 <= 3; d2++)
            {
                for (int d3 = 1; d3 <= 3; d3++)
                {
                    int roll = d1 + d2 + d3;
                    int position = (Player1Position + roll) % 10;
                    int score = Player1Score + position + 1;
                    yield return this with { Player1Position = position, Player1Score = score };
                }
            }
        }
    }

    public IEnumerable<GameState> Player2Rolls()
    {
        for (int d1 = 1; d1 <= 3; d1++)
        {
            for (int d2 = 1; d2 <= 3; d2++)
            {
                for (int d3 = 1; d3 <= 3; d3++)
                {
                    int roll = d1 + d2 + d3;
                    int position = (Player2Position + roll) % 10;
                    int score = Player2Score + position + 1;
                    yield return this with { Player2Position = position, Player2Score = score };
                }
            }
        }
    }
}
