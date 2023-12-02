using System.Collections.Generic;
using Unity.Jobs;
using UnityEngine;
using Unity.Mathematics;
using Unity.Collections;
using System;



public class CharacterMovementPathfinding : MonoBehaviour
{
	public float speed = 5;
	private int currentPathIndex;
	private List<Vector3> pathVectorList;
	public Rigidbody2D rb;
	public bool automate = true;

	private void Start()
	{
		Transform bodyTransform = transform;


	}
	private void Update()
	{
		HandleMovement();
		if (!automate)
		{
			if (Input.GetMouseButtonDown(0))
			{
				SetTargetPosition(GetWorldPosition.GetMouseWorldPosition());
			}
		}
		if(automate && pathVectorList == null)
		{
			RunRandomly();
		}


	}
	// Debug.Log($"");
	private void HandleMovement()
	{
		//Debug.Log("Handle movement");
		if(pathVectorList != null)
		{
			
			Vector3 targetPosition = pathVectorList[currentPathIndex];
			//Debug.Log($"TargetPosition: {targetPosition}");
			//Debug.Log($"{Vector3.Distance(transform.position, targetPosition)}\n" +
			//	$" Index{currentPathIndex}\n" +
			//	$"Character: {transform.position}\n" +
			//	$"Destination: {targetPosition}");
			if (Vector3.Distance(transform.position, targetPosition) > 0.01f) 
			{

				Vector3 moveDir = (targetPosition - transform.position).normalized;
				//Debug.Log($"MoveDir: {moveDir}");
				float distanceBefore = Vector3.Distance(transform.position, targetPosition);
				//transform.position = transform.position + moveDir * speed * Time.deltaTime;
				//rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
				//rb.MovePosition(transform.position + moveDir * speed * Time.fixedDeltaTime);
				rb.MovePosition(Vector3.MoveTowards(transform.position,targetPosition, speed * Time.fixedDeltaTime));


			}
			else
			{
				currentPathIndex++;

					if(currentPathIndex >= pathVectorList.Count) 
				{
					Stop();

				}
			}
		}
	}

	private void Stop()
	{
		pathVectorList = null;
	}

	public Vector3 GetPosition()
	{
		return transform.position;
	}

	public void SetTargetPosition(Vector3 targetPosition)
	{
		currentPathIndex = 0;
		pathVectorList = null;
		//pathVectorList = Pathfinding.Instance.FindPath(GetPosition(), targetPosition);
		Debug.Log("Requesting path");
		PathManager.RequestPath(new PathRequest(GetPosition(), targetPosition, SetVectorList, automate));

		//if (pathVectorList != null && pathVectorList.Count > 1)
		//{
		//	pathVectorList.RemoveAt(0);
		//}
	}

	private void SetVectorList(List<Vector3> _pathVectorList, bool succes)
	{
		if (succes && _pathVectorList.Count > 1)
		{
			pathVectorList = _pathVectorList;
			//foreach(var path in pathVectorList)
			//{
			//	Debug.Log(path);
			//}
			pathVectorList.RemoveAt(0);
		}
		else
		{
			if (automate)
			{
				RunRandomly();
			}
		}
	}

	public void RunRandomly()
	{
		currentPathIndex = 0;
		pathVectorList = null;
		//pathVectorList = Pathfinding.Instance.FindRandomPath(GetPosition());
		PathManager.RequestPath(new PathRequest(GetPosition(), new Vector3(0, 0, 0), SetVectorList, automate));
	}




}
