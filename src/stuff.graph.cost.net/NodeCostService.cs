using stuff.graph.algorithms.net;
using stuff.graph.net;

namespace stuff.graph.cost.net;

public class NodeCostService : CostService, INodeCostService
{
    private uint _defaultValue;
    public void SetDefaultValue(uint defaultValue) => _defaultValue = defaultValue;

    public uint Get(INode? node)
        => base.Get(node?.Id ?? long.MinValue, _defaultValue) + node?.RoutingCost ?? 0;
}
