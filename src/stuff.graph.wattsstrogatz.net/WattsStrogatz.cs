using stuff.graph.algorithms.net;
using System.Numerics;
using stuff.graph.net;

namespace stuff.graph.wattsstrogatz.net;

public class WattsStrogatz : IGenerator<Graph, IGeneratorArgs, IGeneratorConfig<WattsStrogatzSettings>>
{
    private readonly WattsStrogatzSettings _settings;
    public static IGenerator<Graph, IGeneratorArgs, IGeneratorConfig<WattsStrogatzSettings>> Create(IGeneratorConfig<WattsStrogatzSettings> config)
         => new WattsStrogatz(config.Settings);

    private WattsStrogatz(WattsStrogatzSettings settings)
    {
        _settings = settings;
    }

    public Graph Generate(IGeneratorArgs args)
    {
        var numberOfNodes = args.NumberOfNodes;
        var rand = new Random();
        var builder = GraphBuilder.Create(new GraphSettings(0, 0, 0));

        for (long i = 0; i < numberOfNodes; i++)
        {
            builder.CreateNode(i, new Vector3(rand.Next(0, 100), rand.Next(0, 100), 0), 0);
        }

        var edgeId = 0L;
        for (var i = 0; i < numberOfNodes; i++)
        {
            for (var j = 1; j <= numberOfNodes / 2; j++)
            {
                var neighbor = (i + j) % numberOfNodes;
                builder.CreateEdge(edgeId, i, neighbor);
                edgeId++;
            }
        }
        for (var i = 0; i < numberOfNodes; i++)
        {
            for (var j = 1; j <= numberOfNodes / 2; j++)
            {
                if (rand.NextDouble() < _settings.Beta)
                {
                    var oldNeighbor = (i + j) % numberOfNodes;

                    var possibleNeighbors = Enumerable
                                                .Range(0, (int)numberOfNodes)
                                                .Where(n => n != i && !builder.Nodes[i].OutgoingEdgeIds.Contains(n))
                                                .Select(n => (long)n)
                                                .ToList();

                    if (possibleNeighbors.Count == 0)
                        continue;

                    var newNeighbor = possibleNeighbors[rand.Next(possibleNeighbors.Count)];

                    var startNode = builder.Nodes[i];
                    var endNode = builder.Nodes[oldNeighbor];
                    startNode.OutgoingEdgeIds = startNode.OutgoingEdgeIds.Where(eid => builder.Edges[eid].EndNodeId != oldNeighbor).ToArray();
                    endNode.IncomingEdgeIds = endNode.IncomingEdgeIds.Where(eid => builder.Edges[eid].StartNodeId != i).ToArray();
                    builder.CreateEdge(edgeId, i, newNeighbor);
                    edgeId++;
                }
            }
        }

        return builder.CreateGraph();
    }
}