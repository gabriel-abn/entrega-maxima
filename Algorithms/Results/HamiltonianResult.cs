using LogisticsOptimization.Models;

namespace LogisticsOptimization.Algorithms.Results;

public class HamiltonianResult
{
    public bool IsPossible { get; set; }
    public List<Node> Cycle { get; set; }

    public HamiltonianResult()
    {
        IsPossible = false;
        Cycle = new List<Node>();
    }
}
