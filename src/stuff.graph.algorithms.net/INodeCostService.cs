using stuff.graph.net;

namespace stuff.graph.algorithms.net;

public interface INodeCostService : ICostService<long, uint>
{
    void SetDefaultValue(uint defaultValue);
    uint Get(INode? node);
}
