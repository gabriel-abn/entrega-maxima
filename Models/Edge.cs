namespace LogisticsOptimization.Models;

public class Edge
{
    public Node Source { get; set; }
    public Node Target { get; set; }
    public double Cost { get; set; }
    public double Capacity { get; set; }
    public double Flow { get; set; }

    public Edge(Node source, Node target, double cost, double capacity = double.PositiveInfinity)
    {
        Source = source;
        Target = target;
        Cost = cost;
        Capacity = capacity;
        Flow = 0;
    }

    public double ResidualCapacity => Capacity - Flow;

    public override string ToString()
    {
        if (Capacity == double.PositiveInfinity)
            return $"{Source.Id} -> {Target.Id} (Cost: {Cost:F2})";
        return $"{Source.Id} -> {Target.Id} (Cost: {Cost:F2}, Capacity: {Capacity:F2})";
    }

    public Edge Clone()
    {
        return new Edge(Source, Target, Cost, Capacity) { Flow = Flow };
    }
}
