using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapTest : MonoBehaviour
{
	public int height;
	public int width;
	public float scale = .1f;
	public float cellSize;
	private MapGrid mapGrid;

	void Start()
    {
		float[,] noiseMap= new float[height,width];
		for(int x=0; x<height; x++)
		{
			for(int y=0; y<width; y++)
			{
				Debug.Log("wewnetrzne");
				float noiseValue = Mathf.PerlinNoise(x*scale,y*scale);
				noiseMap[x,y] = noiseValue;
			}
		}

		mapGrid = new MapGrid(width,height,cellSize,noiseMap);
    }
	private void OnDrawGizmos()
	{
		
		if (!Application.isPlaying) return;
		for (int x = 0 ; x < width; x++)
		{
			for (int y = 0 ; y < height; y++)
			{
				MapCell cell = mapGrid.Grid.GetGridObject(x, y);
				if (cell.IsWater == true)
				{
					Gizmos.color = Color.blue;

				}
				else if (cell.IsWater == false)
				{
					Gizmos.color = Color.green;
				}
				Gizmos.DrawCube(mapGrid.Grid.GetWorldPosition(x,y),new Vector3(cellSize,cellSize,0));
			}
		}
	}


}
