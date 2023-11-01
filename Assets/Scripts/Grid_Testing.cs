using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid_Testing : MonoBehaviour
{
	public int height;
	public int width;
	public float cellSize;
	private void Start()
	{
		Global_Grid grid = new Global_Grid(height, width, cellSize);
	}


}
