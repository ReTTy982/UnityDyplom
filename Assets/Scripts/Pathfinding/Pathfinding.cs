using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Pathfinding
{
	public Grid<PathNode> Grid { private set; get; }
	private List<PathNode> openList;
	private List<PathNode> closedList;

	private int straight = 10;
	private int diagonal = 14;

	public static Pathfinding Instance { get; private set; } // SIngleton for single unit ??
	public Pathfinding(int width, int height, float cellSize) 
	{
		Instance = this;
		Grid = new Grid<PathNode>(width, height,cellSize,Vector3.zero,(Grid<PathNode> g, int x, int y) => new PathNode(g,x,y));

	}


	public List<Vector3> FindRandomPath(Vector3 startWorldPosition)
	{
		Grid.GetXY(startWorldPosition, out int startX, out int startY);
		int endX = UnityEngine.Random.Range(0, Grid.Width);
		int endY = UnityEngine.Random.Range(0, Grid.Height);
		List<PathNode> path = FindPath(startX, startY, endX, endY);
		if (path == null)
		{
			return null;
		}
		else
		{
			List<Vector3> vectorPath = new List<Vector3>();
			foreach (PathNode pathNode in path)
			{
				vectorPath.Add(new Vector3(pathNode.x, pathNode.y) * Grid.CellSize + Vector3.one * .5f);
			}
			return vectorPath;
		}
	}
	public List<Vector3> FindPath(Vector3 startWorldPosition, Vector3 endWorldPosition)
	{
		
		Grid.GetXY(startWorldPosition, out int startX, out int startY);
		Grid.GetXY(endWorldPosition, out int endX, out int endY);
		List<PathNode> path = FindPath(startX,startY,endX,endY);
		if (path == null)
		{
			return null;
		}
		else
		{
			List<Vector3> vectorPath = new List<Vector3>();
			foreach(PathNode pathNode in path) 
			{
				vectorPath.Add(new Vector3(pathNode.x, pathNode.y) * Grid.CellSize + Vector3.one * .5f);
			}
			return vectorPath;
		}
		
	}

	public List<PathNode> FindPath(int startX, int startY, int endX, int endY)
	{
		if (endX < 0 || endY < 0) return null;
		PathNode startNode = Grid.GetGridObject(startX, startY);
		PathNode endNode = Grid.GetGridObject(endX, endY);

		openList = new List<PathNode>{startNode};
		closedList = new List<PathNode>();


		for (int x = 0; x<Grid.Width; x++)
		{
			for (int y = 0; y<Grid.Height; y++)
			{
				PathNode pathNode = Grid.GetGridObject(x, y);
				pathNode.gCost = int.MaxValue;
				pathNode.GetFCost();
				pathNode.previousNode = null;

			}
		}

		startNode.gCost = 0;
		startNode.hCost = GetDistance(startNode, endNode);
		startNode.GetFCost();

		while (openList.Count > 0)
		{
			PathNode currentNode = GetLowestNode(openList);

			if (currentNode == endNode)
			{
				return GetPath(endNode);
			}

			openList.Remove(currentNode);
			closedList.Add(currentNode);

			foreach( PathNode neighbour in GetNeighbours(currentNode))
			{
				if (closedList.Contains(neighbour))
				{
					continue;
				}
				if (!neighbour.IsWalkable)
				{
					closedList.Add(neighbour);
					continue;
				}

				int tempGCost = currentNode.gCost + GetDistance(currentNode,neighbour);
				if (tempGCost < neighbour.gCost)
				{
					neighbour.previousNode = currentNode;
					neighbour.gCost = tempGCost;
					neighbour.hCost = GetDistance(neighbour,endNode);
					neighbour.GetFCost();

					if (!openList.Contains(neighbour))
					{
						openList.Add(neighbour);
					}
				}
			}

		}
		return null;

	}

	private List<PathNode> GetNeighbours(PathNode selectedNode)
	{
		List<PathNode> neighbours = new List<PathNode>();
		if (selectedNode.x - 1 >= 0)
		{
			// Left
			neighbours.Add(GetNode(selectedNode.x - 1, selectedNode.y));
			// Left Down
			if (selectedNode.y - 1 >= 0)
			{
				neighbours.Add(GetNode(selectedNode.x - 1, selectedNode.y - 1));
			}
			// Left Up
			if (selectedNode.y + 1 < Grid.Height)
			{
				neighbours.Add(GetNode(selectedNode.x - 1, selectedNode.y + 1));
			}

		}
		if (selectedNode.x + 1 < Grid.Width)
		{
			// Right
			neighbours.Add(GetNode(selectedNode.x + 1, selectedNode.y));

			// Right Down
			if (selectedNode.y - 1 >= 0)
			{
				neighbours.Add(GetNode(selectedNode.x + 1, selectedNode.y - 1));
			}
			// Right Up
			if (selectedNode.y + 1 < Grid.Height)
			{
				neighbours.Add(GetNode(selectedNode.x + 1, selectedNode.y + 1));
			}

			// Down
			if (selectedNode.y - 1 >= 0)
			{
				neighbours.Add(GetNode(selectedNode.x, selectedNode.y - 1));
			}
			// Up
			if (selectedNode.y + 1 < Grid.Height)
			{
				neighbours.Add(GetNode(selectedNode.x, selectedNode.y + 1));
			}
			

		}
		Debug.Assert(neighbours.Any());
		return neighbours;
	}

	private PathNode GetNode(int x, int y)
	{
		return Grid.GetGridObject(x, y);
	}

	private List<PathNode> GetPath(PathNode endNode)
	{
		List<PathNode> path = new List<PathNode>();
		path.Add(endNode);
		PathNode currentNode = endNode;
		while (currentNode.previousNode != null) 
		{ 
			path.Add(currentNode.previousNode);
			currentNode = currentNode.previousNode;
		}
		path.Reverse();
		return path;

	}

	private int GetDistance(PathNode a, PathNode b)
	{
		Debug.Assert(a !=null);
		Debug.Assert(b != null);
		int x = Mathf.Abs(a.x - b.x);
		int y = Mathf.Abs(a.y - b.y);
		int distance = Mathf.Abs(x - y);
		return diagonal  *Mathf.Min(x,y) + straight * distance;
	}

	private PathNode GetLowestNode(List<PathNode> pathNodes)
	{
		PathNode lowestNode = pathNodes[0];
		for (int i =1; i < pathNodes.Count; i++)
		{
			if (pathNodes[i].fCost < lowestNode.fCost)
			{
				lowestNode = pathNodes[i];
			}
		}
		return lowestNode;
	}




}