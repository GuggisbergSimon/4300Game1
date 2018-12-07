using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

// Controls player movement and animations associated with it.
public class PlayerMovement : MonoBehaviour
{
	#region References

	// Placeholder

    #region Variables
    // Serialized variables.
    [SerializeField] float playerSpeedMultiplier = 1;
    [SerializeField] float playerJumpMultiplier = 1;
    [SerializeField] float circleCastRadius = 0.1f; // CircleCast is used to avoid infinite jumping.
    [SerializeField] bool debugging = false;
    [SerializeField] float raycastDistanceFromPlayer = 0.6f;

	#region Variables

	// Serialized variables.
	[SerializeField] float playerSpeedMultiplier = 1;
	[SerializeField] float playerJumpMultiplier = 1;
	[SerializeField] float circleCastRadius = 0.3f; // CircleCast is used to avoid infinite jumping.
	[SerializeField] bool isDebug = true;

	// Private variables.
	Rigidbody2D playerRigidbody2D;
	Animator playerAnimator;
	TilemapCollider2D tilemapCollider;

	int PIXELS_PER_UNIT = 100;
	float playerSpriteSizeInPixels; // Used for CircleCast positioning.

	float playerSpeed = 1; // Base horizontal velocity.
	float playerJump = 1; // Base jumping velocity.

	bool isAirborne = false; // Used to check whether the player can jump.
	bool movingRight = true; // Used to flip sprite in the correct direction.

	bool
		lateStartIsDone =
			false; // Avoids part of the script relying on references set in LateStart from running before the LateStart has run.

	#endregion

	#region Custom functions

	IEnumerator CheckGround() // Checks whether the player is standing on solid ground or not.
	{
		yield return new WaitForSeconds(0.05f);

		if (Physics2D.CircleCast(
			    new Vector2(transform.position.x,
				    transform.position.y - (playerSpriteSizeInPixels / 2.0f) / PIXELS_PER_UNIT -
				    circleCastRadius / 2.0f - 0.1f), circleCastRadius, Vector3.forward).collider !=
		    null
		) // Casts a CircleCast at player's feet and sets isAirborne variable to false if the cast returned a collision.
		{
			isAirborne = false;

			playerAnimator.SetBool("isJumping", false);
		}
	}

	IEnumerator
		LateStart() // Used to get references stored in GameManager, which initializes at the same time as everything else which causes an error if we're trying to get a reference from it before it's initialized.
	{
		yield return new WaitForSeconds(0.05f);

    private void FixedUpdate()
    {
        if (!GameManager.Instance.paused || debugging)
        {
            // Horizontal movement.
            if (Input.GetAxisRaw("Horizontal") > 0) // Joystick to the right.
            {
                if (movingRight) // Ensures the sprite is flipped the right way.
                {
                    playerRigidbody2D.velocity = new Vector2(Vector2.right.x * playerSpeed * playerSpeedMultiplier, playerRigidbody2D.velocity.y); // Sets x velocity to new velocity.
                    playerAnimator.SetBool("isIdle", false); // Note: is it worth it to check the state of the bool before assigning it or nah? Seems to me that it's more performant to just set the bool.
                }
                else
                {
                    playerRigidbody2D.velocity = new Vector2(Vector2.right.x * playerSpeed * playerSpeedMultiplier, playerRigidbody2D.velocity.y);
                    playerAnimator.SetBool("isIdle", false);

	#endregion

	#region Unity functions

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
                    playerRigidbody2D.velocity = new Vector2(playerRigidbody2D.velocity.x, Vector2.up.y * playerJump * playerJumpMultiplier); // Sets y velocity to new velocity.
                    isAirborne = true;

		playerSpriteSizeInPixels = PIXELS_PER_UNIT * transform.localScale.y; // Sets up variable.
	}

            // Prevents the player from jumping multiple times.
            if (isAirborne)
            {
                // StartCoroutine(CheckGround()); // A coroutine with a WaitForSeconds() is necessary, otherwise isAirborne bool would be set back to false as soon as it's set to true.
            }

            if (lateStartIsDone)
            {
                if (Input.GetAxisRaw("Vertical") < 0)
                {
                    tilemapCollider.enabled = false;
                }
                else if (Input.GetAxisRaw("Vertical") == 0)
                {
                    tilemapCollider.enabled = true;
                }
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