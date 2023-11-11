using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class GameGrid
{
	public int Width { get; set; }
	public int Height { get; set; }
	public Grid<GameSquare> Grid;
	public int cellsInCell = 2;
	public int chunksPerLine = 4;
	public int cellSize = 2;


	public GameGrid(int mapChunkSize, int chunkCount, Grid<MapCell> mapGrid)
	{
		Width = chunksPerLine * mapChunkSize * cellsInCell;
		Height = (chunkCount / chunksPerLine) * mapChunkSize * cellsInCell;

		Grid = new Grid<GameSquare>(Width*cellsInCell/2, Height*cellsInCell/2,cellSize,Vector3.zero, (Grid<GameSquare> g, int x, int y) => new GameSquare(g, x, y));
		for (int x = 0; x < mapGrid.Width; x++)
		{
			for (int y=0; y < mapGrid.Height; y++)
			{
				int localX = x *cellsInCell;
				int localY = y *cellsInCell;
				bool walkbale = !mapGrid.GetGridObject(x, y).IsWater;

				Grid.GetGridObject(localX, localY).SetTraverse(walkbale);
				Grid.GetGridObject(localX + 1, localY).SetTraverse(walkbale);
				Grid.GetGridObject(localX, localY + 1).SetTraverse(walkbale);
				Grid.GetGridObject(localX + 1, localY + 1).SetTraverse(walkbale);


			}
		}
	}

}

public class GameSquare
{
	Grid<GameSquare> grid;
	public PathNode PathNode { get; set; }
	public int x;
	public int y;
	public bool isWalkable;

	public GameSquare(Grid<GameSquare>grid ,int x, int y)
	{
		this.grid = grid;
		this.x = x;
		this.y = y;
		PathNode = new PathNode(x, y, true);


	}

	public void SetTraverse(bool walkable)
	{
		isWalkable = walkable;
		PathNode.IsWalkable = isWalkable;
	}
}


