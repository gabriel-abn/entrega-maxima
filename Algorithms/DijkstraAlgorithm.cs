using LogisticsOptimization.Models;
using LogisticsOptimization.Algorithms.Results;

namespace LogisticsOptimization.Algorithms;

public static class DijkstraAlgorithm
{
    public static DijkstraResult FindShortestPath(LogisticsGraph graph, int sourceId, int targetId)
    {
        var result = new DijkstraResult();

        if (!graph.ContainsNode(sourceId) || !graph.ContainsNode(targetId))
        {
            return result;
        }

        var distances = new Dictionary<int, double>();
        var predecessors = new Dictionary<int, Edge?>();
        var visited = new HashSet<int>();

        foreach (var node in graph.GetAllNodes())
        {
            distances[node.Id] = double.PositiveInfinity;
            predecessors[node.Id] = null;
        }
        distances[sourceId] = 0;

        var pq = new PriorityQueue<int, double>();
        pq.Enqueue(sourceId, 0);

        while (pq.Count > 0)
        {
            var currentId = pq.Dequeue();

            if (visited.Contains(currentId))
                continue;

            visited.Add(currentId);

            if (currentId == targetId)
                break;

            foreach (var edge in graph.GetOutgoingEdges(currentId))
            {
                int neighborId = edge.Target.Id;
                double newDistance = distances[currentId] + edge.Cost;

                if (newDistance < distances[neighborId])
                {
                    distances[neighborId] = newDistance;
                    predecessors[neighborId] = edge;
                    pq.Enqueue(neighborId, newDistance);
                }
            }
        }

        if (distances[targetId] == double.PositiveInfinity)
        {
            return result;
        }

        result.TotalCost = distances[targetId];
        result.Success = true;

        var path = new List<Edge>();
        int current = targetId;

        while (current != sourceId)
        {
            var edge = predecessors[current];
            if (edge == null)
                break;

            path.Add(edge);
            current = edge.Source.Id;
        }

        path.Reverse();
        result.Path = path;

        return result;
    }
}
