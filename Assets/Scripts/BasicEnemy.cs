using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BasicEnemy : MonoBehaviour
{
	[SerializeField] private GameObject player;
	[SerializeField] private float speed = 3;
	[SerializeField] private float jumpSpeed = 5;
	[SerializeField] private GameObject jumpPosition;
	[SerializeField] private GameObject jumpCheckPlatForm;
	[SerializeField] private GameObject groundDetector;
	[SerializeField] private GameObject frontDetector;
	[SerializeField] private bool enableJump = false;
	[SerializeField] private GameObject bubble;

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
	private Vector2 previousPos;
	private Collider2D groundDetectorCollider2D;
	private Collider2D frontDetectorCollider2D;
	private Collider2D jumpPositionCollider2D;
	private Collider2D jumpCheckPlatFormCollider2D;
	private TilemapCollider2D tilemapCollider2D;
	private Collider2D myCollider;

	#region Inherited methods

	private void Start()
	{
		myRigidbody2D = GetComponent<Rigidbody2D>();
		float myRotationY = this.transform.rotation.eulerAngles.y;
		isLookingRight = Mathf.Abs(myRotationY) < 90.0f;
		//TODO add checkstate somewhere here

		groundDetectorCollider2D = groundDetector.GetComponent<Collider2D>();
		frontDetectorCollider2D = frontDetector.GetComponent<Collider2D>();
		jumpPositionCollider2D = jumpPosition.GetComponent<Collider2D>();
		jumpCheckPlatFormCollider2D = jumpCheckPlatForm.GetComponent<Collider2D>();
		myCollider = GetComponent<Collider2D>();

		bubble.SetActive(false);
		player = GameManager.Instance.player;
		tilemapCollider2D = GameManager.Instance.levelTilemap.GetComponent<TilemapCollider2D>();
	}

	private void Update()
	{
		switch (myState)
		{
			case BasicEnemyStates.Jumping:
			{
				CheckFalling();
			}
				break;
			case BasicEnemyStates.Falling:
			{
				myRigidbody2D.velocity = myRigidbody2D.velocity * Vector2.up;
				CheckPlayerPosX();
				CheckGround();
			}
				break;
			case BasicEnemyStates.Running:
			{
				MoveForward();
				CheckFront();
				CheckGround();
				CheckPlayerPosY();
				CheckForJump();
			}
				break;
			case BasicEnemyStates.InBubble:
			{
				if (!bubble.activeSelf)
				{
					bubble.SetActive(true);
					// Avoids the bubble collider from being triggered by enemy's own colliders.
					myCollider.enabled = false;
					frontDetectorCollider2D.enabled = false;
					groundDetectorCollider2D.enabled = false;

					myRigidbody2D.gravityScale = 0;
				}

				BubbledMove();
			}
				break;
		}

		previousPos = transform.position;
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

	public void Bubble()
	{
		myState = BasicEnemyStates.InBubble;
	}

	#endregion

	#region Private methods

	private void CheckFalling()
	{
		if (previousPos.y > transform.position.y)
		{
			myState = BasicEnemyStates.Falling;
		}
	}

	private void CheckGround()
	{
		if (groundDetectorCollider2D.IsTouching(tilemapCollider2D))
		{
			myState = BasicEnemyStates.Running;
		}
		else
		{
			myState = BasicEnemyStates.Falling;
		}
	}

	private void CheckFront()
	{
		if (frontDetectorCollider2D.IsTouching(tilemapCollider2D))
		{
			Debug.Log("something in front, turn !");
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
		if (diffPosY < 0)
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
		//Debug.Log(wantsToJump + " " + !jumpPositionCollider2D.IsTouching(tilemapCollider2D) + " " +jumpCheckPlatFormCollider2D.IsTouching(tilemapCollider2D));
		if (enableJump && wantsToJump && !jumpPositionCollider2D.IsTouching(tilemapCollider2D) &&
		    jumpCheckPlatFormCollider2D.IsTouching(tilemapCollider2D))
		{
			myRigidbody2D.velocity = Vector2.up * jumpSpeed;
			myState = BasicEnemyStates.Jumping;
		}
	}

	private void MoveForward()
	{
		myRigidbody2D.velocity = (Vector2) transform.right * speed + Vector2.up * myRigidbody2D.velocity.y;
	}

	private void TurnAround()
	{
		isLookingRight = !isLookingRight;
		this.transform.Rotate(Vector3.up * 180);
	}

	private void BubbledMove()
	{
		// Resetting velocity.
		myRigidbody2D.velocity = new Vector2();

		int randomNumber = Random.Range(0, 100);

		if (randomNumber > 50)
		{
			// Moves the bubbled enemy upwards.
			transform.position = new Vector2(transform.position.x, transform.position.y + 0.1f);
		}
		else if (randomNumber > 0 && randomNumber < 25)
		{
			// Moves the bubbled enemy to the right.
			transform.position = new Vector2(transform.position.x + 0.1f, transform.position.y);
		}
		else
		{
			// Moves the bubbled enemy to the left.
			transform.position = new Vector2(transform.position.x - 0.1f, transform.position.y);
		}
	}

	#endregion
}