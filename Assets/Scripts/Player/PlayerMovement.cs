using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

// Controls player movement and animations associated with it.
public class PlayerMovement : MonoBehaviour
{
	#region Variables

	// Serialized variables.
	[SerializeField] float playerSpeedMultiplier = 1;
	[SerializeField] float playerJumpMultiplier = 1;
	[SerializeField] bool debugging = false;
	[SerializeField] float raycastDistanceFromPlayer = 0.6f;

	// Private variables.
	Rigidbody2D playerRigidbody2D;
	Animator playerAnimator;

	float playerSpeed = 1; // Base horizontal velocity.
	float playerJump = 1; // Base jumping velocity.

	bool isAirborne = false; // Used to check whether the player can jump.
	bool movingRight = true; // Used to flip sprite in the correct direction.

	#endregion

	#region Unity functions

	private void Start()
	{
		// Gets references.
		playerRigidbody2D = gameObject.GetComponent<Rigidbody2D>();
		playerAnimator = gameObject.GetComponentInChildren<Animator>();
	}

	private void FixedUpdate()
	{
		if (!GameManager.Instance.paused || debugging)
		{
			// Horizontal movement.
			if (Input.GetAxisRaw("Horizontal") > 0) // Joystick to the right.
			{
				if (movingRight) // Ensures the sprite is flipped the right way.
				{
					playerRigidbody2D.velocity = new Vector2(Vector2.right.x * playerSpeed * playerSpeedMultiplier,
						playerRigidbody2D.velocity.y); // Sets x velocity to new velocity.
					playerAnimator.SetBool("isIdle",
						false); // Note: is it worth it to check the state of the bool before assigning it or nah? Seems to me that it's more performant to just set the bool.
				}
				else
				{
					playerRigidbody2D.velocity = new Vector2(Vector2.right.x * playerSpeed * playerSpeedMultiplier,
						playerRigidbody2D.velocity.y);
					playerAnimator.SetBool("isIdle", false);

					// Flips the sprite the right way.
					GameManager.Instance.player.transform.Rotate(0, 180, 0);
					movingRight = true;
				}
			}
			else if (Input.GetAxisRaw("Horizontal") < 0) // Joystick to the left.
			{
				if (!movingRight)
				{
					playerRigidbody2D.velocity = new Vector2(Vector2.left.x * playerSpeed * playerSpeedMultiplier,
						playerRigidbody2D.velocity.y);
					playerAnimator.SetBool("isIdle", false);
				}
				else
				{
					playerRigidbody2D.velocity = new Vector2(Vector2.left.x * playerSpeed * playerSpeedMultiplier,
						playerRigidbody2D.velocity.y);
					playerAnimator.SetBool("isIdle", false);

					GameManager.Instance.player.transform.Rotate(0, 180, 0);
					movingRight = false;
				}
			}
			else // No horizontal movement input.
			{
				playerRigidbody2D.velocity = new Vector2(0, playerRigidbody2D.velocity.y); // Sets player velocity to 0 on x axis.

				playerAnimator.SetBool("isIdle", true);
			}

			RaycastHit2D groundHit = Physics2D.Raycast(new Vector3(transform.position.x, transform.position.y - raycastDistanceFromPlayer, transform.position.z - 0.5f), Vector3.forward);
			if (groundHit != false && groundHit.collider.gameObject.tag == "Level") // Casts a CircleCast at player's feet and sets isAirborne variable to false if the cast returned a collision.
			{
				isAirborne = false;

				playerAnimator.SetBool("isJumping", false);
			}
			else
			{
				isAirborne = true;
			}

			// Vertical movement.
			if (Input.GetButtonDown("Jump"))
			{
				if (!isAirborne)
				{
					playerRigidbody2D.velocity = new Vector2(playerRigidbody2D.velocity.x,
						Vector2.up.y * playerJump * playerJumpMultiplier); // Sets y velocity to new velocity.
					isAirborne = true;

					playerAnimator.SetBool("isJumping", true);
				}
			}

			// Prevents the player from jumping multiple times.
			if (isAirborne)
			{
				// StartCoroutine(CheckGround()); // A coroutine with a WaitForSeconds() is necessary, otherwise isAirborne bool would be set back to false as soon as it's set to true.
			}
		}
	}

	private void Update()
	{
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

	#endregion
}