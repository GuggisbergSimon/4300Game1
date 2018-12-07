using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HowToPlay : MonoBehaviour
{
	[SerializeField] GameObject howToPlayPanel;
	[SerializeField] GameObject currentPanel;

	public void DisplayHowToPlay()
	{
		if (!GameManager.Instance.hidePausePanel)
		{
			GameManager.Instance.hidePausePanel = true;
		}

		howToPlayPanel.SetActive(true);
		currentPanel.SetActive(false);
	}
}