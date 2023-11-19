using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Unity.VisualScripting;

public class PathManager 
{
    Queue<PathRequest> pathRequestQueue = new Queue<PathRequest>();
    PathRequest currentPathRequest;

    public static PathManager instance;
    Pathfinding pathfinding;
    bool isProcessingPath;

    public PathManager(Pathfinding _pathfinding)
    {
        pathfinding = _pathfinding;
        instance = this;
    }

	public static void RequestPath(Vector3 pathStart, Vector3 pathEnd, Action<List<Vector3>,bool> callback,bool automate)
    {
        PathRequest newRquest = new PathRequest(pathStart,pathEnd,callback,automate);
        instance.pathRequestQueue.Enqueue(newRquest);
        instance.TryProcessNext();
    }

    void TryProcessNext()
    {
        if (!isProcessingPath && pathRequestQueue.Count > 0)
        {
            currentPathRequest = pathRequestQueue.Dequeue();
            isProcessingPath = true;

            pathfinding.StartFindPath(currentPathRequest.pathStart,currentPathRequest.pathEnd,currentPathRequest.automate);
        }
    }

    public void FinishedProcessingPath(List<Vector3> path, bool succes)
    {
        Debug.Log("Finished");
        currentPathRequest.callback(path, succes);
        isProcessingPath = false;
        TryProcessNext();
    }

    struct PathRequest 
    {
        public Vector3 pathStart;
        public Vector3 pathEnd;
        public Action<List<Vector3>, bool> callback;
        public bool automate;

        public PathRequest(Vector3 _start, Vector3 _end, Action<List<Vector3>,bool> _callback,bool _automate)
        {
            pathStart = _start;
            pathEnd = _end;
            callback = _callback;
            automate = _automate;
        }
    }

}
