using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

	public float waterLevel = .4f;
	public float cellSize;
	public int width, height;
	public float scale;

	private MapGrid mapGrid;
	private Pathfinding pathfinding;
	// Start is called before the first frame update
	void Start()
	{
		Pathfinding pathfinding = new Pathfinding(width, height,cellSize);
		MeshRenderer meshRenderer = gameObject.AddComponent<MeshRenderer>();
		mapGrid = new MapGrid(width, height, cellSize, scale, waterLevel);
		Mesh mesh = new Mesh();
		List<Vector3> vertices = new List<Vector3>();
		List<int> triangles = new List<int>();
		mapGrid.DrawTerrainMesh(out vertices, out triangles);
		mesh.vertices = vertices.ToArray();
		mesh.triangles = triangles.ToArray();
		mesh.RecalculateNormals();

		MeshFilter meshFilter = gameObject.AddComponent<MeshFilter>();
		meshFilter.mesh = mesh;
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

