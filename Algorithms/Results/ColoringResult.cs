using LogisticsOptimization.Models;

namespace LogisticsOptimization.Algorithms.Results;

public class ColoringResult
{
    public Dictionary<int, List<Edge>> Shifts { get; set; }

    public ColoringResult()
    {
        Shifts = new Dictionary<int, List<Edge>>();
    }
}
