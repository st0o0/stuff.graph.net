using stuff.graph.algorithms.net;
using stuff.graph.net;

namespace stuff.graph.astar.net;

public record AStarSettings(Func<INode, INode, double> Heuristic) : ISettings;
