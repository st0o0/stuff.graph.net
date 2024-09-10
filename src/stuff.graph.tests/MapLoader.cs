using System.Text.Json;
using stuff.graph.serializable.net;

namespace stuff.graph.tests;

public partial class AstarTests
{
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
