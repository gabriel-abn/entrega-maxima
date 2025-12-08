using LogisticsOptimization.Models;

namespace LogisticsOptimization.Algorithms.Results;

public class EulerianResult
{
    public bool IsPossible { get; set; }
    public List<Edge> Path { get; set; }

    public EulerianResult()
    {
        IsPossible = false;
        Path = new List<Edge>();
    }
}
