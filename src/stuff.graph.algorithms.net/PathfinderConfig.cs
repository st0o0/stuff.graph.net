using stuff.graph.net;

namespace stuff.graph.algorithms.net;

public record PathfinderConfig<TSettings>(IGraph Graph, TSettings Settings) : IPathfinderConfig<TSettings> where TSettings : IPathfinderSettings;