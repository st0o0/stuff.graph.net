using stuff.graph.net;

namespace stuff.graph.astar.net;

public class Astar(IGraph graph)
{
    private readonly IGraph _graph = graph;

    public INode[] FindPath(INode start, INode end)
    {
        return [];
    }

    private List<INode> GetAdjacentNodes(INode n)
    {
        return [];
    }
}