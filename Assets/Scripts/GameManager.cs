using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public static GameManager Instance { get; private set; } = null; // Self reference for use in other scripts.

    #region Public References

    public GameObject player;
    public GameObject projectileSpawner;
    #endregion

    #region Unity Functions

    private void Start()
    {
        Instance = this;

        player = GameObject.FindGameObjectWithTag("Player");
        projectileSpawner = GameObject.FindGameObjectWithTag("ProjectileSpawner");
    }
    #endregion
}
