using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerDetector : MonoBehaviour
{
	private bool triggerEntered = false;

	public bool TriggerEntered => triggerEntered;

	private void OnTriggerEnter2D(Collider2D other)
	{
		triggerEntered = true;
	}

	private void OnTriggerExit2D(Collider2D other)
	{
		triggerEntered = false;
	}
}
