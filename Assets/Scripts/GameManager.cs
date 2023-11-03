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
        mapGrid = new MapGrid(width, height, cellSize, scale, waterLevel);
		//pathfinding = new Pathfinding(width, height, cellSize);
        mapGrid.DrawTerrainMesh();
	}

    // Update is called once per frame
    void Update()
    {
        
    }
}
