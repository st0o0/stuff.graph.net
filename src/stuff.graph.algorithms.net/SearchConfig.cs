using stuff.graph.net;

namespace stuff.graph.algorithms.net;

public record SearchConfig(IGraph Graph) : ISearchConfig;

public record SearchConfig<TSettings>(IGraph Graph, TSettings Settings) : ISearchConfig<TSettings> where TSettings : ISettings;
