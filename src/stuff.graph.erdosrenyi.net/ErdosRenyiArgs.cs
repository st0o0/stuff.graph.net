using stuff.graph.algorithms.net;

namespace stuff.graph.erdosrenyi.net;

public record ErdosRenyiArgs(uint NumberOfNodes, double Prohability = 1) : IGeneratorArgs;
