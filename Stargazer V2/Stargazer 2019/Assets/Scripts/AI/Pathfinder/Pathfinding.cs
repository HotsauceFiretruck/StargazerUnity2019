using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System;

public class Pathfinding : MonoBehaviour
{

    NodeGrid nodeGrid;

    void Awake()
    {
        nodeGrid = GetComponent<NodeGrid>();
    }

    public void FindPath(PathRequest request, Action<PathResult> callback)
    {

        Vector3[] waypoints = new Vector3[0];
        bool pathSuccess = false;

        Node startNode = this.nodeGrid.NodeFromWorldPoint(request.pathStart);
        Node targetNode = this.nodeGrid.NodeFromWorldPoint(request.pathEnd);

        if (!startNode.walkable)
        {
            List<Node> neighbors = nodeGrid.GetNeighbors(startNode);
            if (neighbors.Count > 0)
            {
                startNode = neighbors[0];
            }
        }

        if (!targetNode.walkable)
        {
            List<Node> neighbors = nodeGrid.GetNeighbors(targetNode);
            if (neighbors.Count > 0)
            {
                targetNode = neighbors[0];
            }
        }

        if (startNode.walkable && targetNode.walkable)
        {

            Heap<Node> openSet = new Heap<Node>(nodeGrid.MaxSize);
            HashSet<Node> closedSet = new HashSet<Node>();
            openSet.Add(startNode);

            while (openSet.Count > 0)
            {
                Node currentNode = openSet.RemoveFirst();
                closedSet.Add(currentNode);

                if (currentNode == targetNode)
                {
                    pathSuccess = true;
                    break;
                }

                foreach (Node neighbor in nodeGrid.GetNeighbors(currentNode))
                {
                    if (closedSet.Contains(neighbor)) continue;

                    int movementCost = currentNode.gCost + this.GetDistance(currentNode, neighbor) + neighbor.weight;
                    if (movementCost < neighbor.gCost || !openSet.Contains(neighbor))
                    {
                        neighbor.gCost = movementCost;
                        neighbor.hCost = this.GetDistance(neighbor, targetNode);
                        neighbor.parent = currentNode;

                        if (!openSet.Contains(neighbor))
                        {
                            openSet.Add(neighbor);
                        }
                        else
                        {
                            openSet.UpdateItem(neighbor);
                        }
                    }
                }
            }
        }
        if (pathSuccess)
        {
            waypoints = RetracePath(startNode, targetNode);
            pathSuccess = waypoints.Length > 0;
        }
        callback(new PathResult(waypoints, pathSuccess, request.callback));
    }

    Vector3[] RetracePath(Node startNode, Node endNode)
    {
        List<Node> path = new List<Node>();
        Node currentNode = endNode;
        while (currentNode != startNode)
        {
            path.Add(currentNode);
            currentNode = currentNode.parent;
        }
        Vector3[] waypoints = SimplifyPath(path);
        Array.Reverse(waypoints);
        return waypoints;
    }

    Vector3[] SimplifyPath(List<Node> path)
    {
        List<Vector3> waypoints = new List<Vector3>();
        Vector2 directionOld = Vector2.zero;

        for (int i = 1; i < path.Count; i++)
        {
            Vector2 directionNew = new Vector2(path[i - 1].gridX - path[i].gridX, path[i - 1].gridY - path[i].gridY);
            if (directionNew != directionOld)
            {
                waypoints.Add(path[i].worldPosition);
            }
            directionOld = directionNew;
        }
        return waypoints.ToArray();
    }

    int GetDistance(Node a, Node b)
    {
        int distX = Mathf.Abs(a.gridX - b.gridX);
        int distY = Mathf.Abs(a.gridY - b.gridY);

        if (distX > distY) return 14 * distY + 10 * (distX - distY);
        return 14 * distX + 10 * (distY - distX);
    }

}
