using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingEnemy : Enemy
{
	[SerializeField] float speed = 1;
	[SerializeField] private float speedMax = 2;

	private void FixedUpdate()
	{
		if (!isBubble)
		{
			Vector2 dir = player.transform.position - transform.position;
			myRigidbody2D.velocity += dir*speed;
			if (myRigidbody2D.velocity.magnitude > speedMax)
			{
				myRigidbody2D.velocity = myRigidbody2D.velocity.normalized * speedMax;
			}
		}
		else
		{
			BubbleMove();
		}
	}
}