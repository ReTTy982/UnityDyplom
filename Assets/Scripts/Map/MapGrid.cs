using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.Mesh;

public class MapGrid
{
	Material material;
	public TerrainType[] regions;
	private List<Chunk> chunks = new List<Chunk>();
	public Material defaultMaterial;
	public float waterLevel = .4f;
	public float cellSize;
	public float scale;
	public int chunkCount;
	public int mapChunkSize;
	public int chunksPerLine = 4;
	public int width;
	public int height;

	int octaves = 8;
	float persistance = 0.5f;
	float lacunarity = 2;

	public Grid<MapCell> Grid { private set; get; }	

	public MapGrid(int mapChunkSize,int chunkCount, float cellSize, float scale, float waterLevel,Material material,TerrainType[] regions) {
		Grid = new Grid<MapCell>(chunksPerLine* mapChunkSize, (chunkCount/chunksPerLine) * mapChunkSize, cellSize, Vector3.zero, (Grid<MapCell> g, int x, int y) => new MapCell(g, x, y));
		this.material = material;
		this.chunkCount = chunkCount;
		this.scale = scale;
		this.waterLevel = waterLevel;
		this.cellSize = cellSize;
		this.mapChunkSize = mapChunkSize;
		this.width = chunksPerLine * mapChunkSize;
		this.height = (chunkCount / chunksPerLine) * mapChunkSize;
		this.regions = regions;
		SetNoise(octaves,persistance,lacunarity);
	}
	public void Start()
	{
		Grid = new Grid<MapCell>(chunksPerLine * chunkCount, chunkCount / chunksPerLine, cellSize, Vector3.zero, (Grid<MapCell> g, int x, int y) => new MapCell(g, x, y));
		SetNoise(octaves,persistance,lacunarity);
		//DrawTerrainMesh();
	}



	private void SetNoise(int octaves, float persistance, float lacunarity)
	{


		float[,] noiseMap = new float[Grid.Width, Grid.Height];
		float offsetX = UnityEngine.Random.Range(-10000f, 10000f);
		float offsetY = UnityEngine.Random.Range(-10000f, 10000f);
		float maxNoiseHeight = float.MinValue;
		float minNoiseHeight = float.MaxValue;


		for (int x = 0; x < Grid.Width; x++)
		{
			for (int y = 0; y < Grid.Height; y++)
			{
				float amplitude = 1;
				float frequency = 1;
				float noiseHeight = 0;
				for (int i = 0; i < octaves; i++)
				{
					float value = Mathf.PerlinNoise(x * scale * frequency + offsetX, y * scale * frequency + offsetY) * 2 - 1;
					noiseHeight += value * amplitude;
					amplitude *= persistance;
					frequency *= lacunarity;

				}
				if (noiseHeight > maxNoiseHeight)
				{
					maxNoiseHeight = noiseHeight;
				}
				else if (noiseHeight < minNoiseHeight)
				{
					minNoiseHeight = noiseHeight;
				}
				noiseMap[x, y] = noiseHeight;





			}
		}

		for (int x = 0; x < Grid.Width; x++)
		{
			for (int y = 0; y < Grid.Height; y++)
			{
				noiseMap[x,y] = Mathf.InverseLerp(minNoiseHeight, maxNoiseHeight, noiseMap[x,y]);
				Grid.GetGridObject(x, y).IsWater = noiseMap[x, y] < waterLevel;
				for (int region = 0; region < regions.Length; region++)
				{
					if (regions[region].height >= noiseMap[x, y])
					{
						Grid.GetGridObject(x, y).setTerrain(regions[region]);
						break;
					}
				}
			}
		}
	}


