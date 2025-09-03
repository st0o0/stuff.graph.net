using stuff.graph.net;

namespace stuff.graph.algorithms.net;

public interface ISearchConfig<out TSettings> : ISearchConfig, IConfig<TSettings> where TSettings : ISettings;

public interface ISearchConfig : IConfig
{
    IGraph Graph { get; }
}
