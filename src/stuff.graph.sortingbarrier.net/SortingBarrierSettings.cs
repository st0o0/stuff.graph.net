using stuff.graph.algorithms.net;

namespace stuff.graph.sortingbarrier.net;

public record SortingBarrierSettings(double Delta = 1.0, int RelaxRounds = 4) : ISettings;