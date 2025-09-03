namespace stuff.graph.algorithms.net;

public interface IOptionalService<in T> where T : class, IInjectable
{
    void Inject(T item);
}