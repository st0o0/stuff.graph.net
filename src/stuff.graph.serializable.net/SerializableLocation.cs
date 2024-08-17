using System.Text.Json.Serialization;

namespace stuff.graph.serializable.net;

public record SerializableLocation(
    [property: JsonPropertyName("x")] float X,
    [property: JsonPropertyName("y")] float Y,
    [property: JsonPropertyName("z")] float Z
    );
