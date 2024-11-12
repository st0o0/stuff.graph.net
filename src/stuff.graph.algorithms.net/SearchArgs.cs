using stuff.graph.net;

namespace stuff.graph.algorithms.net;

public record SearchArgs(INode SourceNode, INode TargetNode) : ISearchArgs;