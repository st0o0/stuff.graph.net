using stuff.graph.algorithms.net;
using stuff.graph.net;

namespace stuff.graph.astar.net;

public record AStarSettings(Func<Heuristic.HeuristicArgs, double> Heuristic) : IPathfinderSettings;

public static class Heuristic
{
    public record HeuristicArgs(INode A, INode B)
    {
        public static implicit operator Tuple<INode, INode>(HeuristicArgs args) => Tuple.Create(args.A, args.B);
        public static implicit operator HeuristicArgs(Tuple<INode, INode> args) => new(args.Item1, args.Item2);
    };

    public static double MaxDXDYDZ(HeuristicArgs args)
    {
        var aLocation = args.A.Location;
        var bLocation = args.B.Location;
        var x = Math.Abs(aLocation.X - bLocation.X);
        var y = Math.Abs(aLocation.Y - bLocation.Y);
        var z = Math.Abs(aLocation.Z - bLocation.Z);
        return Math.Max(x, Math.Max(y, z));
    }

    public static double DiagonalShortCut(HeuristicArgs args)
    {
        var a = args.A.Location;
        var b = args.B.Location;

        // Differenzen in den Koordinaten
        var dx = Math.Abs(a.X - b.X);
        var dy = Math.Abs(a.Y - b.Y);
        var dz = Math.Abs(a.Z - b.Z);

        // Erster Schritt: Minimiere Bewegungen entlang aller Achsen (drei diagonal möglich)
        var minXY = Math.Min(dx, dy);
        var minXZ = Math.Min(dx, dz);
        var minYZ = Math.Min(dy, dz);

        // Zweiter Schritt: Diagonale Bewegungen, die restliche Bewegung ist entlang einer Achse
        var diagonalMoves = Math.Min(minXY, Math.Min(minXZ, minYZ));
        var straightMoves = dx + dy + dz - 2 * diagonalMoves;

        // Rückgabe der berechneten Distanz
        return diagonalMoves * Math.Sqrt(2) + straightMoves;
    }

    public static double Euclidean(HeuristicArgs args)
    {
        var a = args.A.Location;
        var b = args.B.Location;
        return Math.Sqrt(Math.Pow(a.X - b.X, 2) + Math.Pow(a.Y - b.Y, 2) + Math.Pow(a.Z - b.Z, 2));
    }

    public static double Manhatten(HeuristicArgs args)
    {
        var a = args.A.Location;
        var b = args.B.Location;
        return Math.Abs(a.X - b.X) + Math.Abs(a.Y - b.Y) + Math.Abs(a.Z - b.Z);
    }
}

public class AStar : IPathfinder<Path, SearchPath, AStarSettings>
{
    private readonly IGraph _graph;
    private readonly AStarSettings _settings;

    public static IPathfinder<Path, SearchPath, AStarSettings> Create(IPathfinderConfig<AStarSettings> config)
        => new AStar(config.Graph, config.Settings);

    private AStar(IGraph graph, AStarSettings settings)
    {
        _graph = graph;
        _settings = settings;
    }

    public Path? GetShortestPath(SearchPath args)
    {
        var source = args.SourceNode;
        var target = args.TargetNode;

        var openSet = new SortedSet<PathNode>() { new(source.Id, source.RoutingCost, double.MinValue, null, source) };
        var closedSet = new HashSet<PathNode>();

        while (openSet.Count > 0)
        {
            var current = openSet.Min;
            if (current!.CurrentNode.Equals(target))
            {
                List<INode> path = [];
                while (current != null)
                {
                    path.Add(current.CurrentNode);
                    current = current.Parent;
                }

                path.Reverse();
                return new Path(source.Id, target.Id, [.. path]);
            }

            openSet.Remove(current);
            closedSet.Add(current);
            foreach (var neighbor in GetNeighbors(current.CurrentNode))
            {
                if (closedSet.Any(item => item.CurrentNode.Equals(current)))
                    continue;

                double tentativeGScore = current.G + current.CurrentNode.RoutingCost;
                var pathNode = new PathNode(neighbor.Id, tentativeGScore, Heuristic(neighbor, target), current, neighbor);
                if (!openSet.Any(item => item.CurrentNode.Equals(neighbor)))
                {
                    openSet.Add(pathNode);
                }
                else if (tentativeGScore < pathNode.G)
                {
                    openSet.RemoveWhere(item => item.Id == pathNode.Id);
                    openSet.Add(pathNode);
                }
            }
        }

        // Kein Pfad gefunden
        return null;
    }

    private double Heuristic(INode a, INode b) => _settings.Heuristic.Invoke(Tuple.Create(a, b));

    private INode[] GetNeighbors(INode node)
    {
        long[] allEdgeIds = [.. node.OutgoingEdgeIds];
        var allEdges = allEdgeIds.Select(_graph.GetEdge);
        var allNeighborNodeIds = allEdges.Select(item => item.EndNodeId).ToArray();
        return allNeighborNodeIds.Select(_graph.GetNode).ToArray();
    }
}

public record SearchPath(INode SourceNode, INode TargetNode) : IPathfinderArguments;
public record Path(long SourceNodeId, long TargetNodeId, INode[] Nodes) : IPathfinderResult;
public record PathNode(long Id, double G, double H, PathNode Parent, INode CurrentNode) : IComparable<PathNode>
{
    public double F => G + H;
    public int CompareTo(PathNode? other) => F.CompareTo(other?.F);
}