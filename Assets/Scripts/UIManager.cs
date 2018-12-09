using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
	[SerializeField] private TextMeshProUGUI scoreText;

	public void UpdateScore()
	{
		scoreText.text = GameManager.Instance.Score.ToString();
	}
}
