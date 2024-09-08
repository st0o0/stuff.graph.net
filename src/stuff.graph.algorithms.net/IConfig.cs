using stuff.graph.net;

namespace stuff.graph.algorithms.net;

public interface IConfig<TSettings> : IConfig where TSettings : ISettings
{
    TSettings Settings { get; }
}

public interface IConfig
{
    IGraph Graph { get; }
}