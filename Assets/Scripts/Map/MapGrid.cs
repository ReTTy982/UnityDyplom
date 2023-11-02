using System.Runtime.CompilerServices;
using Unity.Mathematics;
using UnityEngine;

public class MapGrid 
{
	public float waterLevel = .4f;
	public float cellSize;
	private float[,] noiseMap;
	public Grid<MapCell> Grid { private set; get; }

	public MapGrid(int width, int height, float cellSize, float[,] noise) {
		Grid = new Grid<MapCell>(width, height, cellSize, Vector3.zero, (Grid<MapCell> g, int x, int y) => new MapCell(g, x, y));
		this.noiseMap = noise;
		Debug.Log("Constructor");
		SetNoise();
	}

	private void SetNoise()
	{
		Debug.Log($"SetNoise({Grid.Width} {Grid.Height})");
		for (int x = 0 ; x < Grid.Width; x++)
		{
			for( int y = 0 ; y < Grid.Height; y++)
			{
				Debug.Log("LEVEL");
				Grid.GetGridObject(x, y).IsWater = noiseMap[x, y] < waterLevel;
			}
		}
	}


}
public class MapCell
{
	private Grid<MapCell> grid;
	int x;
	int y;
	public bool IsWater { get; set; }
	public MapCell(Grid<MapCell> grid, int x, int y)
	{
		this.grid = grid;
		this.x = x;
		this.y = y;
		IsWater = true;

	}
}

