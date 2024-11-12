namespace stuff.graph.algorithms.net;

public interface IOptionalService<T> where T : class, IInjectable
{
    void Inject(T item);
}