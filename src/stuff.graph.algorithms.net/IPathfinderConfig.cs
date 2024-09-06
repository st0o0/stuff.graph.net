namespace stuff.graph.algorithms.net;

public interface IPathfinderConfig<TSettings> : IConfig where TSettings : IPathfinderSettings
{
    TSettings Settings { get; }
}
