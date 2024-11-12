using stuff.graph.net;

namespace stuff.graph.algorithms.net;

public interface IGenerator<TResult, TArguments, TConfig> : IAlgorithm<IGenerator<TResult, TArguments, TConfig>, TConfig>
where TArguments : IGeneratorArgs
where TConfig : IGeneratorConfig
where TResult : IGraph
{
    TResult Generate(TArguments args);
}
