using LogisticsOptimization.Models;
using LogisticsOptimization.Algorithms.Results;

namespace LogisticsOptimization.Algorithms;

public static class WelshPowellAlgorithm
{
    public static ColoringResult ScheduleMaintenanceShifts(
        LogisticsGraph graph, 
        List<(int edgeIdx1, int edgeIdx2)> conflicts)
    {
        var result = new ColoringResult();
        var edges = graph.GetAllEdges().ToList();

        if (edges.Count == 0)
            return result;

        var conflictGraph = new Dictionary<int, HashSet<int>>();
        for (int i = 0; i < edges.Count; i++)
        {
            conflictGraph[i] = new HashSet<int>();
        }

        foreach (var (idx1, idx2) in conflicts)
        {
            if (idx1 < edges.Count && idx2 < edges.Count)
            {
                conflictGraph[idx1].Add(idx2);
                conflictGraph[idx2].Add(idx1);
            }
        }

        var degrees = conflictGraph.Select(kvp => new 
        { 
            EdgeIdx = kvp.Key, 
            Degree = kvp.Value.Count 
        }).OrderByDescending(x => x.Degree).ToList();

        var colors = new Dictionary<int, int>();

        foreach (var item in degrees)
        {
            int edgeIdx = item.EdgeIdx;

            var usedColors = new HashSet<int>();
            foreach (var neighbor in conflictGraph[edgeIdx])
            {
                if (colors.ContainsKey(neighbor))
                {
                    usedColors.Add(colors[neighbor]);
                }
            }

            int color = 0;
            while (usedColors.Contains(color))
            {
                color++;
            }

            colors[edgeIdx] = color;
        }

        foreach (var kvp in colors)
        {
            int edgeIdx = kvp.Key;
            int shift = kvp.Value;

            if (!result.Shifts.ContainsKey(shift))
            {
                result.Shifts[shift] = new List<Edge>();
            }

            result.Shifts[shift].Add(edges[edgeIdx]);
        }

        return result;
    }

    public static List<(int, int)> GenerateConflictsFromSharedNodes(LogisticsGraph graph)
    {
        var conflicts = new List<(int, int)>();
        var edges = graph.GetAllEdges().ToList();

        for (int i = 0; i < edges.Count; i++)
        {
            for (int j = i + 1; j < edges.Count; j++)
            {
                if (edges[i].Source.Id == edges[j].Source.Id ||
                    edges[i].Source.Id == edges[j].Target.Id ||
                    edges[i].Target.Id == edges[j].Source.Id ||
                    edges[i].Target.Id == edges[j].Target.Id)
                {
                    conflicts.Add((i, j));
                }
            }
        }

        return conflicts;
    }
}
