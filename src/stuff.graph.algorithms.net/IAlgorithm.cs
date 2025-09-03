namespace stuff.graph.algorithms.net;

public interface IAlgorithm<out T, in TConfig> where TConfig : IConfig
{
    static T Create(TConfig config) => throw new NotImplementedException();
}