using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	// Self reference for use in other scripts.
	public static GameManager Instance { get; private set; } = null;

	#region Public References

	public GameObject player;
	public GameObject projectileSpawner;
	public GameObject levelTilemap;

	#endregion

	#region Public variables

	[HideInInspector] public bool startedGame = false;
	[HideInInspector] public bool hidePausePanel = true;
	[HideInInspector] public bool paused = true;

	#endregion

	#region Custom Functions

	public void ResetGameManager()
	{
		Instance = this;
		player = GameObject.FindGameObjectWithTag("Player");
		projectileSpawner = GameObject.FindGameObjectWithTag("ProjectileSpawner");
		levelTilemap = GameObject.FindGameObjectWithTag("Level");

		startedGame = false;
		hidePausePanel = true;
		paused = true;
	}

	private void CheckEscape()
	{
		if (Input.GetKeyDown(KeyCode.Escape))
		{
#if UNITY_EDITOR
			UnityEditor.EditorApplication.isPlaying = false;
#else
		Application.Quit();
#endif
		}
	}

	private void CheckPause()
	{
		if (startedGame)
		{
			if (Input.GetButtonDown("Pause"))
			{
				if (paused)
				{
					paused = false;
					hidePausePanel = true;
				}
				else
				{
					paused = true;
					hidePausePanel = false;
				}
			}
		}
	}

	#endregion

	#region Unity Functions

	private void Awake()
	{
		Instance = this;

		player = GameObject.FindGameObjectWithTag("Player");
		projectileSpawner = GameObject.FindGameObjectWithTag("ProjectileSpawner");
		levelTilemap = GameObject.FindGameObjectWithTag("Level");
	}

	private void Update()
	{
		CheckEscape();
		CheckPause();
	}

	#endregion
}