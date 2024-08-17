using System.Text.Json.Serialization;

namespace stuff.graph.serializable.net;

public record SerializableGraph(
    [property: JsonPropertyName("id")] Guid Id,
    [property: JsonPropertyName("edges")] SerializableEdge[] Edges,
    [property: JsonPropertyName("nodes")] SerializableNode[] Nodes    
);
