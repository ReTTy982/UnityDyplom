using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovemnt : MonoBehaviour
{
    public float moveSpeed = 5f;
    public Rigidbody2D rb;

	private Inventory inventory;

    Vector2 movement;

	public void Start()
	{
		inventory = new Inventory();
	}

	// Update is called once per frame
	void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
        movement = movement.normalized;


    }

	private void FixedUpdate()
	{


		rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
	}
}
