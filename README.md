# 📊 stuff.graph.net

A lightweight and efficient graph theory library for .NET, providing spatial graph data structures and pathfinding algorithms.

[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)
[![.NET](https://img.shields.io/badge/.NET-8.0-purple)](https://dotnet.microsoft.com/)
[![NuGet](https://img.shields.io/nuget/v/stuff.graph.net.svg)](https://www.nuget.org/packages/stuff.graph.net/)
[![NuGet](https://img.shields.io/nuget/dt/stuff.graph.net.svg)](https://www.nuget.org/packages/stuff.graph.net/)


## 📋 Table of Contents

- [Overview](#overview)
- [Features](#features)
- [Installation](#installation)
- [Quick Start](#quick-start)
- [Usage Examples](#usage-examples)
- [Contributing](#contributing)
- [License](#license)

## 🎯 Overview

`stuff.graph.net` is a comprehensive spatial graph library designed for .NET developers who need to work with graph theory, pathfinding algorithms, and network analysis. The library supports 3D spatial coordinates and provides efficient implementations of various graph algorithms including Dijkstra, A*, PageRank, and multiple graph generators for creating synthetic networks.

Whether you're building route optimization systems, analyzing social networks, generating procedural maps, or studying network topologies, this modular library provides the tools you need.

## ✨ Features

- **Spatial Graph Support**
    - 3D coordinate system (x, y, z) for nodes
    - ID-based node and edge management
    - Directed edges with customizable weights

- **Pathfinding Algorithms**
    - **Dijkstra** (`stuff.graph.dijkstra.net`): Shortest path algorithm
    - **A*** (`stuff.graph.astar.net`): Heuristic-based pathfinding
    - Efficient path search with configurable parameters

- **Graph Analysis**
    - **Weakly Connected Components** (`stuff.graph.wcc.net`): Find disconnected subgraphs
    - **PageRank** (`stuff.graph.pagerank.net`): Node importance ranking
    - **Minimum Weight Spanning Tree** (`stuff.graph.mwst.net`): Find minimal spanning trees
    - Graph traversal and connectivity analysis

- **Graph Generators**
    - **Erdős-Rényi** (`stuff.graph.erdosrenyi.net`): Random graph generation
    - **Watts-Strogatz** (`stuff.graph.wattsstrogatz.net`): Small-world networks
    - **Dorogovtsev-Mendes** (`stuff.graph.dorogovtsevmendes.net`): Scale-free networks

- **Additional Features**
    - **Cost Calculation** (`stuff.graph.cost.net`): Edge cost computations
    - **Serialization** (`stuff.graph.serializable.net`): JSON import/export
    - **Dependency Injection** (`stuff.graph.dependencyinjection.net`): DI support for ASP.NET Core

- **Performance Optimized**
    - Efficient memory usage
    - Fast algorithm implementations
    - Suitable for large-scale graphs

## 📦 Installation

### Core Package

```bash
# NuGet Package Manager
Install-Package stuff.graph.net

# .NET CLI
dotnet add package stuff.graph.net
```

### Algorithm Packages

```bash
# Base algorithms
Install-Package stuff.graph.algorithms.net

# Pathfinding
Install-Package stuff.graph.dijkstra.net
Install-Package stuff.graph.astar.net

# Graph Analysis
Install-Package stuff.graph.wcc.net
Install-Package stuff.graph.pagerank.net
Install-Package stuff.graph.mwst.net

# Graph Generators
Install-Package stuff.graph.erdosrenyi.net
Install-Package stuff.graph.wattsstrogatz.net
Install-Package stuff.graph.dorogovtsevmendes.net

# Utilities
Install-Package stuff.graph.cost.net
Install-Package stuff.graph.serializable.net
Install-Package stuff.graph.dependencyinjection.net
```

## 🚀 Quick Start

Here's a simple example to get you started:

```csharp
using stuff.graph.net;
using stuff.graph.dijkstra.net;

// Create a graph with settings
var builder = GraphBuilder.Create(new GraphSettings(1, 0, 0));

// Create nodes with 3D coordinates
var nodeA = builder.CreateNode(id: 1, x: 0, y: 0, z: 0);
var nodeB = builder.CreateNode(id: 2, x: 1, y: 0, z: 0);
var nodeC = builder.CreateNode(id: 3, x: 2, y: 0, z: 0);

// Create weighted edges
var edgeAB = builder.CreateEdge(id: 1, sourceId: 1, targetId: 2, weight: 1.0);
var edgeBC = builder.CreateEdge(id: 2, sourceId: 2, targetId: 3, weight: 2.0);

// Connect nodes with edges
nodeA.AddOutgoing(edgeAB.Id);
nodeB.AddIncoming(edgeAB.Id);
nodeB.AddOutgoing(edgeBC.Id);
nodeC.AddIncoming(edgeBC.Id);

// Build the graph
var graph = builder.CreateGraph();

// Find shortest path using Dijkstra
var pathfinder = Dijkstra.Create(new DijkstraConfig(graph));
var path = pathfinder.GetShortestPath(new SearchArgs(
    graph.GetNode(1), 
    graph.GetNode(3)
));

Console.WriteLine($"Path found with {path.Nodes.Length} nodes");
```

## 💡 Usage Examples

### Creating a Basic Graph

```csharp
using stuff.graph.net;

var builder = GraphBuilder.Create(new GraphSettings(1, 0, 0));

// Create nodes at different spatial locations
var node1 = builder.CreateNode(1, x: 0, y: 0, z: 0);
var node2 = builder.CreateNode(2, x: 5, y: 0, z: 0);
var node3 = builder.CreateNode(3, x: 10, y: 5, z: 0);
var node4 = builder.CreateNode(4, x: 5, y: 10, z: 0);

// Create edges with weights
var edge1 = builder.CreateEdge(1, sourceId: 1, targetId: 2, weight: 5.0);
var edge2 = builder.CreateEdge(2, sourceId: 2, targetId: 3, weight: 7.0);
var edge3 = builder.CreateEdge(3, sourceId: 1, targetId: 4, weight: 12.0);
var edge4 = builder.CreateEdge(4, sourceId: 4, targetId: 3, weight: 3.0);

// Establish connections
node1.AddOutgoing(edge1.Id);
node2.AddIncoming(edge1.Id);

node2.AddOutgoing(edge2.Id);
node3.AddIncoming(edge2.Id);

node1.AddOutgoing(edge3.Id);
node4.AddIncoming(edge3.Id);

node4.AddOutgoing(edge4.Id);
node3.AddIncoming(edge4.Id);

var graph = builder.CreateGraph();
```

### Finding Shortest Path with Dijkstra

```csharp
using stuff.graph.dijkstra.net;

var graph = SetupYourGraph(); // Use GraphBuilder as shown above

// Create Dijkstra pathfinder
var dijkstra = Dijkstra.Create(new DijkstraConfig(graph));

// Find path from node 1 to node 3
var sourceNode = graph.GetNode(1);
var targetNode = graph.GetNode(3);
var path = dijkstra.GetShortestPath(new SearchArgs(sourceNode, targetNode));

if (path.Nodes.Any())
{
    Console.WriteLine("Path found:");
    foreach (var node in path.Nodes)
    {
        Console.WriteLine($"  Node {node.Id} at ({node.X}, {node.Y}, {node.Z})");
    }
}
else
{
    Console.WriteLine("No path found");
}
```

### Finding Weakly Connected Components

```csharp
using stuff.graph.wcc.net;

var graph = SetupYourGraph();

// Find all disconnected components
var wcc = WeaklyConnectedComponents.Create(new WCCConfig(graph));
var components = wcc.Find();

Console.WriteLine($"Found {components.Length} connected components");

for (int i = 0; i < components.Length; i++)
{
    var component = components[i];
    Console.WriteLine($"Component {i + 1}:");
    Console.WriteLine($"  Nodes: {component.Nodes.Count}");
    Console.WriteLine($"  Edges: {component.Edges.Count}");
}

// Get the largest component
var largestComponent = components
    .OrderByDescending(c => c.Nodes.Count + c.Edges.Count)
    .First();
```

### Handling Same Node Query

```csharp
var dijkstra = Dijkstra.Create(new DijkstraConfig(graph));
var sameNode = graph.GetNode(1);

// Path from a node to itself
var path = dijkstra.GetShortestPath(new SearchArgs(sameNode, sameNode));

// Returns single-node path
Assert.Equal(1, path.Nodes.Length);
Assert.Equal(1, path.Nodes[0].Id);
```

### Using A* Pathfinding

```csharp
using stuff.graph.astar.net;

var graph = SetupYourGraph();

// Create A* pathfinder (uses heuristic for faster pathfinding)
var astar = AStar.Create(new AStarConfig(graph));

var sourceNode = graph.GetNode(1);
var targetNode = graph.GetNode(10);

// Find path with heuristic guidance
var path = astar.GetShortestPath(new SearchArgs(sourceNode, targetNode));

Console.WriteLine($"A* found path with {path.Nodes.Length} nodes");
```

### Calculating PageRank

```csharp
using stuff.graph.pagerank.net;

var graph = SetupYourGraph();

// Calculate PageRank for all nodes
var pagerank = PageRank.Create(new PageRankConfig(graph));
var ranks = pagerank.Calculate();

// Get most important nodes
var topNodes = ranks
    .OrderByDescending(kvp => kvp.Value)
    .Take(10);

foreach (var (nodeId, rank) in topNodes)
{
    Console.WriteLine($"Node {nodeId}: {rank:F4}");
}
```

### Finding Minimum Weight Spanning Tree

```csharp
using stuff.graph.mwst.net;

var graph = SetupYourGraph();

// Find minimum spanning tree
var mwst = MinimumWeightSpanningTree.Create(new MWSTConfig(graph));
var spanningTree = mwst.Find();

Console.WriteLine($"Spanning tree has {spanningTree.Edges.Count} edges");
Console.WriteLine($"Total weight: {spanningTree.Edges.Sum(e => e.Weight)}");
```

### Generating Random Graphs

```csharp
using stuff.graph.erdosrenyi.net;
using stuff.graph.wattsstrogatz.net;

// Generate Erdős-Rényi random graph
var erConfig = new ErdosRenyiConfig(nodeCount: 100, edgeProbability: 0.1);
var randomGraph = ErdosRenyi.Generate(erConfig);

// Generate Watts-Strogatz small-world network
var wsConfig = new WattsStrogatzConfig(nodeCount: 100, k: 6, beta: 0.3);
var smallWorldGraph = WattsStrogatz.Generate(wsConfig);

// Generate Dorogovtsev-Mendes scale-free network
var dmConfig = new DorogovtsevMendesConfig(nodeCount: 100);
var scaleFreeGraph = DorogovtsevMendes.Generate(dmConfig);
```

### Working with Weakly Connected Components

```csharp
using System.Diagnostics;
using stuff.graph.wcc.net;
using stuff.graph.dijkstra.net;

var graph = SetupYourGraph(); // Create your graph

// Find connected components
var wcc = WeaklyConnectedComponents.Create(new WCCConfig(graph));
var stopwatch = Stopwatch.StartNew();
var components = wcc.Find();
stopwatch.Stop();
Console.WriteLine($"WCC completed in {stopwatch.ElapsedMilliseconds}ms");
Console.WriteLine($"Found {components.Length} connected components");

// Work with largest component
var mainGraph = components
    .OrderByDescending(c => c.Nodes.Count)
    .First();

Console.WriteLine($"Largest component has {mainGraph.Nodes.Count} nodes");

// Find path in main graph
var minNodeId = mainGraph.Nodes.Keys.Min();
var maxNodeId = mainGraph.Nodes.Keys.Max();

var dijkstra = Dijkstra.Create(new DijkstraConfig(mainGraph));
stopwatch.Restart();
var path = dijkstra.GetShortestPath(new SearchArgs(
    mainGraph.GetNode(minNodeId),
    mainGraph.GetNode(maxNodeId)
));
stopwatch.Stop();
Console.WriteLine($"Dijkstra completed in {stopwatch.ElapsedMilliseconds}ms");
Console.WriteLine($"Path contains {path.Nodes.Length} nodes");
```

## 🤝 Contributing

Contributions are welcome! Please feel free to submit a Pull Request. For major changes, please open an issue first to discuss what you would like to change.

### Development Setup

1. Clone the repository:
   ```bash
   git clone https://github.com/st0o0/stuff.graph.net.git
   cd stuff.graph.net
   ```

2. Open in your preferred IDE (Visual Studio, Rider, VS Code)

3. Build the solution:
   ```bash
   dotnet build
   ```

4. Run tests:
   ```bash
   dotnet test
   ```

### Contribution Guidelines

- Follow the existing code style and conventions
- Add unit tests for new features
- Update documentation for API changes
- Ensure all tests pass before submitting PR

## 📄 License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## 🔗 Links

- [GitHub Repository](https://github.com/st0o0/stuff.graph.net)
- [Issue Tracker](https://github.com/st0o0/stuff.graph.net/issues)

### NuGet Packages

**Core & Algorithms:**
- [stuff.graph.net](https://www.nuget.org/packages/stuff.graph.net)
- [stuff.graph.algorithms.net](https://www.nuget.org/packages/stuff.graph.algorithms.net)

**Pathfinding:**
- [stuff.graph.dijkstra.net](https://www.nuget.org/packages/stuff.graph.dijkstra.net)
- [stuff.graph.astar.net](https://www.nuget.org/packages/stuff.graph.astar.net)

**Analysis:**
- [stuff.graph.wcc.net](https://www.nuget.org/packages/stuff.graph.wcc.net)
- [stuff.graph.pagerank.net](https://www.nuget.org/packages/stuff.graph.pagerank.net)
- [stuff.graph.mwst.net](https://www.nuget.org/packages/stuff.graph.mwst.net)

**Graph Generators:**
- [stuff.graph.erdosrenyi.net](https://www.nuget.org/packages/stuff.graph.erdosrenyi.net)
- [stuff.graph.wattsstrogatz.net](https://www.nuget.org/packages/stuff.graph.wattsstrogatz.net)
- [stuff.graph.dorogovtsevmendes.net](https://www.nuget.org/packages/stuff.graph.dorogovtsevmendes.net)

**Utilities:**
- [stuff.graph.cost.net](https://www.nuget.org/packages/stuff.graph.cost.net)
- [stuff.graph.serializable.net](https://www.nuget.org/packages/stuff.graph.serializable.net)
- [stuff.graph.dependencyinjection.net](https://www.nuget.org/packages/stuff.graph.dependencyinjection.net)

## 📞 Contact

For questions or feedback, please open an issue on GitHub.

---

Made with ❤️ for the .NET community