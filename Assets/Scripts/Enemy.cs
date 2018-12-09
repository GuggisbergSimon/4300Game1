using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Actor
{
	protected bool isBubble = false;

	public bool IsBubble => isBubble;

	public new void Die()
	{
		base.Die();
		GameManager.Instance.RemoveEnemy(this.gameObject);
	}

}
