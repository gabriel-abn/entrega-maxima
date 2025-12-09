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

        var path = FleuryAlgorithm(graph, startNode);

        if (path != null && path.Count == graph.EdgeCount)
        {
            result.IsPossible = true;
            result.Path = path;
        }

        return result;
    }

    private static List<Edge>? FleuryAlgorithm(LogisticsGraph graph, int startNode)
    {
        var workingGraph = graph.Clone();
        var path = new List<Edge>();
        var currentNode = startNode;

        while (workingGraph.EdgeCount > 0)
        {
            var outgoingEdges = workingGraph.GetOutgoingEdges(currentNode);
            
            if (outgoingEdges.Count == 0)
            {
                break;
            }

            Edge? chosenEdge = null;

            if (outgoingEdges.Count == 1)
            {
                chosenEdge = outgoingEdges[0];
            }
            else
            {
                foreach (var edge in outgoingEdges)
                {
                    if (!IsBridge(workingGraph, edge))
                    {
                        chosenEdge = edge;
                        break;
                    }
                }

                if (chosenEdge == null)
                {
                    chosenEdge = outgoingEdges[0];
                }
            }

            path.Add(graph.GetOutgoingEdges(currentNode)
                .First(e => e.Source.Id == chosenEdge.Source.Id && 
                           e.Target.Id == chosenEdge.Target.Id));

            currentNode = chosenEdge.Target.Id;
            RemoveEdge(workingGraph, chosenEdge);
        }

        return path;
    }

    private static bool IsBridge(LogisticsGraph graph, Edge edge)
    {
        int reachableBeforeCount = CountReachableNodes(graph, edge.Source.Id);
        
        var tempGraph = graph.Clone();
        RemoveEdge(tempGraph, edge);
        
        int reachableAfterCount = CountReachableNodes(tempGraph, edge.Source.Id);
        
        return reachableAfterCount < reachableBeforeCount;
    }

    private static int CountReachableNodes(LogisticsGraph graph, int startNode)
    {
        var visited = new HashSet<int>();
        var queue = new Queue<int>();

        queue.Enqueue(startNode);
        visited.Add(startNode);

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

        return visited.Count;
    }

    private static void RemoveEdge(LogisticsGraph graph, Edge edgeToRemove)
    {
        var edges = graph.GetOutgoingEdges(edgeToRemove.Source.Id);
        var edgeFound = edges.FirstOrDefault(e => 
            e.Source.Id == edgeToRemove.Source.Id && 
            e.Target.Id == edgeToRemove.Target.Id);
        
        if (edgeFound != null)
        {
            edges.Remove(edgeFound);
        }
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
