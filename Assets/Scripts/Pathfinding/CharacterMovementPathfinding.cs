using System.Collections.Generic;
using Unity.Jobs;
using UnityEngine;
using Unity.Mathematics;
using Unity.Collections;



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
		if(pathVectorList != null)
		{
			
			Vector3 targetPosition = pathVectorList[currentPathIndex];
			if(Vector3.Distance(transform.position, targetPosition) > 1f) 
			{

				Vector3 moveDir = (targetPosition - transform.position).normalized;
				float distanceBefore = Vector3.Distance(transform.position, targetPosition);
				//transform.position = transform.position + moveDir * speed * Time.deltaTime;
				//rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
				rb.MovePosition(transform.position + moveDir * speed * Time.fixedDeltaTime);

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
		pathVectorList = Pathfinding.Instance.FindPath(GetPosition(), targetPosition);

		if (pathVectorList != null && pathVectorList.Count > 1)
		{
			pathVectorList.RemoveAt(0);
		}
	}

	public void RunRandomly()
	{
		currentPathIndex = 0;
		Debug.Assert(Pathfinding.Instance != null);
		pathVectorList = Pathfinding.Instance.FindRandomPath(GetPosition());
		if (pathVectorList != null && pathVectorList.Count > 1)
		{
			pathVectorList.RemoveAt(0);
		}
	}




}
