using LogisticsOptimization.Models;
using LogisticsOptimization.Algorithms.Results;
using System.Diagnostics;

namespace LogisticsOptimization.Algorithms;

public static class HamiltonianAlgorithm
{
    public static HamiltonianResult FindHamiltonianCycle(LogisticsGraph graph, int timeoutSeconds = 10)
    {
        var result = new HamiltonianResult();
        var nodes = graph.GetAllNodes().ToList();

        if (nodes.Count == 0)
            return result;

        var path = new List<Node> { nodes[0] };
        var visited = new HashSet<int> { nodes[0].Id };
        var stopwatch = Stopwatch.StartNew();

        bool found = BacktrackHamiltonian(
            graph, 
            nodes[0].Id, 
            path, 
            visited, 
            nodes.Count,
            stopwatch,
            timeoutSeconds * 1000);

        if (found)
        {
            path.Add(nodes[0]);
            
            result.IsPossible = true;
            result.Cycle = path;
        }

        return result;
    }

    private static bool BacktrackHamiltonian(
        LogisticsGraph graph,
        int currentNodeId,
        List<Node> path,
        HashSet<int> visited,
        int totalNodes,
        Stopwatch stopwatch,
        long timeoutMs)
    {
        if (stopwatch.ElapsedMilliseconds > timeoutMs)
        {
            return false;
        }

        if (path.Count == totalNodes)
        {
            int startNodeId = path[0].Id;
            
            var edgesToStart = graph.GetOutgoingEdges(currentNodeId)
                .Where(e => e.Target.Id == startNodeId);
            
            return edgesToStart.Any();
        }

        foreach (var edge in graph.GetOutgoingEdges(currentNodeId))
        {
            int neighborId = edge.Target.Id;

            if (!visited.Contains(neighborId))
            {
                visited.Add(neighborId);
                path.Add(edge.Target);

                if (BacktrackHamiltonian(graph, neighborId, path, visited, totalNodes, stopwatch, timeoutMs))
                {
                    return true;
                }

                visited.Remove(neighborId);
                path.RemoveAt(path.Count - 1);
            }
        }

        return false;
    }
}
