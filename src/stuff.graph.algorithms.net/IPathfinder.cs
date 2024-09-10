namespace stuff.graph.algorithms.net;

public interface IPathfinder<TResult, TArguments, TSettings> : IAlgorithm<IPathfinder<TResult, TArguments, TSettings>, IConfig<TSettings>>
where TArguments : IPathfinderArguments
where TResult : IPathfinderResult
where TSettings : ISettings
{
    TResult? GetShortestPath(TArguments args);
}


public interface IUpdatableSettings<TSettings> : IUpdatable<TSettings> where TSettings : ISettings
{
}

public interface IUpdatable<T>
{
    bool Update(T value);
}