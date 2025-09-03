using stuff.graph.algorithms.net;

namespace stuff.graph.cost.net;

public abstract class CostService : ICostService<long, uint>
{
    private readonly Dictionary<long, uint> _edgeCosts = [];

    public bool TryAddOrUpdate(long key, Func<long, uint> update, uint defaultValue)
    {
        var value = Get(key, defaultValue);
        _edgeCosts[key] = update(value);
        return true;
    }

    public virtual bool TryGetValue(long id, out uint value)
        => _edgeCosts.TryGetValue(id, out value);

    public virtual uint Get(long key, uint defaultValue)
        => _edgeCosts.GetValueOrDefault(key, defaultValue);
}
