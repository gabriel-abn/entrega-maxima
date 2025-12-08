namespace LogisticsOptimization.Models;

public class Node
{
    public int Id { get; set; }
    public string Name { get; set; }

    public Node(int id, string name = "")
    {
        Id = id;
        Name = string.IsNullOrEmpty(name) ? $"Hub_{id}" : name;
    }

    public override string ToString() => $"Node {Id} ({Name})";

    public override bool Equals(object? obj)
    {
        return obj is Node node && Id == node.Id;
    }

    public override int GetHashCode() => Id.GetHashCode();
}
