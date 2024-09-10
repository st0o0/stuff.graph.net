using stuff.graph.algorithms.net;
using stuff.graph.net;

namespace stuff.graph.dijkstra.net;

public record Path(long SourceNodeId, long TargetNodeId, INode[] Nodes) : IPathfinderResult;
