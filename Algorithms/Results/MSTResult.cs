using LogisticsOptimization.Models;

namespace LogisticsOptimization.Algorithms.Results;

public class MSTResult
{
    public double TotalCost { get; set; }
    public List<Edge> Edges { get; set; }

    public MSTResult()
    {
        TotalCost = 0;
        Edges = new List<Edge>();
    }
}
