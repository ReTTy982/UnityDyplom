using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour
{

	public float waterLevel = .4f;
	public float cellSize;
	public int mapChunkSize;
	public float scale;
	public int mapChunkCount;

	private MapGrid mapGrid;
	private Pathfinding pathfinding;
	// Start is called before the first frame update
	void Start()
	{
		
		//MeshRenderer meshRenderer = gameObject.AddComponent<MeshRenderer>();
		mapGrid = new MapGrid(mapChunkCount, cellSize, scale, waterLevel);
		mapGrid.DrawChunk();

		//Mesh mesh = mapGrid.DrawTerrainMesh();

		
		//Debug.Log(mesh.triangles.Length);
		//Debug.Log(mesh.vertices.Length);
		//MeshFilter meshFilter = gameObject.AddComponent<MeshFilter>();
		//meshFilter.mesh = mesh;
		//MeshData meshData = new MeshData();
		//meshData.AddTriangles(mesh.vertices.ToList<Vector3>(), mesh.triangles.ToList<int>());
		//Chunk chunk = new Chunk(new Vector3(mapChunkSize, 0, 0), meshData);


		pathfinding = new Pathfinding(mapChunkSize, mapChunkSize, cellSize, mapGrid);

	}



	//}


	//public void Start()
	//{
	//	MeshRenderer meshRenderer = gameObject.AddComponent<MeshRenderer>();
	//	meshRenderer.sharedMaterial = new Material(Shader.Find("Standard"));

	//	MeshFilter meshFilter = gameObject.AddComponent<MeshFilter>();

	//	Mesh mesh = new Mesh();

	//	Vector3[] vertices = new Vector3[4]
	//	{
	//		new Vector3(0, 0, 0),
	//		new Vector3(width, 0, 0),
	//		new Vector3(0, height, 0),
	//		new Vector3(width, height, 0)
	//	};
	//	mesh.vertices = vertices;

	//	int[] tris = new int[6]
	//	{
	//           // lower left triangle
	//           0, 2, 1,
	//           // upper right triangle
	//           2, 3, 1
	//	};
	//	mesh.triangles = tris;


	//	meshFilter.mesh = mesh;
	//}



	// Update is called once per frame
	void Update()
	{

	}
}

