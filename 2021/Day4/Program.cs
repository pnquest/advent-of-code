namespace Day4;

public static class Program
{
    public static void Main()
    {
        Part1();
        Part2();
    }

    private static void Part2()
    {
        LoadPicksAndBoards(out Queue<int> picks, out List<Board> boards);

        while (picks.Count > 0)
        {
            int pick = picks.Dequeue();

            if (boards.Count > 1)
            {
                int winningBoards = boards.RemoveAll(b => b.MarkNumberAndCheckVictory(pick));

                if (winningBoards > 0)
                {
                    Console.WriteLine($"Removed {winningBoards} boards");
                }
            }
            else if (boards[0].MarkNumberAndCheckVictory(pick))
            {
                Console.WriteLine($"Part 2: {boards[0].ComputeScore()}");
                return;
            }
        }
    }

    private static void Part1()
    {
        LoadPicksAndBoards(out Queue<int> picks, out List<Board> boards);

        while (picks.Count > 0)
        {
            int pick = picks.Dequeue();

            foreach (Board board in boards)
            {
                if (board.MarkNumberAndCheckVictory(pick))
                {
                    Console.WriteLine($"Part 1: {board.ComputeScore()}");
                    return;
                }
            }
        }
    }

    private static void LoadPicksAndBoards(out Queue<int> picks, out List<Board> boards)
    {
        string[] lines = File.ReadAllLines("./input.txt");

        picks = new Queue<int>(lines[0].Split(',').Select(int.Parse));
        IEnumerable<string[]>? boardsRaw = lines.Skip(2).Where(l => l.Length > 0).Chunk(5);

        boards = new();
        foreach (string[]? boardRaw in boardsRaw)
        {
            var board = new Board();

            IEnumerable<int>? values = boardRaw
                .Select(l => l.Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(int.Parse))
                .SelectMany(l => l);

            foreach (int value in values)
            {
                board.AddBoardPosition(value);
            }

            boards.Add(board);
        }
    }

    internal record struct BoardPosition(int XPosition, int YPosition);

    internal class BoardSpace
    {
        public int Value { get; }
        public bool IsPicked { get; set; }

        public BoardSpace(int value)
        {
            Value = value;
        }
    }

    internal class Board
    {
        private readonly BoardSpace[][] _board;
        private readonly Dictionary<int, BoardPosition> _spaceDictionary = new();

        private int _curX = 0;
        private int _curY = 0;

        private int? _lastMarked = null;

        public Board()
        {
            _board = new BoardSpace[5][];

            for (int i = 0; i < 5; i++)
            {
                _board[i] = new BoardSpace[5];
            }
        }

        public bool MarkNumberAndCheckVictory(int value)
        {
            if (!_spaceDictionary.TryGetValue(value, out BoardPosition position))
            {
                return false;
            }

            _lastMarked = value;

            _board[position.XPosition][position.YPosition].IsPicked = true;

            if (_board[position.XPosition].All(position => position.IsPicked) ||
                Enumerable.Range(0, 5).Select(r => _board[r][position.YPosition]).All(p => p.IsPicked))
            {
                return true;
            }

            return false;
        }

        public int ComputeScore()
        {
            int unmarkedSum = _board
                .SelectMany(b => b)
                .Where(s => !s.IsPicked)
                .Sum(s => s.Value);

            ArgumentNullException.ThrowIfNull(_lastMarked);

            return unmarkedSum * _lastMarked.Value;
        }

        public void AddBoardPosition(int value)
        {
            var space = new BoardSpace(value);
            _board[_curX][_curY] = space;
            _spaceDictionary[value] = new BoardPosition(_curX, _curY);

            if (_curX == 4)
            {
                _curX = 0;
                _curY++;
            }
            else
            {
                _curX++;
            }
        }
    }
}