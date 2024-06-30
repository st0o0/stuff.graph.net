﻿using System.Numerics;

namespace stuff.graph.net;

public class Node : INode
{
    public int Id { get; init; }

    public Vector3 Location { get; init; } = Vector3.Zero;
}