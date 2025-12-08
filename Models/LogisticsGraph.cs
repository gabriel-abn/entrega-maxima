namespace LogisticsOptimization.Models;

public class LogisticsGraph
{
    private readonly Dictionary<int, Node> _nodes;
    private readonly Dictionary<int, List<Edge>> _adjacencyList;

    public LogisticsGraph()
    {
        _nodes = new Dictionary<int, Node>();
        _adjacencyList = new Dictionary<int, List<Edge>>();
    }

    public void AddNode(Node node)
    {
        if (!_nodes.ContainsKey(node.Id))
        {
            _nodes[node.Id] = node;
            _adjacencyList[node.Id] = new List<Edge>();
        }
    }

    public void AddEdge(Edge edge)
    {
        AddNode(edge.Source);
        AddNode(edge.Target);

        _adjacencyList[edge.Source.Id].Add(edge);
    }

    public Node? GetNode(int id)
    {
        return _nodes.TryGetValue(id, out var node) ? node : null;
    }

    public List<Edge> GetOutgoingEdges(int nodeId)
    {
        return _adjacencyList.TryGetValue(nodeId, out var edges) ? edges : new List<Edge>();
    }

    public List<Edge> GetIncomingEdges(int nodeId)
    {
        var incoming = new List<Edge>();
        foreach (var edges in _adjacencyList.Values)
        {
            incoming.AddRange(edges.Where(e => e.Target.Id == nodeId));
        }
        return incoming;
    }

    public IEnumerable<Node> GetAllNodes()
    {
        return _nodes.Values;
    }

    public IEnumerable<Edge> GetAllEdges()
    {
        return _adjacencyList.Values.SelectMany(edges => edges);
    }

    public int NodeCount => _nodes.Count;

    public int EdgeCount => _adjacencyList.Values.Sum(edges => edges.Count);

    public bool ContainsNode(int nodeId)
    {
        return _nodes.ContainsKey(nodeId);
    }

    public int GetOutDegree(int nodeId)
    {
        return GetOutgoingEdges(nodeId).Count;
    }

    public int GetInDegree(int nodeId)
    {
        return GetIncomingEdges(nodeId).Count;
    }

    public LogisticsGraph Clone()
    {
        var clone = new LogisticsGraph();
        
        foreach (var node in _nodes.Values)
        {
            clone.AddNode(new Node(node.Id, node.Name));
        }

        foreach (var edge in GetAllEdges())
        {
            var sourceClone = clone.GetNode(edge.Source.Id)!;
            var targetClone = clone.GetNode(edge.Target.Id)!;
            clone.AddEdge(new Edge(sourceClone, targetClone, edge.Cost, edge.Capacity));
        }

        return clone;
    }
}
