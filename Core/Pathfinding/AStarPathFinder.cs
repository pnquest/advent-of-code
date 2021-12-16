namespace Core.Pathfinding;

public class AStarPathFinder
{
    private readonly IntPoint _startPoint;
    private readonly IntPoint _endPoint;
    private readonly AStarNode[][] _map;

    private readonly HashSet<AStarSelection> _openList = new();
    private readonly Dictionary<AStarNode, AStarSelection> _selections = new();

    public AStarPathFinder(AStarNode[][] map, IntPoint startPoint, IntPoint targetPoint)
    {
        _map = map;
        _startPoint = startPoint;
        _endPoint = targetPoint;
    }

    public IEnumerable<AStarSelection> SolvePath()
    {
        var start = new AStarSelection(_map[_startPoint.Y][_startPoint.X], null, 0, _startPoint.CalculateManhattenDistanceTo(_endPoint));
        _openList.Add(start);
        _selections[start.Node] = start;

        while(_openList.Count > 0)
        {
            AStarSelection? curNode = _openList.MinBy(l => l.FScore);

            if (curNode == null)
            {
                throw new InvalidOperationException("Something went wrong");
            }

            _openList.Remove(curNode);
            

            if(curNode.Node.Location == _endPoint)
            {
                AStarSelection? cur = _selections[curNode.Node];

                do
                {
                    yield return cur;
                    cur = cur?.Previous;
                } while (cur != null);
            }

            foreach(IntPoint neighbor in curNode.Node.Location.GetNeighbors(0, _map[0].Length - 1, 0, _map.Length - 1))
            {
                ScoreAndAddNeighbors(curNode, neighbor);
            }
        }
    }

    private void ScoreAndAddNeighbors(AStarSelection? curNode, IntPoint neighbor)
    {
        ArgumentNullException.ThrowIfNull(curNode);
        AStarNode node = _map[neighbor.Y][neighbor.X];

        if (node.IsPassable)
        {
            if (_selections.TryGetValue(node, out AStarSelection? selection) && selection != null)
            {
                int gScore = curNode.GScore + node.Cost;
                if (gScore < selection.GScore)
                {
                    selection.GScore = gScore;
                    selection.FScore = gScore + selection.Node.Location.CalculateManhattenDistanceTo(_endPoint);
                    selection.Previous = curNode;

                    _openList.Add(selection);
                }
            }
            else
            {
                int gScore = curNode.GScore + node.Cost;
                AStarSelection sel = new(node, curNode, gScore, gScore + node.Location.CalculateManhattenDistanceTo(_endPoint));
                _openList.Add(sel);
                _selections[node] = sel;
            }
        }
    }
}