	public void DrawChunk()
	{
		int row = 0;
		int column = 0;
		for(int i =0 ; i < chunkCount; i++)
		{

			
			if(i%chunksPerLine == 0 && i!=0)
			{
				row++;
				column = 0;
			}
			List<Vector3> vertices = new List<Vector3>();
			List<int> triangles = new List<int>();
			List<Vector2> uvs = new List<Vector2>();
			for (int x = 0; x < mapChunkSize; x++)
			{

				for (int y = 0; y < mapChunkSize; y++)
				{
					int chunkX = x + column * mapChunkSize ;
					int chunkY = y + row * mapChunkSize;

					MapCell cell = Grid.GetGridObject(chunkX, chunkY);


					//Vector3 a = new Vector3(chunkX, chunkY + cellSize); // top left
					//Vector3 b = new Vector3(chunkX + cellSize, chunkY + cellSize); // top right
					//Vector3 c = new Vector3(chunkX + cellSize, chunkY); // bottom right
					//Vector3 d = new Vector3(chunkX, chunkY); // bottom left
					//Vector3[] v = new Vector3[] { a, c, d, a, b, c };
					chunkX *= (int)cellSize;
					chunkY *= (int)cellSize;

					Vector3 a = new Vector3(chunkX, chunkY,0); // bottom left
					Vector3 b = new Vector3(chunkX + cellSize, chunkY,0); // bottom right
					Vector3 c = new Vector3(chunkX, chunkY + cellSize,0); // top left
					Vector3 d = new Vector3(chunkX + cellSize, chunkY + cellSize, 0); // top right
					
					
					Vector3[] v = new Vector3[] { c, b, a, c, d, b };

					//Vector3 a = new Vector3(chunkX - cellSize, chunkY + cellSize); bottom left
					//Vector3 b = new Vector3(chunkX + cellSize, chunkY + cellSize); bottom right
					//Vector3 c = new Vector3(chunkX - cellSize, chunkY - cellSize); top left
					//Vector3 d = new Vector3(chunkX + cellSize, chunkY - cellSize); top right
					//Vector3[] v = new Vector3[] { a, b, c, b, d, c };

					chunkX = x + column * mapChunkSize;
					chunkY = y + row * mapChunkSize;

					Vector2 uvA = new Vector2(chunkX / (float)mapChunkSize, chunkY / (float)mapChunkSize);
					Vector2 uvB = new Vector2((chunkX + 1) / (float)mapChunkSize, chunkY / (float)mapChunkSize);
					Vector2 uvC = new Vector2(chunkX / (float)mapChunkSize, (chunkY + 1) / (float)mapChunkSize);
					Vector2 uvD = new Vector2((chunkX + 1) / (float)mapChunkSize , (chunkY + 1) / (float)mapChunkSize);


					Vector2[] uv = new Vector2[] {uvC,uvB,uvA,uvC,uvD,uvB };

					for (int k = 0; k < 6; k++)
					{
						vertices.Add(v[k]);
						triangles.Add(triangles.Count);
						uvs.Add(uv[k]);
					}
					
					

					
				}
			}
			
			MeshData meshData = new MeshData();
			meshData.AddTriangles(vertices, triangles,uvs);
			Chunk chunk = new Chunk(new Vector3(mapChunkSize * i,0,0),meshData,row,column);
			Texture2D texture = DrawTexture(chunk);
			chunk.PutTexture(texture);
			chunks.Add(chunk);
			column++;
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
		List<Vector2> uvs = new List<Vector2>(); // DUMMY
		meshData.AddTriangles(vertices, triangles,uvs);
		return meshData.CreateMesh();
		//mesh.vertices = vertices.ToArray();
		//mesh.triangles = triangles.ToArray();
		//mesh.RecalculateNormals();

		//MeshFilter meshFilter = gameObject.AddComponent<MeshFilter>();
		//meshFilter.mesh = mesh;

		//MeshRenderer meshRenderer = gameObject.AddComponent<MeshRenderer>();
		



	}

	public Texture2D DrawTexture(Chunk chunk)
	{
		Texture2D texture = new Texture2D(mapChunkSize, mapChunkSize);
		Color[] colorMap = new Color[mapChunkSize * mapChunkSize];
		int startX = chunk.Column * this.mapChunkSize;
		int startY = chunk.Row * this.mapChunkSize;
		int endX = startX + this.mapChunkSize;
		int endY = startY + this.mapChunkSize;
		for(int x = 0; x < this.mapChunkSize; x++)
		{
			for (int y = 0; y < this.mapChunkSize; y++)
			{
				colorMap[y * mapChunkSize + x] = Grid.GetGridObject(startX + x, startY + y).terrainType.color;
				//if(Grid.GetGridObject(startX + x, startY + y).IsWater)
				//{
				//	colorMap[y * mapChunkSize + x] = Color.blue;
				//}
				//else
				//{
				//	colorMap[y * mapChunkSize + x] = Color.red;
				//}
				

			}
		}
		texture.filterMode = FilterMode.Point;
		texture.SetPixels(colorMap);
		texture.Apply();
		return texture;


	}


}


public class MeshData{
	List<Vector3> vertices;
	List<int> triangles;
	List<Vector2> uvs;

	public MeshData()
	{


	}

	public void AddTriangles(List<Vector3> vertices, List<int> triangles,List<Vector2>uvs)
	{
		this.vertices = vertices;
		this.triangles = triangles;
		this.uvs = uvs;

	}

	public Mesh CreateMesh()
	{
		Mesh mesh = new Mesh();
		mesh.vertices = this.vertices.ToArray();
		mesh.triangles = this.triangles.ToArray();
		mesh.uv = this.uvs.ToArray();
		
		mesh.RecalculateNormals();
		return mesh;
	}


}

public class Chunk
{
	GameObject gameObject;
	MeshRenderer meshRenderer;
	MeshFilter meshFilter;
	Vector3 position;
	MeshData meshData;
	public int Row { get; set; }
	public int Column { get; set; }

	public Chunk(Vector3 position, MeshData meshData,int row, int column)
	{
		Row = row;
		Column = column;
		this.position = position;
		this.meshData = meshData;
		gameObject = new GameObject("Chunk");
		//gameObject.transform.position = this.position;
		

	}

	public void PutTexture(Texture2D texture)
	{
		Material newMaterial = new Material(Shader.Find("Unlit/Texture"));
		newMaterial.SetFloat("_Glossiness", 0.0f);
		newMaterial.SetFloat("_Metallic", 0.0f); 

		this.meshRenderer = gameObject.AddComponent<MeshRenderer>();
		this.meshRenderer.material = newMaterial;
		this.meshRenderer.material.mainTexture = texture;
		this.meshFilter = gameObject.AddComponent<MeshFilter>();
		this.meshFilter.mesh = this.meshData.CreateMesh();

	}


}


public class MapCell
{
	private Grid<MapCell> grid;
	public TerrainType terrainType;
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
	public void setTerrain(TerrainType terrainType)
	{
		this.terrainType = terrainType;
	}

}

[Serializable]
public struct TerrainType
{
	public float height;
	public Color color;
	public string name;

}

