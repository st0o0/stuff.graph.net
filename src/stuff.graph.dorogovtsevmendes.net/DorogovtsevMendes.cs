using stuff.graph.algorithms.net;
using stuff.graph.net;

namespace stuff.graph.dorogovtsevmendes.net;

public class DorogovtsevMendes : IDorogovtsevMendes
{
    private readonly Random _random;

    public static IGenerator<Graph, GeneratorArgs, GeneratorConfig> Create(GeneratorConfig? config = null)
        => new DorogovtsevMendes(config?.Instance ?? new Random());

    private DorogovtsevMendes(Random random)
    {
        _random = random;
    }

    public Graph Generate(GeneratorArgs args)
    {
        ArgumentNullException.ThrowIfNull(args);
        var numberOfNodes = args.NumberOfNodes;
        var builder = GraphBuilder.Create(new GraphSettings(0, 0, 1));

        var node1 = builder.CreateNode(0, 0, 0, 0);
        var node2 = builder.CreateNode(1, 1, 0, 0);
        var node3 = builder.CreateNode(2, 0.5f, 1, 0);
        _ = builder.CreateEdge(0, node3.Id, node1.Id);
        _ = builder.CreateEdge(1, node3.Id, node2.Id);
        _ = builder.CreateEdge(2, node1.Id, node2.Id);

        for (var i = 3; i < numberOfNodes; i++)
        {
            var edgeIds = builder.EdgeIds;
            var randomEdgeId = edgeIds[_random.Next(edgeIds.Length)];

            var randomEdge = builder.Edges[randomEdgeId];

            var startNode = builder.Nodes[randomEdge.StartNodeId];
            var endNode = builder.Nodes[randomEdge.EndNodeId];

            var newNode = builder.CreateNode(i, (startNode.Location + endNode.Location) / 2);

            var highestEdgeId = builder.EdgeIds[^1];
            builder.CreateEdge(highestEdgeId + 1, startNode.Id, newNode.Id);
            builder.CreateEdge(highestEdgeId + 2, newNode.Id, endNode.Id);
            builder.RemoveEdge(randomEdge);
        }

        return builder.CreateGraph();
    }
}
