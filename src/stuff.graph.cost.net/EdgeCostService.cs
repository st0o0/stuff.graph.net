using stuff.graph.algorithms.net;
using stuff.graph.net;

namespace stuff.graph.cost.net;

public class EdgeCostService : CostService, IEdgeCostService
{
    private uint _defaultValue;
    public void SetDefaultValue(uint defaultValue) => _defaultValue = defaultValue;

    public uint Get(IEdge? edge)
        => base.Get(edge?.Id ?? long.MinValue, _defaultValue) + (edge?.RoutingCost ?? 0);
}