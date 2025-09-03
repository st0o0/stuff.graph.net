namespace stuff.graph.algorithms.net;

public interface IUpdatable<in T>
{
    bool Update(T value);
}