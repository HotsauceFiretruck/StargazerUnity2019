using UnityEngine;

public class Node : IHeapItem<Node>
{

	public bool walkable;
	public Vector3 worldPosition;
	public Node parent;
	public int gridX;
	public int gridY;
	public int weight;

	public int gCost;
	public int hCost;
	int heapIndex;

	public Node(bool walkable, Vector3 worldPos, int gridX, int gridY, int weight)
	{
		this.walkable = walkable;
		this.worldPosition = worldPos;
		this.gridX = gridX;
		this.gridY = gridY;
		this.weight = weight;
	}

	public int fCost
	{
		get
		{
			return gCost + hCost;
		}
	}

	public int HeapIndex
	{
		get
		{
			return heapIndex;
		}
		set
		{
			heapIndex = value;
		}
	}

	public int CompareTo(Node node)
	{
		int compare = fCost.CompareTo(node.fCost);
		if (compare == 0)
		{
			compare = hCost.CompareTo(node.hCost);
		}
		return -compare;
	}
}

