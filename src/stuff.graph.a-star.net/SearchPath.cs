using stuff.graph.algorithms.net;
using stuff.graph.net;

namespace stuff.graph.astar.net;

public record SearchPath(INode SourceNode, INode TargetNode) : IPathfinderArguments;
