using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UIElements;

public class MapGrid
{
	private List<Chunk> chunks = new List<Chunk>();
	public Material defaultMaterial;
	public float waterLevel = .4f;
	public float cellSize;
	public float scale;
	public int chunkCount;
	public int mapChunkSize = 100;

	public Grid<MapCell> Grid { private set; get; }

	public MapGrid(int chunkCount, float cellSize, float scale, float waterLevel) {
		Grid = new Grid<MapCell>(chunkCount*mapChunkSize, cellSize, Vector3.zero, (Grid<MapCell> g, int x, int y) => new MapCell(g, x, y));
		this.chunkCount = chunkCount;
		this.scale = scale;
		this.waterLevel = waterLevel;
		this.cellSize = cellSize;
		SetNoise();
	}
	public void Start()
	{
		Grid = new Grid<MapCell>(mapChunkSize*chunkCount,cellSize, Vector3.zero, (Grid<MapCell> g, int x, int y) => new MapCell(g, x, y));
		SetNoise();
		//DrawTerrainMesh();
	}



	private void SetNoise()
	{
		float offsetX = UnityEngine.Random.Range(-10000f, 10000f);
		float offsetY = UnityEngine.Random.Range(-10000f, 10000f);
		for (int x = 0 ; x < Grid.Width; x++)
		{
			for( int y = 0 ; y < Grid.Height; y++)
			{
				Grid.GetGridObject(x, y).IsWater = Mathf.PerlinNoise(x * scale +offsetX, y * scale + offsetY) < waterLevel;
			}
		}
	}
	public void DrawChunk()
	{
		for(int i =0 ; i < chunkCount; i++)
		{
			List<Vector3> vertices = new List<Vector3>();
			List<int> triangles = new List<int>();
			for (int x = 0; x < mapChunkSize; x++)
			{

				for (int y = 0; y < mapChunkSize; y++)
				{
					int chunkX = x + i * mapChunkSize;
					int chunkY = y;
					MapCell cell = Grid.GetGridObject(chunkX, chunkY);
					if (!cell.IsWater)
					{
						Vector3 a = new Vector3(chunkX, chunkY + cellSize);

						Vector3 b = new Vector3(chunkX + cellSize, chunkY + cellSize);
						Vector3 c = new Vector3(chunkX + cellSize, chunkY);
						Vector3 d = new Vector3(chunkX, chunkY);
						Vector3[] v = new Vector3[] { a, c, d, a, b, c };

						for (int k = 0; k < 6; k++)
						{
							vertices.Add(v[k]);
							triangles.Add(triangles.Count);
						}


					}
				}
			}
			MeshData meshData = new MeshData();
			meshData.AddTriangles(vertices, triangles);
			Chunk chunk = new Chunk(new Vector3(mapChunkSize * i,0,0),meshData);
			chunks.Add(chunk);
		}
	}

	public Mesh DrawTerrainMesh()
	{
		//Mesh mesh = new Mesh();
		List<Vector3> vertices = new List<Vector3>();
		List<int> triangles = new List<int>();	
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

					for (int k =0; k < 6; k++)
					{
						vertices.Add(v[k]);
						triangles.Add(triangles.Count);
					}


				}
			}
		}
		MeshData meshData = new MeshData();
		meshData.AddTriangles(vertices, triangles);
		return meshData.CreateMesh();
		//mesh.vertices = vertices.ToArray();
		//mesh.triangles = triangles.ToArray();
		//mesh.RecalculateNormals();

		//MeshFilter meshFilter = gameObject.AddComponent<MeshFilter>();
		//meshFilter.mesh = mesh;

		//MeshRenderer meshRenderer = gameObject.AddComponent<MeshRenderer>();
		



	}


}


public class MeshData{
	List<Vector3> vertices;
	List<int> triangles;

	public MeshData()
	{


	}

	public void AddTriangles(List<Vector3> vertices, List<int> triangles)
	{
		this.vertices = vertices;
		this.triangles = triangles;

	}

	public Mesh CreateMesh()
	{
		Mesh mesh = new Mesh();
		mesh.vertices = this.vertices.ToArray();
		mesh.triangles = this.triangles.ToArray();
		Debug.Log(mesh.triangles.Length);
		Debug.Log(this.triangles.ToArray().Length);
		
		mesh.RecalculateNormals();
		return mesh;
	}
}

public class Chunk
{
	GameObject gameObject;
	Vector3 position;

	public Chunk(Vector3 position, MeshData meshData)
	{
		this.position = position;
		gameObject = new GameObject("Chunk");
		gameObject.transform.position = this.position;
		MeshRenderer meshRenderer = gameObject.AddComponent<MeshRenderer>();
		MeshFilter meshFilter = gameObject.AddComponent<MeshFilter>();
		meshFilter.mesh = meshData.CreateMesh();

	}
}


public class MapCell
{
	private Grid<MapCell> grid;
	public PathNode PathNode { get; set; }
	int x;
	int y;
	private bool isWater;
	public bool IsWater {
		get { return isWater; } 
		set{ isWater = value;
			PathNode.IsWalkable = !value;
		} }
	public MapCell(Grid<MapCell> grid, int x, int y)
	{
		this.grid = grid;
		this.x = x;
		this.y = y;
		PathNode = new PathNode(x, y,IsWater);

	}
}

