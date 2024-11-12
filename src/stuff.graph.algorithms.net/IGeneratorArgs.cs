namespace stuff.graph.algorithms.net;

public interface IGeneratorArgs : IArgs
{
    uint NumberOfNodes { get; }
}

public record GeneratorArgs(uint NumberOfNodes) : IGeneratorArgs;
