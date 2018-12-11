using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.Tilemaps;

// Controls player movement and animations associated with it.
public class PlayerMovement : Actor
{
	#region Variables

	// Serialized variables.
	[SerializeField] private float playerSpeed = 1;
	[SerializeField] private float playerJump = 1;
	[SerializeField] private bool debugging = false;
	[SerializeField] private GameObject groundDetector;

    // Private variables.
    private Rigidbody2D playerRigidbody2D;
	private Animator playerAnimator;
	private CompositeCollider2D tilemapCollider;
	private Collider2D groundDetectorCollider2D;
	//private Collider2D myCollider2D;

	// Used to check whether the player can jump.
	private bool isAirborne = false;

	// Used to flip sprite in the correct direction.
	private bool movingRight = true;

	private bool hasPressedJump = false;

	#endregion

	#region Unity functions

	private void Start()
	{
		// Gets references.
		playerRigidbody2D = gameObject.GetComponent<Rigidbody2D>();
		playerAnimator = gameObject.GetComponentInChildren<Animator>();
		tilemapCollider = GameManager.Instance.levelTilemap.GetComponent<CompositeCollider2D>();
		groundDetectorCollider2D = groundDetector.GetComponent<Collider2D>();
		//myCollider2D = gameObject.GetComponent<Collider2D>();
	}

	private void FixedUpdate()
	{
		if (!GameManager.Instance.paused || debugging)
		{
			HorizontalMovement();
			CheckAirborne();
			CheckJump();
		}
	}

	private void Update()
	{
		if (Input.GetButtonDown("Jump"))
		{
            SoundManager.Instance.PlaySound(SoundManager.Sound.JUMP);
			hasPressedJump = true;
		}
		else if (Input.GetButtonUp("Jump"))
		{
			hasPressedJump = false;
		}

		// Responsible for transitioning between jumping and falling animations.
		if (playerRigidbody2D.velocity.y < 0)
		{
			playerAnimator.SetBool("isFalling", true);
			playerAnimator.SetBool("isJumping", false);
		}
		else
		{
			playerAnimator.SetBool("isFalling", false);
		}

		// Handle the dropdown of player
		//TODO player can be stuck inside platform, it's due to composite collider
		/*if (Input.GetAxisRaw("Vertical") < 0)
		{
			myCollider2D.isTrigger = true;
		}*/
	}

	/*private void OnTriggerExit2D(Collider2D other)
	{
		myCollider2D.isTrigger = false;
	}*/

	private void OnCollisionEnter2D(Collision2D other)
	{
	//TODO add check for bubbled state
		if (other.gameObject.CompareTag("Enemy") && !other.gameObject.GetComponent<Enemy>().IsBubble)
		{
			this.Die();
			GameManager.Instance.LoadLevel("GameOver");
		}
	}

	#endregion

	#region Private methods

	private void CheckAirborne()
	{
		if (groundDetectorCollider2D.IsTouching(tilemapCollider))
		{
			isAirborne = false;
			playerAnimator.SetBool("isJumping", false);
		}
		else
		{
			isAirborne = true;
		}
	}

	private void HorizontalMovement()
	{
		// Horizontal movement
		float currentInputHorizontal = Input.GetAxisRaw("Horizontal");
		if (currentInputHorizontal.CompareTo(0) != 0)
		{
			playerRigidbody2D.velocity =
				Vector2.right * playerSpeed * currentInputHorizontal + Vector2.up * playerRigidbody2D.velocity;
			playerAnimator.SetBool("isIdle", false);
			if ((currentInputHorizontal > 0 && !movingRight) || (currentInputHorizontal < 0 && movingRight))
			{
				TurnAround();
			}
		}
		// No horizontal movement input.
		else
		{
			// Sets player velocity to 0 on x axis.
			playerRigidbody2D.velocity = Vector2.up * playerRigidbody2D.velocity;
			playerAnimator.SetBool("isIdle", true);
		}
	}

	private void TurnAround()
	{
		this.transform.Rotate(Vector3.up * 180);
		movingRight = !movingRight;
	}

	private void CheckJump()
	{
		if (hasPressedJump)
		{
			if (!isAirborne)
			{
				// Sets y velocity to new velocity.
				playerRigidbody2D.velocity = new Vector2(playerRigidbody2D.velocity.x, Vector2.up.y * playerJump);
				isAirborne = true;
				playerAnimator.SetBool("isJumping", true);
				hasPressedJump = false;
			}
		}

		// Prevents the player from jumping multiple times.
		/*if (isAirborne)
		{
			// A coroutine with a WaitForSeconds() is necessary, otherwise isAirborne bool would be set back to false as soon as it's set to true.
			// StartCoroutine(CheckGround());
		}*/
	}

	#endregion
}