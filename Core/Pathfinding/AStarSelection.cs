namespace Core.Pathfinding;

public class AStarSelection<T>
{
    public AStarNode<T> Node { get; }
    public AStarSelection<T>? Previous { get; set; }
    public int GScore { get; set; }
    public int FScore { get; set; }

    public AStarSelection(AStarNode<T> node, AStarSelection<T>? previous, int gScore, int fScore)
    {
        Node = node;
        Previous = previous;
        GScore = gScore;
        FScore = fScore;
    }


}
