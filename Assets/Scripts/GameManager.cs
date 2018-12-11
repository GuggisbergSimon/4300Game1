using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public enum Level
    {
        MAINMENU,
        LEVEL_1,
        LEVEL_2,
        LEVEL_3,
        LEVEL_4,
        GAMEOVER,
        WIN
    }

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

	// [HideInInspector] public bool startedGame = false;
	[HideInInspector] public bool hidePausePanel = true;
    public bool paused = true;
    public bool debugMode = false;

	[SerializeField] private Canvas canvas;
	private List<GameObject> enemies;
	private UIManager UIManager;
	private bool inLevel = false;
	private float score = 0.0f;
    private Level currentScene = Level.MAINMENU;

	#endregion

	#region Custom Functions

	public float Score => score;

	// Score related.
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

    // User input related
	private void CheckEscape()
	{
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			Quit();
		}
	}

	private void CheckPause()
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

    // Win and lose related.
	private void CheckWin()
	{
		if (inLevel && enemies.Count == 0)
		{
            switch (currentScene)
            {
                case Level.LEVEL_1:
                    {
                        LoadLevel("Level2");
                        currentScene = Level.LEVEL_2;
                    }
                    break;
                case Level.LEVEL_2:
                    {
                        LoadLevel("Level3");
                        currentScene = Level.LEVEL_3;
                    }
                    break;
                case Level.LEVEL_3:
                    {
                        LoadLevel("Level4");
                        currentScene = Level.LEVEL_4;
                    }
                    break;
                case Level.LEVEL_4:
                    {
                        LoadLevel("EndingScene");
                    }
                    break;
                default:
                    {
                        Debug.LogError("Error in GameManager.cs: Level stored in currentScene variable is invalid: " + currentScene.ToString());
                    }break;
            }
		}
	}

    // GameManager scene loading and initialization related.
    public void LoadLevel(string nameLevel)
    {
        SceneManager.LoadScene(nameLevel);
    }

    // a virer
    public void ResetGameManager()
    {
        Setup();

        // startedGame = false;
        hidePausePanel = true;
        paused = true;
    }

    private void Setup()
	{
		canvas = FindObjectOfType<Canvas>();
		UIManager = canvas.gameObject.GetComponent<UIManager>();
		player = GameObject.FindGameObjectWithTag("Player");
		//projectileSpawner = GameObject.FindGameObjectWithTag("ProjectileSpawner");
		levelTilemap = GameObject.FindGameObjectWithTag("Level");
		enemies = GameObject.FindGameObjectsWithTag("Enemy").ToList();

        switch (SceneManager.GetActiveScene().name)
        {
            case "Level1":
                {
                    currentScene = Level.LEVEL_1;
                }break;
            case "Level2":
                {
                    currentScene = Level.LEVEL_2;
                }
                break;
            case "Level3":
                {
                    currentScene = Level.LEVEL_3;
                }
                break;
            case "Level4":
                {
                    currentScene = Level.LEVEL_4;
                }
                break;
        }

		if (enemies.Count > 0)
		{
			inLevel = true;
		}
		else
		{
			inLevel = false;
		}
	}

    // Enemy related.
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
        // Check for user input.
        CheckEscape();
        CheckPause();

        // Check for pausing.
        if (paused)
        {
            // Pauses the game if it isn't paused.
            if(Time.timeScale != 0)
            {
                Time.timeScale = 0;
            }
        }
        else
        {
            // Unpauses the game if it was paused.
            if(Time.timeScale != 1)
            {
                Time.timeScale = 1;
            }

            // Check for winning/losing conditions.
            CheckWin();
        }
	}

	#endregion
}