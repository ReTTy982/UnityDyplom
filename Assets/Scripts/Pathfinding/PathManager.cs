using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Unity.VisualScripting;
using System.Threading;
using JetBrains.Annotations;

public class PathManager 
{
    Queue<PathResult> results = new Queue<PathResult>();
    PathRequest currentPathRequest;

    public static PathManager instance;
    Pathfinding pathfinding;
    bool isProcessingPath;

    public PathManager(Pathfinding _pathfinding)
    {
        pathfinding = _pathfinding;
        instance = this;
    }

    public void Update()
    {
        if (results.Count > 0)
        {
            int itemsInQueue = results.Count;
            lock(results) {
                for(int i =0; i < itemsInQueue; i++)
                {
                    PathResult result = results.Dequeue();
                    result.callback(result.path, result.succes);
                }
            }

        }
    }

	public static void RequestPath(PathRequest request)
    {
        ThreadStart threadStart = delegate
        {
            instance.pathfinding.StartFindPath(request, instance.FinishedProcessingPath);
        };
        threadStart.Invoke();
    }


    public void FinishedProcessingPath(PathResult result)
    {
        lock (results)
        {
			results.Enqueue(result);
		}
        
    }

    




}
public struct PathRequest
{
	public Vector3 pathStart;
	public Vector3 pathEnd;
	public Action<List<Vector3>, bool> callback;
	public bool automate;

	public PathRequest(Vector3 _start, Vector3 _end, Action<List<Vector3>, bool> _callback, bool _automate)
	{
		pathStart = _start;
		pathEnd = _end;
		callback = _callback;
		automate = _automate;
	}
}

public struct PathResult
{
	public List<Vector3> path;
	public bool succes;
	public Action<List<Vector3>, bool> callback;

	public PathResult(List<Vector3> _path, bool _succes, Action<List<Vector3>, bool> _callback)
	{
		path = _path;
		succes = _succes;
		callback = _callback;
	}
}
