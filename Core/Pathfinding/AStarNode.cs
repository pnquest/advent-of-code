namespace Core.Pathfinding;

public record struct AStarNode(IntPoint Location, int Cost, bool IsPassable);
