using stuff.graph.net;

namespace stuff.graph.algorithms.net;

public interface IPathfinderResult : IResult
{
    long SourceNodeId { get; }
    long TargetNodeId { get; }
    INode[] Nodes { get;}
}