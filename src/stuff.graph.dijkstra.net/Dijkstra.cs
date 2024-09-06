using stuff.graph.net;

namespace stuff.graph.dijkstra.net;

public class Dijkstra(IGraph graph)
{
    private readonly IGraph _graph = graph;

    public List<long> FindShortestPath(long startNodeId, long targetNodeId)
    {
        // Priority Queue für die Dijkstra-Algorithmus
        var priorityQueue = new SortedSet<(uint distance, long nodeId)>();
        var distances = new Dictionary<long, uint>();
        var previousNodes = new Dictionary<long, long>();

        // Initialisiere alle Knoten
        foreach (var node in _graph.Nodes.Values)
        {
            distances[node.Id] = uint.MaxValue;
            previousNodes[node.Id] = long.MinValue;
        }

        // Distanz des Startknotens zu sich selbst ist 0
        distances[startNodeId] = 0;
        priorityQueue.Add((0, startNodeId));

        while (priorityQueue.Count > 0)
        {
            // Wähle den Knoten mit der kleinsten Distanz
            var (currentDistance, currentNodeId) = priorityQueue.Min;
            priorityQueue.Remove(priorityQueue.Min);

            // Wenn wir den Zielknoten erreicht haben, können wir aufhören
            if (currentNodeId == targetNodeId)
            {
                return ReconstructPath(previousNodes, targetNodeId);
            }

            var currentNode = _graph.Nodes[currentNodeId];
            foreach (var edgeId in currentNode.OutgoingEdgeIds)
            {
                var edge = _graph.Edges[edgeId];
                var neighborNodeId = edge.EndNodeId;

                // Berechne die neue potenzielle Distanz zum Nachbarknoten
                var newDistance = currentDistance + edge.RoutingCost;

                // Wenn die neue Distanz kleiner ist als die bisher bekannte, aktualisiere
                if (newDistance < distances[neighborNodeId])
                {
                    // Entferne den alten Wert aus der Queue (falls vorhanden)
                    priorityQueue.Remove((distances[neighborNodeId], neighborNodeId));

                    // Aktualisiere die Distanz und den Vorgänger
                    distances[neighborNodeId] = newDistance;
                    previousNodes[neighborNodeId] = currentNodeId;

                    // Füge den Nachbarknoten mit der aktualisierten Distanz zur Queue hinzu
                    priorityQueue.Add((newDistance, neighborNodeId));
                }
            }
        }

        // Wenn kein Pfad gefunden wurde, returniere eine leere Liste
        return [];
    }

    // Hilfsmethode zur Rekonstruktion des Pfades vom Zielknoten zum Startknoten
    private static List<long> ReconstructPath(Dictionary<long, long> previousNodes, long targetNodeId)
    {
        var path = new List<long>();
        var currentNodeId = targetNodeId;

        // Rückverfolgung des Pfades anhand der Vorgänger-Knoten
        while (currentNodeId != null)
        {
            path.Add(currentNodeId);
            currentNodeId = previousNodes[currentNodeId];
        }

        // Da wir vom Ziel zum Start gehen, müssen wir den Pfad umdrehen
        path.Reverse();
        return path;
    }
}