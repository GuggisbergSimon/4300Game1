using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
	// Self reference for use in other scripts.
	public static GameManager Instance { get; private set; } = null;

	#region Public References

	public GameObject player;
	public GameObject projectileSpawner;
	public GameObject levelTilemap;

	#endregion

	#region Variables

	[HideInInspector] public bool startedGame = false;
	[HideInInspector] public bool hidePausePanel = true;
	[HideInInspector] public bool paused = true;

	[SerializeField] private Canvas canvas;
	private List<GameObject> enemies;
	private UIManager UIManager;
	private bool inLevel = false;
	private float score = 0.0f;

	#endregion

	#region Custom Functions

	public void LoadLevel(string nameLevel)
	{
		SceneManager.LoadScene(nameLevel);
	}

	public void ResetGameManager()
	{
		Setup();

		startedGame = false;
		hidePausePanel = true;
		paused = true;
	}

	public void AddScore(float points)
	{
		score += points;
		UIManager.UpdateScore();
	}

	public void ResetScore()
	{
		score = 0.0f;
	}

	public float Score => score;

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
		if (startedGame && Input.GetButtonDown("Pause"))
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

	private void CheckWin()
	{
		if (inLevel && enemies.Count == 0)
		{
			LoadLevel("EndingScene");
		}
	}

	private void Setup()
	{
		Instance = this;
		canvas = FindObjectOfType<Canvas>();
		UIManager = canvas.gameObject.GetComponent<UIManager>();
		player = GameObject.FindGameObjectWithTag("Player");
		projectileSpawner = GameObject.FindGameObjectWithTag("ProjectileSpawner");
		levelTilemap = GameObject.FindGameObjectWithTag("Level");
		enemies = GameObject.FindGameObjectsWithTag("Enemy").ToList();

		if (enemies.Count > 0)
		{
			inLevel = true;
		}
		else
		{
			inLevel = false;
		}
	}

	public void AddEnemy(GameObject enemy)
	{
		enemies.Add(enemy);
	}

	public void RemoveEnemy(GameObject enemy)
	{
		enemies.Remove(enemy);
	}

	#endregion

	#region Unity Functions

	private void Awake()
	{
		Setup();
	}

	private void Update()
	{
		CheckEscape();
		CheckPause();
		CheckWin();
	}

	#endregion
}