using System.Diagnostics.CodeAnalysis;
using stuff.graph.algorithms.net;

namespace stuff.graph.cost.net;

public abstract class CostService : ICostService<long, long>
{
    private readonly Dictionary<long, long> _edgeCosts = [];

    //public abstract bool TryAdd(long id, long value);

    public bool TryAddOrUpdate(long key, Func<long, long> update, long defaultValue)
    {
        var value = Get(key, defaultValue);
        _edgeCosts[key] = update(value);
        return true;
    }

    public bool TryGetValue(long id, [NotNullWhen(true)] out long value)
        => _edgeCosts.TryGetValue(id, out value);

    public long Get(long key, long defaultValue)
        => _edgeCosts.TryGetValue(key, out var oldValue) ? oldValue : defaultValue;
}
