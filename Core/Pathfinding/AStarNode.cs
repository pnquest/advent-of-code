namespace Core.Pathfinding;

public record AStarNode<T>(IntPoint Location, int Cost, bool IsPassable, T ExtraData);
