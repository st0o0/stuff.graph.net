using System.Text.Json.Serialization;

namespace stuff.graph.serializable.net;

public record SerializableNode(
        [property: JsonPropertyName("id")] long Id,
        [property: JsonPropertyName("location")] SerializableLocation Location);
