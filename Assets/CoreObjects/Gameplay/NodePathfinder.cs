using UnityEngine;
using System.Collections.Generic;

public class NodePathfinder
{
    private Node m_current;
    private Node m_destination;
    private Node m_next;

    public NodePathfinder(Node current, Node destination)
    {
        m_current = current;
        m_destination = destination;

        m_next = DoPathfind();
    }

    // A*
    private Node DoPathfind()
    {
        // Prepare A*
        Node startNode = m_current;
        Node endNode = m_destination;

        Node current;
        List<Node> frontier = new List<Node>();
        Dictionary<Node, float> nodePriority = new Dictionary<Node, float>();
        Dictionary<Node, float> currentCost = new Dictionary<Node, float>();
        Dictionary<Node, Node> cameFrom = new Dictionary<Node, Node>();

        // Start the algorithm
        frontier.Add(startNode);
        nodePriority.Add(startNode, 0);
        currentCost.Add(startNode, 0);
        cameFrom.Add(startNode, null);

        while (frontier.Count > 0)
        {
            current = nodePriority.GetSmallest();
            nodePriority.Remove(current);

            if (current == endNode)
                break;

            foreach (Node next in current.Connections)
            {
                float newCost = currentCost[current] + 1;

                if (!currentCost.ContainsKey(next) || newCost < currentCost[next])
                {
                    currentCost[next] = newCost;

                    float priority = newCost + 1;

                    if (!nodePriority.ContainsKey(next))
                        nodePriority.Add(next, priority);
                    else
                        nodePriority[next] = priority;

                    frontier.Add(next);

                    if (!cameFrom.ContainsKey(next))
                        cameFrom.Add(next, current);
                    else
                        cameFrom[next] = current;
                }
            }
        }

        // Start backtracking through cameFrom to find the cell
        List<Node> path = new List<Node>();

        current = endNode;
        while (cameFrom[current] != null)
        {
            path.Add(current);
            current = cameFrom[current];
        }

        // Add original start and end positions to path
        path.Add(startNode);
        path.Reverse();
        path.Add(endNode);

        for (int i = 0; i < path.Count - 1; ++i)
        {
            Debug.Log(path[i].name);
            Debug.DrawLine(path[i].transform.position, path[i + 1].transform.position, Color.green);
        }

        return path[1];
    }

    public Node Next => m_next;
}
