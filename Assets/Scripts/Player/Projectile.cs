using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Controlls projectile's behaviour.
public class Projectile : MonoBehaviour
{
	#region Variables

	// Serialized variables
	[SerializeField] float projectileSpeed = 5.0f;

	// Private variables
	Rigidbody2D projectileRigidbody2D;
	Collider2D collision = null; // Used to manage projectile's destruction.
	Vector3 projectileDirection; // Used to fire the projectile the right way when turning right and left.
	BasicEnemy enemyScript;

	#endregion

	#region Unity functions

	private void
		OnTriggerEnter2D(
			Collider2D collision) // Allows the script to know what object the projectile has collided with.
	{
		this.collision = collision;
	}

	private void Start()
	{
		projectileRigidbody2D = GetComponent<Rigidbody2D>();
		projectileDirection = Vector3.Normalize(GameManager.Instance.projectileSpawner.transform.position -
		                                        GameManager.Instance.player.transform
			                                        .position); // Set up once, gives the projectile the direction in which to fly: (1,0,0) when facing right, (-1,0,0) when facing left.
	}

	private void Update()
	{
		projectileRigidbody2D.velocity = projectileDirection * projectileSpeed; // Moves the projectile.

		if (collision != null) // If collided with something.
		{
			if (collision.gameObject.tag == "Enemy") // If collided with enemy, bubble the enemy and destroy self.
			{
				// Bubble the enemy.
				enemyScript = collision.gameObject.GetComponent<BasicEnemy>();
				enemyScript.Bubble();

				Destroy(gameObject);
			}
			else if (collision.gameObject.tag == "Level")
			{
				Destroy(gameObject);
			}
		}
	}

	#endregion
}