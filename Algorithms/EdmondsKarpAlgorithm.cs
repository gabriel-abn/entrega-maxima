using LogisticsOptimization.Models;
using LogisticsOptimization.Algorithms.Results;

namespace LogisticsOptimization.Algorithms;

public static class EdmondsKarpAlgorithm
{
    public static MaxFlowResult CalculateMaxFlow(LogisticsGraph graph, int sourceId, int sinkId)
    {
        var result = new MaxFlowResult();

        if (!graph.ContainsNode(sourceId) || !graph.ContainsNode(sinkId))
        {
            return result;
        }

        var workingGraph = graph.Clone();
        var residualCapacity = new Dictionary<(int, int), double>();
        
        foreach (var edge in workingGraph.GetAllEdges())
        {
            residualCapacity[(edge.Source.Id, edge.Target.Id)] = edge.Capacity;
            if (!residualCapacity.ContainsKey((edge.Target.Id, edge.Source.Id)))
            {
                residualCapacity[(edge.Target.Id, edge.Source.Id)] = 0;
            }
        }

        double maxFlow = 0;

        while (true)
        {
            var parent = BFS(workingGraph, sourceId, sinkId, residualCapacity);
            
            if (parent == null)
                break;

            double pathFlow = double.PositiveInfinity;
            int current = sinkId;

            while (current != sourceId)
            {
                int prev = parent[current];
                pathFlow = Math.Min(pathFlow, residualCapacity[(prev, current)]);
                current = prev;
            }

            current = sinkId;
            while (current != sourceId)
            {
                int prev = parent[current];
                residualCapacity[(prev, current)] -= pathFlow;
                residualCapacity[(current, prev)] += pathFlow;
                current = prev;
            }

            maxFlow += pathFlow;
        }

        result.MaxFlow = maxFlow;

        var reachable = GetReachableNodes(workingGraph, sourceId, residualCapacity);
        
        foreach (var edge in graph.GetAllEdges())
        {
            if (reachable.Contains(edge.Source.Id) && 
                !reachable.Contains(edge.Target.Id))
            {
                result.BottleneckEdges.Add(edge);
            }
        }

        return result;
    }

    private static Dictionary<int, int>? BFS(
        LogisticsGraph graph, 
        int sourceId, 
        int sinkId,
        Dictionary<(int, int), double> residualCapacity)
    {
        var visited = new HashSet<int>();
        var queue = new Queue<int>();
        var parent = new Dictionary<int, int>();

        queue.Enqueue(sourceId);
        visited.Add(sourceId);

        while (queue.Count > 0)
        {
            int current = queue.Dequeue();

            if (current == sinkId)
            {
                return parent;
            }

            foreach (var node in graph.GetAllNodes())
            {
                int neighbor = node.Id;
                
                if (!visited.Contains(neighbor) && 
                    residualCapacity.ContainsKey((current, neighbor)) &&
                    residualCapacity[(current, neighbor)] > 0)
                {
                    visited.Add(neighbor);
                    parent[neighbor] = current;
                    queue.Enqueue(neighbor);
                }
            }
        }

        return null;
    }

    private static HashSet<int> GetReachableNodes(
        LogisticsGraph graph,
        int sourceId,
        Dictionary<(int, int), double> residualCapacity)
    {
        var reachable = new HashSet<int>();
        var queue = new Queue<int>();

        queue.Enqueue(sourceId);
        reachable.Add(sourceId);

        while (queue.Count > 0)
        {
            int current = queue.Dequeue();

            foreach (var node in graph.GetAllNodes())
            {
                int neighbor = node.Id;
                
                if (!reachable.Contains(neighbor) &&
                    residualCapacity.ContainsKey((current, neighbor)) &&
                    residualCapacity[(current, neighbor)] > 0)
                {
                    reachable.Add(neighbor);
                    queue.Enqueue(neighbor);
                }
            }
        }

        return reachable;
    }
}
