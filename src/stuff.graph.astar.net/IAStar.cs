using stuff.graph.algorithms.net;
using Path = stuff.graph.algorithms.net.Path;

namespace stuff.graph.astar.net;

public interface IAStar : ISearch<Path, ISearchArgs, AStarSettings>, IUpdatable<AStarSettings>;