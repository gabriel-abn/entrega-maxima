using LogisticsOptimization.Models;

namespace LogisticsOptimization.Algorithms.Results;

public class MaxFlowResult
{
    public double MaxFlow { get; set; }
    public List<Edge> BottleneckEdges { get; set; }

    public MaxFlowResult()
    {
        MaxFlow = 0;
        BottleneckEdges = new List<Edge>();
    }
}
