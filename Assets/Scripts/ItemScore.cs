using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemScore : MonoBehaviour
{
	[SerializeField] private float scorePoints = 1000;

	private bool isTaken = false;

	private void OnCollisionEnter2D(Collision2D other)
	{
		if (other.gameObject.CompareTag("Player") && !isTaken)
		{
			GameManager.Instance.AddScore(scorePoints);
			Destroy(this.gameObject);
			isTaken = true;
		}
	}
}
