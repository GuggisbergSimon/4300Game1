using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public static GameManager Instance { get; private set; } = null; // Self reference for use in other scripts.

    #region Public References

    public GameObject player;
    public GameObject projectileSpawner;
    public GameObject levelTilemap;
    #endregion

    #region Public variables

    public bool startedGame = false;
    public bool hidePausePanel = true;
    public bool paused = true;
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
    #endregion

    #region Unity Functions

    private void Start()
    {
        Instance = this;

        player = GameObject.FindGameObjectWithTag("Player");
        projectileSpawner = GameObject.FindGameObjectWithTag("ProjectileSpawner");
        levelTilemap = GameObject.FindGameObjectWithTag("Level");
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #endif

            Application.Quit();
        }

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
    #endregion
}
