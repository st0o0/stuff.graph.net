using stuff.graph.algorithms.net;
using stuff.graph.net;

namespace stuff.graph.sortingbarrier.net;

public record SortingBarrierConfig(IGraph  Graph, SortingBarrierSettings Settings) : ISearchConfig, ISearchConfig<SortingBarrierSettings>;