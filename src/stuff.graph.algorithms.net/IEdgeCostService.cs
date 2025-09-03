using stuff.graph.net;

namespace stuff.graph.algorithms.net;

public interface IEdgeCostService : ICostService<long, uint>
{
    void SetDefaultValue(uint defaultValue);
    uint Get(IEdge? edge);
}
