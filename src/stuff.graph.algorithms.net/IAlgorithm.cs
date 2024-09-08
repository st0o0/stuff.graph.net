namespace stuff.graph.algorithms.net;

public interface IAlgorithm<T, TConfig> where TConfig : IConfig
{
    static T Create(TConfig config) => throw new NotImplementedException();
}