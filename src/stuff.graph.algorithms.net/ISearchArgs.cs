using stuff.graph.net;

namespace stuff.graph.algorithms.net;

public interface ISearchArgs : IArgs
{
    INode SourceNode { get; }
    INode TargetNode { get; }
}
