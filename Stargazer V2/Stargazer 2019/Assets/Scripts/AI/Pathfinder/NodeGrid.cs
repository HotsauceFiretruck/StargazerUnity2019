using System.Collections.Generic;
using UnityEngine;
using System;

public class NodeGrid : MonoBehaviour
{

    public bool displayGizmos;

    public LayerMask unwalkableMask;
    public Vector2 gridWorldSize;
    public float nodeRadius;
    Node[,] grid;

    float nodeDiameter;
    int gridSizeX, gridSizeY;

    void Awake()
    {
        this.nodeDiameter = this.nodeRadius * 2;
        this.gridSizeX = Mathf.RoundToInt(this.gridWorldSize.x / this.nodeDiameter);
        this.gridSizeY = Mathf.RoundToInt(this.gridWorldSize.y / this.nodeDiameter);
        this.CreateGrid();
    }

    public int MaxSize
    {
        get
        {
            return gridSizeX * gridSizeY;
        }
    }

    void CreateGrid()
    {
        this.grid = new Node[gridSizeX, gridSizeY];
        Vector3 worldBottomLeft = transform.position - Vector3.right * this.gridWorldSize.x / 2 - Vector3.forward * this.gridWorldSize.y / 2;
        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                Vector3 worldPoint = worldBottomLeft +
                    Vector3.right * (x * this.nodeDiameter + this.nodeRadius) +
                    Vector3.forward * (y * this.nodeDiameter + this.nodeRadius);
                Collider[] collide = Physics.OverlapSphere(worldPoint, nodeRadius, unwalkableMask);
                bool walkable = true;
                if (collide.Length > 0)
                {
                    walkable = false;
                }

                this.grid[x, y] = new Node(walkable, worldPoint, x, y, walkable ? 0 : 15);

                if (!walkable)
                {
                    foreach (Collider c in collide)
                    {
                        AmbushWall wall = c.GetComponent<AmbushWall>();
                        if (wall != null)
                        {
                            wall.AddPathFindNode(this.grid[x, y]);
                        }
                    }
                }
            }
        }

        BlurWeightMap(3);
    }

    void BlurWeightMap(int blurSize)
    {
        int kernelSize = blurSize * 2 + 1;
        int kernelExtents = (kernelSize - 1) / 2;

        int[,] weightHozPass = new int[gridSizeX, gridSizeY];
        int[,] weightVerPass = new int[gridSizeX, gridSizeY];
        for (int y = 0; y < gridSizeY; y++)
        {
            for (int x = -kernelExtents; x <= kernelExtents; x++)
            {
                int sampleX = Mathf.Clamp(x, 0, kernelExtents);
                weightHozPass[0, y] += grid[sampleX, y].weight;
            }

            for (int x = 1; x < gridSizeX; x++)
            {
                int removeIndex = Mathf.Clamp(x - kernelExtents - 1, 0, gridSizeX);
                int addIndex = Mathf.Clamp(x + kernelExtents, 0, gridSizeX - 1);

                weightHozPass[x, y] = weightHozPass[x - 1, y] - grid[removeIndex, y].weight + grid[addIndex, y].weight;
            }
        }

        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = -kernelExtents; y <= kernelExtents; y++)
            {
                int sampleY = Mathf.Clamp(y, 0, kernelExtents);
                weightVerPass[x, 0] += weightHozPass[x, sampleY];
            }

            int blurredWeight = Mathf.RoundToInt((float)weightVerPass[x, 0] / (kernelSize * kernelSize));
            grid[x, 0].weight = blurredWeight;
            for (int y = 1; y < gridSizeY; y++)
            {
                int removeIndex = Mathf.Clamp(y - kernelExtents - 1, 0, gridSizeX);
                int addIndex = Mathf.Clamp(y + kernelExtents, 0, gridSizeY - 1);
                weightVerPass[x, y] = weightVerPass[x, y - 1] - weightHozPass[x, removeIndex] + weightHozPass[x, addIndex];
                blurredWeight = Mathf.RoundToInt((float)weightVerPass[x, y] / (kernelSize * kernelSize));
                grid[x, y].weight = blurredWeight;
            }
        }
    }

    public List<Node> GetNeighbors(Node node)
    {
        List<Node> neighbors = new List<Node>();

        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if (x == 0 && y == 0) continue;
                int checkX = node.gridX + x;
                int checkY = node.gridY + y;
                if (checkX >= 0 && checkX < gridSizeX && checkY >= 0 && checkY < gridSizeY)
                {
                    if (!this.grid[checkX, checkY].walkable) continue;
                    neighbors.Add(this.grid[checkX, checkY]);
                }
            }
        }

        return neighbors;
    }

    public Node NodeFromWorldPoint(Vector3 worldPosition)
    {
        float percentX = (worldPosition.x + this.gridWorldSize.x / 2) / this.gridWorldSize.x;
        float percentY = (worldPosition.z + this.gridWorldSize.y / 2) / this.gridWorldSize.y;
        percentX = Mathf.Clamp01(percentX);
        percentY = Mathf.Clamp01(percentY);

        int x = Mathf.RoundToInt((this.gridSizeX - 1) * percentX);
        int y = Mathf.RoundToInt((this.gridSizeY - 1) * percentY);
        return this.grid[x, y];
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(gridWorldSize.x, 1, gridWorldSize.y));

        if (this.grid != null && displayGizmos)
        {
            foreach (Node n in grid)
            {
                Gizmos.color = n.walkable ? Color.white : Color.red;
                Gizmos.DrawCube(n.worldPosition, Vector3.one * (this.nodeDiameter - .1f));
                // 	if (n.walkable)
                // 		Handles.Label(n.worldPosition, n.weight.ToString());
            }
        }

    }
}
