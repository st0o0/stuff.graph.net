using System.Numerics;
using stuff.graph.algorithms.net;
using stuff.graph.net;

namespace stuff.graph.erdosrenyi.net;

public class ErdosRenyi : IErdosRenyi
{
    private readonly Random _random;
    public static IGenerator<Graph, ErdosRenyiArgs, IGeneratorConfig> Create(IGeneratorConfig? config = null)
        => new ErdosRenyi(config?.Instance ?? new Random());

    private ErdosRenyi(Random random)
    {
        _random = random;
    }

    public Graph Generate(ErdosRenyiArgs args)
    {
        var (numberOfNodes, probability) = (args.NumberOfNodes, args.Prohability);
        if (numberOfNodes <= 0 || probability < 0 || probability > 1)
        {
            throw new ArgumentOutOfRangeException(nameof(numberOfNodes));
        }

        var builder = GraphBuilder.Create();

        for (var i = 0; i < numberOfNodes; i++)
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
