using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathNode
{
    private Grid<PathNode> grid;
    public int x { private set; get; }
    public int y { private set; get; }

	public bool IsWalkable = true;

	public int gCost;
	public int hCost;
	public int fCost;

	public PathNode previousNode;

    public PathNode(Grid<PathNode> grid, int x, int y)
	{
		this.grid = grid;
		this.x = x;
		this.y = y;
	}

	public override string ToString()
	{
		return x + "," + y;
	}

	public void GetFCost()
	{
		fCost = gCost + hCost;
	}
}
