using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resume : MonoBehaviour
{
	[SerializeField] GameObject pausePanel;

	public void ResumeGame()
	{
		GameManager.Instance.paused = false;

		pausePanel.SetActive(false);
	}
}