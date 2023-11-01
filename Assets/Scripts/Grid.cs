using UnityEngine;
using System.Collections;

public class Grid : MonoBehaviour
{
	public Transform player;
	public LayerMask unwalkableMask;
	public Vector2 gridWorldSize;
	public float nodeRadius;
	Node[,] grid;

	float nodeDiameter;
	int gridSizeX, gridSizeY;

	void Start()
	{
		nodeDiameter = nodeRadius * 2;
		gridSizeX = Mathf.RoundToInt(gridWorldSize.x / nodeDiameter);
		gridSizeY = Mathf.RoundToInt(gridWorldSize.y / nodeDiameter);
		CreateGrid();
	}

	void CreateGrid()
	{
		grid = new Node[gridSizeX, gridSizeY];
		Vector3 worldBottomLeft = transform.position - Vector3.right * gridWorldSize.x / 2 - Vector3.up * gridWorldSize.y / 2;

		for (int x = 0; x < gridSizeX; x++)
		{
			for (int y = 0; y < gridSizeY; y++)
			{
				Vector3 worldPoint = worldBottomLeft + Vector3.right * (x * nodeDiameter + nodeRadius) + Vector3.up * (y * nodeDiameter + nodeRadius);

				bool walkable = !(Physics2D.OverlapCircle(worldPoint,nodeRadius,unwalkableMask));
				grid[x, y] = new Node(walkable, worldPoint);
			}
		}
	}


	public Node NodeFromWorldPoint(Vector3 worldPosition)
	{
		float percentX = (worldPosition.x + gridWorldSize.x / 2) / gridWorldSize.x;
		float percentY = (worldPosition.y + gridWorldSize.y / 2) / gridWorldSize.y;

		int x = Mathf.Clamp(Mathf.FloorToInt((worldPosition.x - transform.position.x + gridWorldSize.x / 2) / nodeDiameter), 0, gridSizeX - 1);
		int y = Mathf.Clamp(Mathf.FloorToInt((worldPosition.y - transform.position.y + gridWorldSize.y / 2) / nodeDiameter), 0, gridSizeY - 1);

	

		return grid[x, y];
	}

	void OnDrawGizmos()
	{
		Gizmos.DrawWireCube(transform.position, new Vector3(gridWorldSize.x, gridWorldSize.y,1));


		if (grid != null)
		{
			Node playerNode = NodeFromWorldPoint(player.position);
			foreach (Node n in grid)
			{
				Gizmos.color = (n.walkable) ? Color.white : Color.red;
				if (playerNode == n)
				{
					Gizmos.color = Color.blue;
				}
				Gizmos.DrawCube(n.WorldPosition, Vector3.one * (nodeDiameter - .1f));
				
			}

		}
	}
}

public class Node
{

	public bool walkable;
	public Vector3 WorldPosition { get; }

	public Node(bool _walkable, Vector3 _worldPos)
	{
		walkable = _walkable;
		WorldPosition = _worldPos;
	}
}