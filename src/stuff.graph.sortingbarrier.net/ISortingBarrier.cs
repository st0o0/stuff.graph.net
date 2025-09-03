using stuff.graph.algorithms.net;
using Path = stuff.graph.algorithms.net.Path;

namespace stuff.graph.sortingbarrier.net;

public interface ISortingBarrier : ISearch<Path, ISearchArgs, SortingBarrierSettings>;