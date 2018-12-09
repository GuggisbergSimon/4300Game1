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
	[SerializeField] private GameObject bubble;
	[SerializeField] private float bubbleSpeed = 2;
	[SerializeField] private float frequency = 1;
	[SerializeField] private float amplitude = 1;
	[SerializeField] private Sprite bubbleSprite;
	[SerializeField] private ItemScore itemscorePrefab;
	[SerializeField] private int minItemScoreDropped = 1;
	[SerializeField] private int maxItemScoreDropped = 5;
	[SerializeField] private float minSpeedItemScore = 0.3f;
	[SerializeField] private float maxSpeedItemScore = 6.0f;

	private enum BasicEnemyStates
	{
		Falling,
		Jumping,
		Running,
		InBubble
	}

	private GameObject player;
	private BasicEnemyStates myState;
	private bool isLookingRight;
	private bool wantsToJump = false;
	private Rigidbody2D myRigidbody2D;
	private Vector2 previousPos;
	private Collider2D jumpPositionCollider2D;
	private Collider2D jumpCheckPlatFormCollider2D;
	private Collider2D groundDetectorCollider2D;
	private Collider2D frontDetectorCollider2D;
	private Collider2D bubbleCollider2D;
	private Collider2D playerCollider2D;
	private CompositeCollider2D tilemapCollider2D;
	private Collider2D myCollider;
	private Vector2 startSinPos;
	private float sinTimer = 0.0f;

	#endregion

	#region Inherited methods

	private void Start()
	{
		player = GameManager.Instance.player;
		myRigidbody2D = GetComponent<Rigidbody2D>();
		myCollider = GetComponent<Collider2D>();
		jumpPositionCollider2D = jumpPosition.GetComponent<Collider2D>();
		jumpCheckPlatFormCollider2D = jumpCheckPlatForm.GetComponent<Collider2D>();
		groundDetectorCollider2D = groundDetector.GetComponent<Collider2D>();
		frontDetectorCollider2D = frontDetector.GetComponent<Collider2D>();
		bubbleCollider2D = bubble.GetComponent<Collider2D>();
		playerCollider2D = player.GetComponent<Collider2D>();
		tilemapCollider2D = GameManager.Instance.levelTilemap.GetComponent<CompositeCollider2D>();

		bubble.SetActive(false);
		float myRotationY = this.transform.rotation.eulerAngles.y;
		isLookingRight = Mathf.Abs(myRotationY) < 90.0f;
		//TODO add checkstate somewhere here
	}

	private void FixedUpdate()
	{
		switch (myState)
		{
			case BasicEnemyStates.Falling:
			{
				myRigidbody2D.velocity = myRigidbody2D.velocity * Vector2.up;
				CheckGround();
				CheckPlayerPosX();
			}
				break;
			case BasicEnemyStates.Jumping:
			{
				CheckFalling();
			}
				break;
			case BasicEnemyStates.Running:
			{
				MoveForward();
				CheckGround();
				if (myState == BasicEnemyStates.Running)
				{
					CheckFront();
					CheckPlayerPosY();
					CheckForJump();
				}
			}
				break;
			case BasicEnemyStates.InBubble:
			{
				if (!bubble.activeSelf)
				{
					startSinPos = transform.position;
					isBubble = true;
					GetComponentInChildren<SpriteRenderer>().sprite = bubbleSprite;
					bubble.SetActive(true);
					// Avoids the bubble collider from being triggered by enemy's own colliders.
					//myCollider.enabled = false;
					myRigidbody2D.gravityScale = 0;
				}

				CheckCollisionsInBubble();
				BubbledMove();
			}
				break;
		}

		previousPos = transform.position;
	}

	#endregion

	#region Public methods

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
			TurnAround();
		}
	}

	void CheckCollisionsInBubble()
	{
		if (bubbleCollider2D.IsTouching(playerCollider2D))
		{
			for (int i = 0; i <= Random.Range(minItemScoreDropped, maxItemScoreDropped); i++)
			{
				ItemScore spawn = Instantiate(itemscorePrefab, transform.position,
					Quaternion.Euler(0, 0, Random.Range(0, 360)));
				spawn.gameObject.GetComponent<Rigidbody2D>().velocity =
					spawn.transform.up * Random.Range(minSpeedItemScore, maxSpeedItemScore);
			}

			this.Die();
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
			myState = BasicEnemyStates.Jumping;
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

	private void BubbledMove()
	{
		startSinPos += (Vector2) transform.up * Time.deltaTime * bubbleSpeed;
		myRigidbody2D.MovePosition(startSinPos + Vector2.right * Mathf.Sin(sinTimer * frequency) * amplitude);
		//transform.position = startSinPos + Vector2.right * Mathf.Sin(sinTimer * frequency) * amplitude;
		sinTimer += Time.deltaTime;
	}

	#endregion
}