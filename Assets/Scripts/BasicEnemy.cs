using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicEnemy : MonoBehaviour
{
	[SerializeField] private GameObject player;
	[SerializeField] private bool isLookingRight=true; //put that in Start and I'll be happy
	[SerializeField] private float speed = 4.0f;
	[SerializeField] private float distMinX = 1.5f;

	private bool noTurn;
	private Animator myAnimator; // not used currently
	private Rigidbody2D myRigidbody2D;

	private void Start()
	{
		myRigidbody2D = GetComponent<Rigidbody2D>();
	}

	private void Update()
	{
		Rotate();
		Move();
	}

	//Moves the enemy
	private void Move()
	{
		myRigidbody2D.velocity = (Vector2)transform.right * speed + Vector2.up * myRigidbody2D.velocity.y;
	}

	//Rotates the enemy in direction of the player (and if the player is far enough)
	private void Rotate()
	{
		float diffPosX = player.transform.position.x - this.transform.position.x;

		if (Mathf.Abs(diffPosX) > distMinX)
		{
			if (diffPosX > 0 && !isLookingRight)
			{
				TurnAround();
			}
			else if (diffPosX < 0 && isLookingRight)
			{
				TurnAround();
			}
		}
	}


	private void TurnAround()
	{
		isLookingRight = !isLookingRight;
		this.transform.rotation = Quaternion.Euler(0, this.transform.rotation.eulerAngles.y + 180, 0);
	}
}