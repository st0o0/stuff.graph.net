using System.Diagnostics.CodeAnalysis;

namespace stuff.graph.algorithms.net;

public interface ICostService<in TKey, TValue> : IInjectable
{
    public bool TryGetValue(TKey key, [NotNullWhen(true)] out TValue value);
    public TValue? Get(TKey key, TValue defaultValue);
}