namespace Core.Pathfinding;

public class AStarSelection
{
    public AStarNode Node { get; }
    public AStarSelection? Previous { get; set; }
    public int GScore { get; set; }
    public int FScore { get; set; }

    public AStarSelection(AStarNode node, AStarSelection? previous, int gScore, int fScore)
    {
        Node = node;
        Previous = previous;
        GScore = gScore;
        FScore = fScore;
    }


}
