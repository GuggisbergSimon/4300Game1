using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Start : MonoBehaviour
{
	[SerializeField] GameObject gameManagerPrefab;
	[SerializeField] GameObject mainMenuPanel;

	public void PlayTheGame()
	{
		GameManager.Instance.paused = false;
		// GameManager.Instance.startedGame = true;

		mainMenuPanel.SetActive(false);
	}
}