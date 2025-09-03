using stuff.graph.algorithms.net;
using Path = stuff.graph.algorithms.net.Path;

namespace stuff.graph.dijkstra.net;

public interface IDijkstra : ISearch<Path, ISearchArgs, ISettings>;