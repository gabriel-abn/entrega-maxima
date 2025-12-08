using LogisticsOptimization.Models;
using LogisticsOptimization.Algorithms.Results;
using LogisticsOptimization.Utils;

namespace LogisticsOptimization.Algorithms;

public static class KruskalAlgorithm
{
    public static MSTResult FindMinimumSpanningTree(LogisticsGraph graph)
    {
        var result = new MSTResult();

        var edges = graph.GetAllEdges().OrderBy(e => e.Cost).ToList();
        
        var nodeIds = graph.GetAllNodes().Select(n => n.Id).OrderBy(id => id).ToList();
        var idToIndex = new Dictionary<int, int>();
        for (int i = 0; i < nodeIds.Count; i++)
        {
            idToIndex[nodeIds[i]] = i;
        }

        var uf = new UnionFind(nodeIds.Count);

        foreach (var edge in edges)
        {
            int sourceIdx = idToIndex[edge.Source.Id];
            int targetIdx = idToIndex[edge.Target.Id];

            if (uf.Union(sourceIdx, targetIdx))
            {
                result.Edges.Add(edge);
                result.TotalCost += edge.Cost;

                if (result.Edges.Count == nodeIds.Count - 1)
                    break;
            }
        }

        return result;
    }
}
