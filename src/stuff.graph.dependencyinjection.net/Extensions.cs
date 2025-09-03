using Microsoft.Extensions.DependencyInjection;
using stuff.graph.algorithms.net;
using stuff.graph.astar.net;
using stuff.graph.cost.net;
using stuff.graph.dijkstra.net;
using stuff.graph.net;
using stuff.graph.sortingbarrier.net;

namespace stuff.graph.dependencyinjection.net;

public static class Extensions
{
    public static IServiceCollection AddGraphStuff(this IServiceCollection serviceCollection, Func<IServiceProvider, Graph> graphProvider)
    {
        serviceCollection
            .AddScoped<IGraphProvider>(sp => new GenericGraphProvider(() =>  graphProvider.Invoke(sp)));
        
        serviceCollection
            .AddEdgeCostService()
            .AddNodeCostService();
        
        serviceCollection
            .AddScoped<IAStar>(sp =>
            {
                var settings = new AStarSettings(Heuristic.Manhatten);
                var graph = sp.GetRequiredService<IGraphProvider>().GetGraph();
                var nodeCostService = sp.GetRequiredService<INodeCostService>();
                var edgeCostService = sp.GetRequiredService<IEdgeCostService>();
                var service = AStar.Create(new AStarConfig(graph, settings));
                service.Inject(nodeCostService);
                service.Inject(edgeCostService);
                return (IAStar)service;
            })
            .AddScoped<IDijkstra>(sp =>
            {
                var graph = sp.GetRequiredService<IGraphProvider>().GetGraph();
                var nodeCostService = sp.GetRequiredService<INodeCostService>();
                var edgeCostService = sp.GetRequiredService<IEdgeCostService>();
                var service = Dijkstra.Create(new DijkstraConfig(graph));
                service.Inject(nodeCostService);
                service.Inject(edgeCostService);
                return (IDijkstra)service;
            })
            .AddScoped<ISortingBarrier>(sp =>
            {
                var graph = sp.GetRequiredService<IGraphProvider>().GetGraph();
                var nodeCostService = sp.GetRequiredService<INodeCostService>();
                var edgeCostService = sp.GetRequiredService<IEdgeCostService>();
                var service = SortingBarrier.Create(new SortingBarrierConfig(graph, new SortingBarrierSettings()));
                service.Inject(nodeCostService);
                service.Inject(edgeCostService);
                return (ISortingBarrier)service;
            });
        
        return serviceCollection;
    }

    public static IServiceCollection AddNodeCostService(this IServiceCollection collection, uint? defaultCost = null)
    {
        return collection.AddSingleton<INodeCostService>(_ =>
        {
            var service = new NodeCostService();
            service.SetDefaultValue(defaultCost ?? 0);
            return service;
        });
    }

    public static IServiceCollection AddEdgeCostService(this IServiceCollection collection, uint? defaultCost = null)
    {
        return collection.AddSingleton<IEdgeCostService>(_ =>
        {
            var service = new EdgeCostService();
            service.SetDefaultValue(defaultCost ?? 0);
            return service;
        });
    }
}