using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BasicEnemy : MonoBehaviour
{
	[SerializeField] private GameObject player;
	[SerializeField] private float speed = 3;
	[SerializeField] private float jumpForce = 150;
	[SerializeField] private GameObject jumpPosition;
	[SerializeField] private GameObject jumpCheckPlatForm;
	[SerializeField] private GameObject groundDetector;
	[SerializeField] private GameObject frontDetector;
	[SerializeField] private Tilemap tilemap;
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

	private Collider2D groundDetectorCollider2D;
	private Collider2D frontDetectorCollider2D;
	private Collider2D jumpPositionCollider2D;
	private Collider2D jumpCheckPlatFormCollider2D;
	private TilemapCollider2D tilemapCollider2D;
    private CapsuleCollider2D myCollider;


	#region Inherited methods

	private void Start()
	{
		myRigidbody2D = GetComponent<Rigidbody2D>();
		float myRotation = this.transform.rotation.eulerAngles.y;
		isLookingRight = -90.0f > myRotation && myRotation > 90.0f;
		//TODO add checkstate somewhere here

		groundDetectorCollider2D = groundDetector.GetComponent<Collider2D>();
		frontDetectorCollider2D = frontDetector.GetComponent<Collider2D>();
		jumpPositionCollider2D = jumpPosition.GetComponent<Collider2D>();
		jumpCheckPlatFormCollider2D = jumpCheckPlatForm.GetComponent<Collider2D>();
		tilemapCollider2D = tilemap.GetComponent<TilemapCollider2D>();
        myCollider = GetComponent<CapsuleCollider2D>();

        bubble.SetActive(false);
	}

	private void Update()
	{
		switch (myState)
		{
			case BasicEnemyStates.Falling:
			{
				CheckPlayerPosX();
				CheckGround();
			}
				break;
			case BasicEnemyStates.Jumping:
			{
				CheckGround();
			}
				break;
			case BasicEnemyStates.Running:
			{
				MoveForward();
				CheckPlayerPosY();
				CheckForJump();
				CheckGround();
				CheckFront();
			}
				break;
			case BasicEnemyStates.InBubble:
			{
                if (!bubble.activeSelf)
                {
                    bubble.SetActive(true);
                    myCollider.enabled = false; // Avoids the bubble collider from being triggered by enemy's own colliders.
                    frontDetectorCollider2D.enabled = false;
                    groundDetectorCollider2D.enabled = false;

                    myRigidbody2D.gravityScale = 0;
                }

                BubbledMove();
			}
				break;
		}
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
		if (diffPosY < 0)
		{
			wantsToJump = true;
		}
		else
		{
			wantsToJump = false;
		}
	}

	//Checks if jump is possible, if so, jump
	private void CheckForJump()
	{
		if (wantsToJump && myState == BasicEnemyStates.Running &&
		    !jumpPositionCollider2D.IsTouching(tilemapCollider2D) &&
		    jumpCheckPlatFormCollider2D.IsTouching(tilemapCollider2D))
		{
			myRigidbody2D.AddForce(Vector2.up*jumpForce);
			//myRigidbody2D.velocity = (jumpPosition.transform.position - transform.position)*jumpForce;
			//Debug.DrawLine(transform.position, jumpPosition.transform.position - transform.position,Color.red,1.0f);
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
		this.transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y + 180, 0);
	}

    private void BubbledMove()
    {
        myRigidbody2D.velocity = new Vector2(); // Resetting velocity.

        int randomNumber = Random.Range(0,100);

        if (randomNumber > 50)
        {
            transform.position = new Vector2(transform.position.x, transform.position.y + 0.1f); // Moves the bubbled enemy upwards.
        }
        else if (randomNumber > 0 && randomNumber < 25)
        {
            transform.position = new Vector2(transform.position.x + 0.1f, transform.position.y); // Moves the bubbled enemy to the right.
        }
        else
        {
            transform.position = new Vector2(transform.position.x - 0.1f, transform.position.y); // Moves the bubbled enemy to the left.
        }
    }

	#endregion
}