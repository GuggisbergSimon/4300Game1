using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Actor : MonoBehaviour
{
	private bool isAlive = true;

	public void Die()
	{
		if (isAlive)
		{
			isAlive = false;
			Destroy(this.gameObject);
		}
	}
}