using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UIElements;

public class MapGrid
{
	public Material defaultMaterial;
	public float waterLevel = .4f;
	public float cellSize;
	public int width, height;
	public float scale;

	public Grid<MapCell> Grid { private set; get; }

	public MapGrid(int width, int height, float cellSize, float scale, float waterLevel) {
		Grid = new Grid<MapCell>(width, height, cellSize, Vector3.zero, (Grid<MapCell> g, int x, int y) => new MapCell(g, x, y));
		this.width = width;
		this.height = height;
		this.scale = scale;
		this.waterLevel = waterLevel;
		this.cellSize = cellSize;
		SetNoise();
	}
	public void Start()
	{
		Grid = new Grid<MapCell>(width, height, cellSize, Vector3.zero, (Grid<MapCell> g, int x, int y) => new MapCell(g, x, y));
		SetNoise();
		//DrawTerrainMesh();
	}



	private void SetNoise()
	{
		for (int x = 0 ; x < Grid.Width; x++)
		{
			for( int y = 0 ; y < Grid.Height; y++)
			{
				Grid.GetGridObject(x, y).IsWater = Mathf.PerlinNoise(x * scale, y * scale) < waterLevel;
			}
		}
	}

	public void DrawTerrainMesh(out List<Vector3> vertices, out List<int> triangles)
	{
		//Mesh mesh = new Mesh();
		vertices = new List<Vector3>();
		triangles = new List<int>();	
		for (int x= 0 ; x < Grid.Width; x++)
		{

			for (int y= 0 ; y < Grid.Height;y++)
			{
				MapCell cell = Grid.GetGridObject(x, y);
				if (!cell.IsWater)
				{
					Vector3 a = new Vector3(x, y + cellSize);
					
					Vector3 b = new Vector3(x + cellSize, y + cellSize);
					Vector3 c = new Vector3(x+ cellSize,y);
					Vector3 d = new Vector3(x, y);
					Vector3[] v = new Vector3[] { a, c, d, a, b, c };
					Debug.Log($"{a} {b} {c} {d} {v}");
					for (int k =0; k < 6; k++)
					{
						vertices.Add(v[k]);
						triangles.Add(triangles.Count);
					}


				}
			}
		}
		//mesh.vertices = vertices.ToArray();
		//mesh.triangles = triangles.ToArray();
		//mesh.RecalculateNormals();

		//MeshFilter meshFilter = gameObject.AddComponent<MeshFilter>();
		//meshFilter.mesh = mesh;

		//MeshRenderer meshRenderer = gameObject.AddComponent<MeshRenderer>();
		



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

