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
	[SerializeField] float playerSpeed = 1;
	[SerializeField] float playerJump = 1;
	[SerializeField] bool debugging = false;
	[SerializeField] float raycastDistanceFromPlayer = 0.6f;

	// Private variables.
	Rigidbody2D playerRigidbody2D;
	Animator playerAnimator;
	TilemapCollider2D tilemapCollider;

	// Used to check whether the player can jump.
	bool isAirborne = false;

	// Used to flip sprite in the correct direction.
	bool movingRight = true;

	private bool hasPressedJump = false;

	#endregion

	/*// Avoids part of the script relying on references set in LateStart from running before the LateStart has run.
	bool lateStartIsDone = false;

	#region Custom functions

	// Used to get references stored in GameManager,
	// which initializes at the same time as everything else,
	// which causes an error if we're trying to get a reference from it before it's initialized.
	IEnumerator LateStart()
	{
		yield return new WaitForSeconds(0.05f);

		tilemapCollider = GameManager.Instance.levelTilemap.GetComponent<TilemapCollider2D>();
		lateStartIsDone = true;
	}

	#endregion*/

	#region Unity functions

	private void Start()
	{
		// Gets references.
		playerRigidbody2D = gameObject.GetComponent<Rigidbody2D>();
		playerAnimator = gameObject.GetComponentInChildren<Animator>();
		tilemapCollider = GameManager.Instance.levelTilemap.GetComponent<TilemapCollider2D>();
	}

	private void FixedUpdate()
	{
		if (!GameManager.Instance.paused || debugging)
		{
			HorizontalMovement();
			CheckAirborne();
			CheckJump();

			//TODO Code for dropping below platforms, currently not working as intended
			/*if (lateStartIsDone)
			{
				if (Input.GetAxisRaw("Vertical") < 0)
				{
					tilemapCollider.enabled = false;
				}
				else if (Input.GetAxisRaw("Vertical") == 0)
				{
					tilemapCollider.enabled = true;
				}
			}*/
		}
	}

	private void Update()
	{
		if (Input.GetButtonDown("Jump"))
		{
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
	}

	private void OnCollisionEnter2D(Collision2D other)
	{
		if (other.gameObject.CompareTag("Enemy"))
		{
			this.Die();
			GameManager.Instance.LoadLevel("GameOver");
		}
	}

	#endregion

	#region Private methods

	private void CheckAirborne()
	{
		// Casts a CircleCast at player's feet and sets isAirborne variable to false if the cast returned a collision.
		Vector3 originRaycast = new Vector3(transform.position.x, transform.position.y - raycastDistanceFromPlayer,
			transform.position.z - 0.5f);
		RaycastHit2D groundHit = Physics2D.Raycast(originRaycast, Vector3.forward);
		if (groundHit != false && groundHit.collider.gameObject.CompareTag("Level"))
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