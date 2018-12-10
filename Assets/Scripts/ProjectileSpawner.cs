﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Object responsible for spawning projectiles.
public class ProjectileSpawner : MonoBehaviour
{
	[SerializeField] GameObject projectilePrefab;
	[SerializeField] bool debugging = false;

	private void Update()
	{
		if (!GameManager.Instance.paused || debugging)
		{
			if (Input.GetButtonDown("Fire1"))
			{
				// Note: The position difference between the the player and the spawner allows orientation of the projectiles... Not sure if necessary?
				Instantiate(projectilePrefab, transform.position, transform.rotation);
			}
		}
	}
}