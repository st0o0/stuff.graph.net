using System.Numerics;
using stuff.graph.algorithms.net;
using stuff.graph.net;

namespace stuff.graph.erdosrenyi.net;

public class ErdosRenyi : IGenerator<Graph, ErdosRenyiArgs, IGeneratorConfig>
{
    private readonly Random _random;
    public static IGenerator<Graph, ErdosRenyiArgs, IGeneratorConfig> Create(IGeneratorConfig? config = null)
        => new ErdosRenyi(config?.Instance ?? new());

    private ErdosRenyi(Random random)
    {
        _random = random;
    }

    public Graph Generate(ErdosRenyiArgs args)
    {
        (uint numberOfNodes, double probability) = (args.NumberOfNodes, args.Prohability);
        if (numberOfNodes <= 0 || probability < 0 || probability > 1)
        {
            throw new ArgumentOutOfRangeException("Number of nodes must be greater than 0, and probability must be between 0 and 1.");
        }

        var builder = GraphBuilder.Create();

        for (int i = 0; i < numberOfNodes; i++)
        {
            builder.CreateNode(i, new Vector3(_random.Next(), _random.Next(), _random.Next()));
        }

        var edgeId = 0;
        var limit = builder.NodeCount;
        for (var i = 0; i < limit; i++)
        {
            for (var j = i + 1; j < limit; j++)
            {
                if (_random.NextDouble() <= probability)
                {
                    builder.CreateEdge(edgeId++, i, j, 1);
                }
            }
        }

        return builder.CreateGraph();
    }
}
