using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid_Testing : MonoBehaviour
{
	public int height;
	public int width;
	public float cellSize;

	private Pathfinding pathfinding;

	private void Start()
	{
		pathfinding = new Pathfinding(width, height, cellSize);

		
	}

	private void Update()
	{
		if (Input.GetMouseButtonDown(0))
		{
			Vector3 mouseWorldPostion = GetMouseWorldPosition();
			pathfinding.Grid.GetXY(mouseWorldPostion, out int x, out int y);
			List<PathNode> path = pathfinding.FindPath(0, 0, x, y);
			List<PathNode> path1 = pathfinding.FindPath(99, 99, x, y);
			if(path != null)
			{
				for (int i=0; i<path.Count-1; i++)
				{


					Vector3 start = pathfinding.Grid.GetWorldPosition(path[i].x, path[i].y);
					Vector3 start1 = pathfinding.Grid.GetWorldPosition(path1[i].x, path1[i].y);
					Vector3 end = pathfinding.Grid.GetWorldPosition(path[i+1].x, path[i+1].y);
					Vector3 end1 = pathfinding.Grid.GetWorldPosition(path1[i + 1].x, path1[i + 1].y);


					Debug.DrawLine(start,end,Color.yellow,5f);
					Debug.DrawLine(start1, end1, Color.cyan, 5f);
				}
			}

		}
	}


	public static Vector3 GetMouseWorldPosition()
	{
		Vector3 vec = GetMouseWorldPositionWithZ(Input.mousePosition, Camera.main);
		vec.z = 0f;
		return vec;
	}

	public static Vector3 GetMouseWorldPositionWithZ()
	{
		return GetMouseWorldPositionWithZ(Input.mousePosition, Camera.main);
	}

	public static Vector3 GetMouseWorldPositionWithZ(Camera worldCamera)
	{
		return GetMouseWorldPositionWithZ(Input.mousePosition, worldCamera);
	}

	public static Vector3 GetMouseWorldPositionWithZ(Vector3 screenPosition, Camera worldCamera)
	{
		Vector3 worldPosition = worldCamera.ScreenToWorldPoint(screenPosition);
		return worldPosition;
	}




}
