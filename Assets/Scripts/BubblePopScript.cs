using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubblePopScript : MonoBehaviour
{
	[SerializeField] private GameObject enemy;

	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.collider.gameObject.tag == "Player")
		{
			Destroy(enemy);
		}
	}
}