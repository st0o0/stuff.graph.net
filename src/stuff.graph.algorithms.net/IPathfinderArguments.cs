using stuff.graph.net;

namespace stuff.graph.algorithms.net;

public interface IPathfinderArguments : IArguments
{
    INode SourceNode { get; }
    INode TargetNode { get; }
}