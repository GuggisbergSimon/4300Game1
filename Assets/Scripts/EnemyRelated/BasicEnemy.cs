using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BasicEnemy : Enemy
{
	#region Variables

	[SerializeField] private float speed = 3;
	[SerializeField] private float checkPlayerHeightThreshold = 0.5f;
	[SerializeField] private float jumpSpeed = 5;
	[SerializeField] private GameObject jumpPosition;
	[SerializeField] private GameObject jumpCheckPlatForm;
	[SerializeField] private GameObject groundDetector;
	[SerializeField] private GameObject frontDetector;
	[SerializeField] private bool enableJump = true;

	private enum States
	{
		Falling,
		Jumping,
		Running,
		InBubble
	}

	private States myState;
	private bool isLookingRight;
	private bool wantsToJump = false;
	private Vector2 previousPos;
	private TriggerDetector frontTrigger;
	private Collider2D jumpPositionCollider2D;
	private Collider2D jumpCheckPlatFormCollider2D;
	private Collider2D groundDetectorCollider2D;
	private CompositeCollider2D tilemapCollider2D;

	#endregion

	#region Inherited methods

	private new void Start()
	{
		base.Start();
		jumpPositionCollider2D = jumpPosition.GetComponent<Collider2D>();
		jumpCheckPlatFormCollider2D = jumpCheckPlatForm.GetComponent<Collider2D>();
		groundDetectorCollider2D = groundDetector.GetComponent<Collider2D>();
		frontTrigger = frontDetector.GetComponent<TriggerDetector>();
		tilemapCollider2D = GameManager.Instance.levelTilemap.GetComponent<CompositeCollider2D>();
		float myRotationY = this.transform.rotation.eulerAngles.y;
		isLookingRight = Mathf.Abs(myRotationY) < 90.0f;
		//TODO add checkstate somewhere here
	}

	private void FixedUpdate()
	{
		if (isBubble)
		{
			myState = States.InBubble;
		}

		switch (myState)
		{
			case States.Falling:
			{
				myRigidbody2D.velocity = myRigidbody2D.velocity * Vector2.up;
				CheckGround();
				CheckPlayerPosX();
			}
				break;
			case States.Jumping:
			{
				CheckFalling();
			}
				break;
			case States.Running:
			{
				MoveForward();
				CheckGround();
				if (myState == States.Running)
				{
					CheckFront();
					if (enableJump)
					{
						CheckPlayerPosY();
						CheckForJump();
					}
				}
			}
				break;
			case States.InBubble:
			{
				BubbleMove();
			}
				break;
		}

		previousPos = transform.position;
	}

	#endregion

	#region Public methods

	public void Bubble()
	{
		myState = States.InBubble;
	}

	#endregion

	#region Private methods

	private void CheckFalling()
	{
		if (previousPos.y > transform.position.y)
		{
			myState = States.Falling;
		}
	}

	private void CheckGround()
	{
		if (groundDetectorCollider2D.IsTouching(tilemapCollider2D))
		{
			myState = States.Running;
		}
		else
		{
			myState = States.Falling;
		}
	}

	private void CheckFront()
	{
		if (frontTrigger.TriggerEntered)
		{
			TurnAround();
		}
	}



	private void CheckPlayerPosX()
	{
		float diffPosX = this.transform.position.x - player.transform.position.x;
		if ((diffPosX < 0 && !isLookingRight) || (diffPosX > 0 && isLookingRight))
		{
			TurnAround();
		}
	}

	private void CheckPlayerPosY()
	{
		float diffPosY = this.transform.position.y - player.transform.position.y;
		if (diffPosY < -checkPlayerHeightThreshold)
		{
			wantsToJump = true;
		}
		else
		{
			wantsToJump = false;
		}
	}

	//Checks if jump is possible depending on various conditions
	private void CheckForJump()
	{
		if (wantsToJump && !jumpPositionCollider2D.IsTouching(tilemapCollider2D) &&
		    jumpCheckPlatFormCollider2D.IsTouching(tilemapCollider2D))
		{
			myRigidbody2D.velocity = Vector2.up * jumpSpeed;
			myState = States.Jumping;
		}
	}

	private void MoveForward()
	{
		myRigidbody2D.velocity = (Vector2) transform.right * speed + Vector2.up * myRigidbody2D.velocity;
	}

	private void TurnAround()
	{
		isLookingRight = !isLookingRight;
		this.transform.Rotate(Vector3.up * 180);
	}

	#endregion
}