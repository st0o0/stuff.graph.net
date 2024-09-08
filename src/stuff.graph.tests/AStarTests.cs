using System.Diagnostics;
using System.Runtime.Serialization;
using System.Text.Json;
using System.Text.Json.Serialization;
using stuff.graph.algorithms.net;
using stuff.graph.astar.net;
using stuff.graph.net;
using stuff.graph.serializable.net;

namespace stuff.graph.tests;

public class AstarTests
{
    [Fact]
    public void FindPath_ShouldReturnCorrectPath_WhenPathExists()
    {
        // Arrange;
        var builder = GraphBuilder.Create(new GraphSettings(1, 0, 0));
        var startNode = builder.CreateNode(1, 0, 0, 0);
        var middleNode = builder.CreateNode(2, 1, 1, 1);
        var endNode = builder.CreateNode(3, 2, 2, 2);

        var edge1 = builder.CreateEdge(1, startNode.Id, middleNode.Id);
        var edge2 = builder.CreateEdge(2, middleNode.Id, endNode.Id);
        var graph = builder.CreateGraph();

        var settings = new AStarSettings(Heuristic.Manhatten);
        var astar = AStar.Create(new PathfinderConfig<AStarSettings>(graph, settings));

        // Act
        var path = astar.GetShortestPath(new SearchPath(startNode, endNode));

        // Assert
        Assert.NotNull(path);
        Assert.Equal(3, path.Nodes.Length);
        Assert.Equal([startNode, middleNode, endNode], path.Nodes);
    }

    [Fact]
    public void FindPath_ShouldReturnNull_WhenNoPathExists()
    {
        // Arrange
        var builder = GraphBuilder.Create(new GraphSettings(1, 0, 0));
        var startNode = builder.CreateNode(1, 0, 0, 0);
        var isolatedNode = builder.CreateNode(2, 10, 10, 10);
        var graph = builder.CreateGraph();
        var settings = new AStarSettings(Heuristic.Manhatten);
        var astar = AStar.Create(new PathfinderConfig<AStarSettings>(graph, settings));

        // Act
        var path = astar.GetShortestPath(new SearchPath(startNode, isolatedNode));

        // Assert
        Assert.Null(path);
    }

    [Fact]
    public void FindPath_ShouldHandleSingleStepPath()
    {
        // Arrange
        var builder = GraphBuilder.Create(new GraphSettings(1, 0, 0));
        var startNode = builder.CreateNode(1, 0, 0, 0);
        var endNode = builder.CreateNode(2, 1, 1, 1);
        var edge = builder.CreateEdge(1, startNode.Id, endNode.Id);
        var graph = builder.CreateGraph();
        var settings = new AStarSettings(Heuristic.Manhatten);
        var astar = AStar.Create(new PathfinderConfig<AStarSettings>(graph, settings));

        // Act
        var path = astar.GetShortestPath(new SearchPath(startNode, endNode));

        // Assert
        Assert.NotNull(path);
        Assert.Equal(2, path.Nodes.Length);
        Assert.Equal([startNode, endNode], path.Nodes);
    }

    [Fact]
    public void FindPath_OnCustomMap()
    {
        var json = "./newmap.json";
        var jsonGraph = MapLoader.Load(json);
        var graph = jsonGraph.To();

        var settings = new AStarSettings(Heuristic.Manhatten);
        var astar = AStar.Create(new PathfinderConfig<AStarSettings>(graph, settings));

        var maxNodeId = graph.Nodes.Values.Max(x => x.Id);
        var paths = Enumerable.Range(1, (int)maxNodeId).SelectMany(source =>
        {
            var sourceNode = graph.GetNode(source);
            return Enumerable.Range(1, (int)maxNodeId).Select(target =>
            {
                Debug.Print($"{source:d4} -> {target:d4}");
                return astar.GetShortestPath(new SearchPath(sourceNode, graph.GetNode(target)));
            });
        }).Where(x => x is not null).ToArray();
        // Assert
        Assert.NotEmpty(paths);
    }

    public static class MapLoader
    {
        public static SerializableGraph Load(string json)
        {
            return JsonSerializer.Deserialize<SerializableGraph>(File.ReadAllText(json))!;
        }

        public static void Write(string json, SerializableGraph serializableGraph)
        {
            File.AppendAllText(json, JsonSerializer.Serialize(serializableGraph));
        }
    }
}
