namespace Core.Pathfinding;

public class AStarPathFinder<T>
{
    private readonly IntPoint _startPoint;
    private readonly IntPoint _endPoint;
    private readonly AStarNode<T>[][] _map;
    private readonly Func<AStarNode<T>, AStarNode<T>, IntPoint, int> _costEstimate;
    private readonly Func<AStarNode<T>, AStarNode<T>, IntPoint, int> _actualCost;
    Func<AStarNode<T>, AStarNode<T>, bool> _isPassable;

    private readonly HashSet<AStarSelection<T>> _openList = [];
    private readonly Dictionary<AStarNode<T>, AStarSelection<T>> _selections = [];

    public AStarPathFinder(AStarNode<T>[][] map,
                           IntPoint startPoint,
                           IntPoint targetPoint,
                           Func<AStarNode<T>, AStarNode<T>, IntPoint, int>? costEstimateFunc = null,
                           Func<AStarNode<T>, AStarNode<T>, IntPoint, int>? costFunc = null,
                           Func<AStarNode<T>, AStarNode<T>, bool>? passableFunc = null)
    {
        _map = map;
        _startPoint = startPoint;
        _endPoint = targetPoint;
        _costEstimate = costEstimateFunc ?? ((frm, to, tgt) => frm.Location.CalculateManhattenDistanceTo(tgt));
        _actualCost = costFunc ?? ((_, to, _) => to.Cost);
        _isPassable = passableFunc ?? ((_, to) => to.IsPassable);
    }

    public IEnumerable<AStarSelection<T>> SolvePath()
    {
        var start = new AStarSelection<T>(_map[_startPoint.Y][_startPoint.X], null, 0, _startPoint.CalculateManhattenDistanceTo(_endPoint));
        _openList.Add(start);
        _selections[start.Node] = start;

        while (_openList.Count > 0)
        {
            AStarSelection<T>? curNode = _openList.MinBy(l => l.FScore);

            if (curNode == null)
            {
                throw new InvalidOperationException("Something went wrong");
            }

            _openList.Remove(curNode);


            if (curNode.Node.Location == _endPoint)
            {
                AStarSelection<T>? cur = _selections[curNode.Node];

                do
                {
                    yield return cur;
                    cur = cur?.Previous;
                } while (cur != null);

                yield break;
            }

            foreach (IntPoint neighbor in curNode.Node.Location.GetNeighbors(0, _map[0].Length - 1, 0, _map.Length - 1))
            {
                ScoreAndAddNeighbors(curNode, neighbor);
            }
        }
    }

    private void ScoreAndAddNeighbors(AStarSelection<T>? curNode, IntPoint neighbor)
    {
        ArgumentNullException.ThrowIfNull(curNode);
        AStarNode<T> node = _map[neighbor.Y][neighbor.X];

        if (_isPassable(curNode.Node, node))
        {
            if (_selections.TryGetValue(node, out AStarSelection<T>? selection) && selection != null)
            {
                int gScore = curNode.GScore + _actualCost(curNode.Node, node, _endPoint);
                if (gScore < selection.GScore)
                {
                    selection.GScore = gScore;
                    selection.FScore = gScore + _costEstimate(curNode.Node, node, _endPoint);
                    selection.Previous = curNode;

                    _openList.Add(selection);
                }
            }
            else
            {
                int gScore = curNode.GScore + _actualCost(curNode.Node, node, _endPoint);
                AStarSelection<T> sel = new(node, curNode, gScore, gScore + _costEstimate(curNode.Node, node, _endPoint));
                _openList.Add(sel);
                _selections[node] = sel;
            }
        }
    }
}
