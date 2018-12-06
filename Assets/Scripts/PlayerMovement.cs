using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

// Controls player movement and animations associated with it.
public class PlayerMovement : MonoBehaviour {

    #region References
    // Placeholder
    #endregion

    #region Variables
    // Serialized variables.
    [SerializeField] float playerSpeedMultiplier = 1;
    [SerializeField] float playerJumpMultiplier = 1;
    [SerializeField] float circleCastRadius = 0.3f; // CircleCast is used to avoid infinite jumping.

    // Private variables.
    Rigidbody2D playerRigidbody2D;
    Animator playerAnimator;
    TilemapCollider2D tilemapCollider;

    int PIXELS_PER_UNIT = 100;
    float playerSpriteSizeInPixels; // Used for CircleCast positioning.

    float playerSpeed = 1;  // Base horizontal velocity.
    float playerJump = 1;   // Base jumping velocity.

    bool isAirborne = false; // Used to check whether the player can jump.
    bool movingRight = true; // Used to flip sprite in the correct direction.
    bool lateStartIsDone = false; // Avoids part of the script relying on references set in LateStart from running before the LateStart has run.
    #endregion

    #region Custom functions
    IEnumerator CheckGround() // Checks whether the player is standing on solid ground or not.
    {
        yield return new WaitForSeconds(0.05f);

        if (Physics2D.CircleCast(new Vector2(transform.position.x, transform.position.y - (playerSpriteSizeInPixels / 2.0f) / PIXELS_PER_UNIT - circleCastRadius / 2.0f - 0.1f), circleCastRadius, Vector3.forward).collider != null) // Casts a CircleCast at player's feet and sets isAirborne variable to false if the cast returned a collision.
        {
            isAirborne = false;

            playerAnimator.SetBool("isJumping", false);
        }
    }

    IEnumerator LateStart() // Used to get references stored in GameManager, which initializes at the same time as everything else which causes an error if we're trying to get a reference from it before it's initialized.
    {
        yield return new WaitForSeconds(0.05f);

        tilemapCollider = GameManager.Instance.levelTilemap.GetComponent<TilemapCollider2D>();
        lateStartIsDone = true;
    }
    #endregion

    #region Unity functions

    private void Start()
    {
        // Gets references.
        playerRigidbody2D = gameObject.GetComponent<Rigidbody2D>();
        playerAnimator = gameObject.GetComponentInChildren<Animator>();

        StartCoroutine(LateStart());

        playerSpriteSizeInPixels = PIXELS_PER_UNIT * transform.localScale.y; // Sets up variable.
    }

    private void Update()
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

                // Flips the sprite the right way.
                GameManager.Instance.player.transform.Rotate(0, 180, 0);
                movingRight = true;
            }
        }
        else if(Input.GetAxisRaw("Horizontal") < 0) // Joystick to the left.
        {
            if (!movingRight)
            {
                playerRigidbody2D.velocity = new Vector2(Vector2.left.x * playerSpeed * playerSpeedMultiplier, playerRigidbody2D.velocity.y);
                playerAnimator.SetBool("isIdle", false);
            }
            else
            {
                playerRigidbody2D.velocity = new Vector2(Vector2.left.x * playerSpeed * playerSpeedMultiplier, playerRigidbody2D.velocity.y);
                playerAnimator.SetBool("isIdle", false);

                GameManager.Instance.player.transform.Rotate(0,180,0);
                movingRight = false;
            }
        }
        else // No horizontal movement input.
        {
            playerRigidbody2D.velocity = new Vector2(0, playerRigidbody2D.velocity.y); // Sets player velocity to 0 on x axis.

            playerAnimator.SetBool("isIdle", true);
        }

        // Vertical movement.
        if (Input.GetButtonDown("Jump"))
        {
            if (!isAirborne)
            {
                playerRigidbody2D.velocity = new Vector2(playerRigidbody2D.velocity.x, Vector2.up.y * playerJump * playerJumpMultiplier); // Sets y velocity to new velocity.
                isAirborne = true;

                playerAnimator.SetBool("isJumping", true);
            }
        }

        // Prevents the player from jumping multiple times.
        if (isAirborne)
        {
            StartCoroutine(CheckGround()); // A coroutine with a WaitForSeconds() is necessary, otherwise isAirborne bool would be set back to false as soon as it's set to true.
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
