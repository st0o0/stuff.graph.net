using System.Numerics;
using stuff.graph.tests;

namespace stuff.graph.net;

public static class GraphBuilderExtensions
{
    public static INode CreateNode(this GraphBuilder builder, long id, float x, float y, float z)
        => builder.CreateNode(id, new Vector3(x, y, z));
}