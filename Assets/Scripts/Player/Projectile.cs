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

	// Used to fire the projectile the right way when turning right and left.
	BasicEnemy enemyScript;

	#endregion

	#region Unity functions

	// Allows the script to know what object the projectile has collided with.
	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.gameObject.CompareTag("Enemy"))
		{
			enemyScript = collision.gameObject.GetComponent<BasicEnemy>();
			enemyScript.Bubble();
			Destroy(this.gameObject);
		}
		else if (collision.gameObject.CompareTag("Level"))
		{
			Destroy(this.gameObject);
		}
	}

	private void Start()
	{
		projectileRigidbody2D = GetComponent<Rigidbody2D>();
	}

	private void Update()
	{
		// Moves the projectile.
		projectileRigidbody2D.velocity = transform.right * projectileSpeed;
	}

	#endregion
}