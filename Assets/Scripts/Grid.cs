using System;
using UnityEngine;
public class Grid<TGridObject>
{
    public bool debug = true;
    public int Width { get; private set; }
    public int Height{ get; private set; }
    public float CellSize { get; private set; }
    private TGridObject[,] gridArray;
    private Vector3 centerPosition;
    public Grid(int width, int height, float cellSize, Vector3 centerPosition, Func<Grid<TGridObject>, int, int, TGridObject> createGridObject) {
        Width = width;
        Height = height;
        this.CellSize = cellSize;
        gridArray = new TGridObject[width, height];
        this.centerPosition = centerPosition;

		for (int x = 0; x < gridArray.GetLength(0); x++)
		{
			for (int y = 0; y < gridArray.GetLength(1); y++)
			{
				gridArray[x, y] = createGridObject(this,x, y);
			}
		}
        if (debug)
		{
			for (int x = 0; x < gridArray.GetLength(0); x++)
			{
				for (int y = 0; y < gridArray.GetLength(1); y++)
				{
					Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x + 1, y), Color.red,1000f);
					Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x, y + 1), Color.red, 1000f);

				}

			}
			Debug.DrawLine(GetWorldPosition(0, height), GetWorldPosition(width, height), Color.red, 1000f);
			Debug.DrawLine(GetWorldPosition(width, 0), GetWorldPosition(width, height), Color.red, 1000f);
		}
		


	}



    public TGridObject GetGridObject(int x, int y)
    {
		if (x >= 0 && y >= 0 && x < Width && y< Height){
            return gridArray[x,y];
        }
        else
        {
            return default(TGridObject);
        }
    }

    public Vector3 GetWorldPosition(int x, int y)
    {
        return new Vector3(x, y) * CellSize + centerPosition;
    }

    public void GetXY(Vector3 worldPosition,out int x, out int y)
    {
        x = Mathf.FloorToInt((worldPosition - centerPosition).x / CellSize);
		y = Mathf.FloorToInt((worldPosition - centerPosition).y / CellSize);
	}
}
