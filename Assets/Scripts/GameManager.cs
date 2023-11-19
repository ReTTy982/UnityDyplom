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
	public TerrainType[] regions;
	public Material material;

	private MapGrid mapGrid;
	private Pathfinding pathfinding;
	private GameGrid gameGrid;
	private PathManager pathManager;
	// Start is called before the first frame update
	void Start()
	{
		

		mapGrid = new MapGrid(mapChunkSize,mapChunkCount, cellSize, scale, waterLevel,material,regions);
		mapGrid.DrawChunk();


		gameGrid = new GameGrid(mapChunkSize, mapChunkCount, mapGrid.Grid);
		pathfinding = new Pathfinding(mapChunkSize, mapChunkSize, cellSize, gameGrid);
		pathManager = new PathManager(pathfinding);
		pathfinding.setManager(pathManager);
		
		

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
		pathManager.Update();
	}
}

