using stuff.graph.algorithms.net;

namespace stuff.graph.pagerank.net;

public record PageRankSettings(double DampingFactor, double Tolerance, int MaxIterations) : ISettings;
