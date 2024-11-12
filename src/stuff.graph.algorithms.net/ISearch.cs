namespace stuff.graph.algorithms.net;

public interface ISearch<TResult, TArguments, TSettings> : IAlgorithm<ISearch<TResult, TArguments, TSettings>, ISearchConfig<TSettings>>, IOptionalService<INodeCostService>, IOptionalService<IEdgeCostService>
where TArguments : ISearchArgs
where TResult : ISearchResult
where TSettings : ISettings
{
    TResult? GetShortestPath(TArguments args);
}