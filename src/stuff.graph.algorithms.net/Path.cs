using stuff.graph.net;

namespace stuff.graph.algorithms.net;

public record Path(long SourceNodeId, long TargetNodeId, INode[] Nodes) : ISearchResult;
