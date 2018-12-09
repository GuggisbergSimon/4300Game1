﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubblePopScript : MonoBehaviour
{
	private GameObject enemy;

	private void Start()
	{
		enemy = GetComponentInParent<Collider2D>().gameObject;
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.collider.CompareTag("Player"))
		{
			enemy.GetComponent<Actor>().Die();
		}
	}
}