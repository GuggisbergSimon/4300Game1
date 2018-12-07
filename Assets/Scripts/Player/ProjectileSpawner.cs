using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Object responsible for spawning projectiles.
public class ProjectileSpawner : MonoBehaviour {
    
    [SerializeField] GameObject projectilePrefab;

    private void Update()
    {
        if (!GameManager.Instance.paused)
        {
            if (Input.GetButtonDown("Fire1"))
            {
                Instantiate(projectilePrefab, GameManager.Instance.player.transform.position, Quaternion.Euler(transform.position - GameManager.Instance.player.transform.position)); // Note: The position difference between the the player and the spawner allows orientation of the projectiles... Not sure if necessary?
            }
        }
    }
}
