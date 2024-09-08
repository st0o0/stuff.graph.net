namespace stuff.graph.algorithms.net;

public interface IPathfinder<TResult, TArguments, TSettings> : IAlgorithm<IPathfinder<TResult, TArguments, TSettings>, IPathfinderConfig<TSettings>>
where TArguments : IPathfinderArguments
where TResult : IPathfinderResult
where TSettings : ISettings
{
    TResult? GetShortestPath(TArguments args);
}
