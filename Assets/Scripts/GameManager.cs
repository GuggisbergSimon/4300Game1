using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
	// Self reference for use in other scripts.
	private static GameManager instance = null;
	public static GameManager Instance => instance;
	//public static GameManager Instance { get; private set; } = null;

	#region Public References

	public GameObject player;
	//public GameObject projectileSpawner;
	public GameObject levelTilemap;

	#endregion

	#region Variables

	/*
	[HideInInspector] public bool startedGame = false;
	[HideInInspector] public bool hidePausePanel = true;
	[HideInInspector] public bool paused = true;
	*/

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

	public void AddScore(float points)
	{
		score += points;
		UIManager.UpdateScore();
	}

	public void ResetScore()
	{
		score = 0.0f;
	}

	private void OnEnable()
	{
		SceneManager.sceneLoaded += OnLevelFinishedLoadingScene;
	}

	private void OnDisable()
	{
		SceneManager.sceneLoaded -= OnLevelFinishedLoadingScene;
	}

	void OnLevelFinishedLoadingScene(Scene scene, LoadSceneMode mode)
	{
		Setup();
	}

	public void Quit()
	{
#if UNITY_EDITOR
		UnityEditor.EditorApplication.isPlaying = false;
#else
		Application.Quit();
#endif
	}

	public float Score => score;

	private void CheckEscape()
	{
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			Quit();
		}
	}

	/*private void CheckPause()
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
	}*/

	private void CheckWin()
	{
		if (inLevel && enemies.Count == 0)
		{
			LoadLevel("EndingScene");
		}
	}

	private void Setup()
	{
		canvas = FindObjectOfType<Canvas>();
		UIManager = canvas.gameObject.GetComponent<UIManager>();
		player = GameObject.FindGameObjectWithTag("Player");
		//projectileSpawner = GameObject.FindGameObjectWithTag("ProjectileSpawner");
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
		if (instance != null && instance != this)
		{
			Destroy(this.gameObject);
		}
		else
		{
			instance = this;
			DontDestroyOnLoad(this.gameObject);
		}
		Setup();
	}

	private void Update()
	{
		CheckEscape();
		//CheckPause();
		CheckWin();
	}

	#endregion
}