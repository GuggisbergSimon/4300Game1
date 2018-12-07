using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Back : MonoBehaviour
{
	[SerializeField] GameObject mainMenuPanel;
	[SerializeField] GameObject currentPanel;

	public void GoBackToMainMenu()
	{
		/*
		if (GameManager.Instance.hidePausePanel && GameManager.Instance.startedGame) // Avoids the pause menu from being displayed in the How to Play menu.
		{
		    GameManager.Instance.hidePausePanel = false;
		}*/

		if (GameManager.Instance.startedGame)
		{
			if (GameManager.Instance.hidePausePanel)
			{
				GameManager.Instance.hidePausePanel = false;
				currentPanel.SetActive(false);
			}
		}
		else
		{
			mainMenuPanel.SetActive(true);
			currentPanel.SetActive(false);
		}
	}
}