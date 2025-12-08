using LogisticsOptimization.Models;

namespace LogisticsOptimization.Algorithms.Results;

public class DijkstraResult
{
    public double TotalCost { get; set; }
    public List<Edge> Path { get; set; }
    public bool Success { get; set; }

    public DijkstraResult()
    {
        Path = new List<Edge>();
        Success = false;
        TotalCost = double.PositiveInfinity;
    }
}
