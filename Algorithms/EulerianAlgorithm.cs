using LogisticsOptimization.Models;
using LogisticsOptimization.Algorithms.Results;

namespace LogisticsOptimization.Algorithms;

public static class EulerianAlgorithm
{
    public static EulerianResult FindEulerianPath(LogisticsGraph graph)
    {
        var result = new EulerianResult();

        int oddDegreeCount = 0;
        int startNode = -1;

        foreach (var node in graph.GetAllNodes())
        {
            int inDegree = graph.GetInDegree(node.Id);
            int outDegree = graph.GetOutDegree(node.Id);

            if (inDegree != outDegree)
            {
                oddDegreeCount++;
                
                if (outDegree == inDegree + 1)
                {
                    startNode = node.Id;
                }
            }
        }

        if (oddDegreeCount > 2)
        {
            return result;
        }

        if (oddDegreeCount == 0)
        {
            startNode = graph.GetAllNodes().First().Id;
        }

        if (!IsWeaklyConnected(graph))
        {
            return result;
        }

        var path = HierholzerAlgorithm(graph, startNode);

        if (path != null && path.Count == graph.EdgeCount)
        {
            result.IsPossible = true;
            result.Path = path;
        }

        return result;
    }

    private static List<Edge>? HierholzerAlgorithm(LogisticsGraph graph, int startNode)
    {
        var remainingEdges = new Dictionary<int, List<Edge>>();
        foreach (var node in graph.GetAllNodes())
        {
            remainingEdges[node.Id] = new List<Edge>(graph.GetOutgoingEdges(node.Id));
        }

        var circuit = new List<Edge>();
        var stack = new Stack<int>();
        stack.Push(startNode);

        while (stack.Count > 0)
        {
            int current = stack.Peek();

            if (remainingEdges[current].Count > 0)
            {
                var edge = remainingEdges[current][0];
                remainingEdges[current].RemoveAt(0);
                
                stack.Push(edge.Target.Id);
            }
            else
            {
                if (stack.Count > 1)
                {
                    stack.Pop();
                    int prev = stack.Peek();
                    
                    var edge = graph.GetOutgoingEdges(prev)
                        .FirstOrDefault(e => e.Target.Id == current);
                    
                    if (edge != null)
                    {
                        circuit.Insert(0, edge);
                    }
                }
                else
                {
                    stack.Pop();
                }
            }
        }

        return circuit;
    }

    private static bool IsWeaklyConnected(LogisticsGraph graph)
    {
        var nodes = graph.GetAllNodes().ToList();
        if (nodes.Count == 0)
            return true;

        var visited = new HashSet<int>();
        var queue = new Queue<int>();

        queue.Enqueue(nodes[0].Id);
        visited.Add(nodes[0].Id);

        while (queue.Count > 0)
        {
            int current = queue.Dequeue();

            foreach (var edge in graph.GetOutgoingEdges(current))
            {
                if (!visited.Contains(edge.Target.Id))
                {
                    visited.Add(edge.Target.Id);
                    queue.Enqueue(edge.Target.Id);
                }
            }

            foreach (var edge in graph.GetIncomingEdges(current))
            {
                if (!visited.Contains(edge.Source.Id))
                {
                    visited.Add(edge.Source.Id);
                    queue.Enqueue(edge.Source.Id);
                }
            }
        }

        return visited.Count == nodes.Count;
    }
}
