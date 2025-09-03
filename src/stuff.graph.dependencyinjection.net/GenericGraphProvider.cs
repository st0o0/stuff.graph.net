using stuff.graph.net;

namespace stuff.graph.dependencyinjection.net;

public class GenericGraphProvider : IGraphProvider
{
    private readonly Func<Graph> _getGraphFunction;
 
    public GenericGraphProvider(Func<Graph> getGraphFunction)
    {
        _getGraphFunction = getGraphFunction;
    }

    public Graph GetGraph()
        => _getGraphFunction.Invoke();
}