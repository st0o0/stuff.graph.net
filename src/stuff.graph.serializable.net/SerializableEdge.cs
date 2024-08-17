using System.Text.Json.Serialization;
using stuff.graph.net;

namespace stuff.graph.serializable.net;

public record SerializableEdge(
    [property: JsonPropertyName("id")] long Id,
    [property: JsonPropertyName("startNodeId")] long StartNodeId,
    [property: JsonPropertyName("endNodeId")] long EndNodeId,
    [property: JsonPropertyName("additionalRoutingCost")] int AdditionalRoutingCost,
    [property: JsonPropertyName("allowedDirection")] EdgeDirection Direction = EdgeDirection.TwoWay);
