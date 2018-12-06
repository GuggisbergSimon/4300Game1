using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicEnemy : MonoBehaviour
{
	[SerializeField] private GameObject player;
	[SerializeField] private float speed = 3;

	private enum BasicEnemyStates
	{
		Falling,
		Jumping,
		Running,
		InBubble
	}

	private BasicEnemyStates myState;
	private bool isLookingRight;
	private bool isAlive = true;
	private bool wantsToJump = false;
	private Rigidbody2D myRigidbody2D;

	#region Inherited methods

	private void Start()
	{
		myRigidbody2D = GetComponent<Rigidbody2D>();
		float myRotation = this.transform.rotation.eulerAngles.y;
		isLookingRight = -90.0f > myRotation && myRotation > 90.0f;
		//TODO setup state here
	}

	private void Update()
	{
		switch (myState)
		{
			case BasicEnemyStates.Falling:
			{
				CheckPlayerPosX();
			}
				break;
			case BasicEnemyStates.Jumping:
			{
			}
				break;
			case BasicEnemyStates.Running:
			{
				MoveForward();
			}
				break;
			case BasicEnemyStates.InBubble:
			{
			}
				break;
		}
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.CompareTag("Platform"))
		{
			//That means the enemy has touched the ground after falling
			if (myState == BasicEnemyStates.Falling)
			{
				myState = BasicEnemyStates.Running;
			}
			//That means the enemy encountered a wall in front of him
			else if (myState == BasicEnemyStates.Running)
			{
				TurnAround();
			}
		}

		//implement the jump somewhere here or use children
	}

	#endregion


	#region Public methods

	public void Die()
	{
		if (isAlive)
		{
			Destroy(this.gameObject);
		}
	}

	#endregion

	#region Private methods

	private void CheckPlayerPosX()
	{
		float diffPosX = this.transform.position.x - player.transform.position.x;
		if ((diffPosX > 0 && !isLookingRight) || (diffPosX < 0 && isLookingRight))
		{
			TurnAround();
		}
	}

	private void CheckPlayerPosY()
	{
		float diffPosY = this.transform.position.y - player.transform.position.y;
		if (diffPosY > 0)
		{
			wantsToJump = true;
		}
	}

	private void MoveForward()
	{
		myRigidbody2D.velocity = (Vector2) transform.right * speed + Vector2.up * myRigidbody2D.velocity.y;
	}

	private void TurnAround()
	{
		isLookingRight = !isLookingRight;
		this.transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y + 180, 0);
	}

	#endregion
}