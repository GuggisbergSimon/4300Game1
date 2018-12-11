using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Actor
{
	[SerializeField] private GameObject bubble;
	[SerializeField] private float bubbleSpeed = 2;
	[SerializeField] private float frequency = 1;
	[SerializeField] private float amplitude = 1;
	[SerializeField] private Sprite bubbleSprite;
	[SerializeField] private ItemScore itemscorePrefab;
	[SerializeField] private int minItemScoreDropped = 1;
	[SerializeField] private int maxItemScoreDropped = 5;
	[SerializeField] private float minSpeedItemScore = 0.3f;
	[SerializeField] private float maxSpeedItemScore = 6.0f;

	private Collider2D playerCollider2D;
	private Collider2D bubbleCollider2D;
	private Vector2 startSinPos;
	private float sinTimer = 0.0f;
	private Animator myAnimator;
	protected Rigidbody2D myRigidbody2D;
	protected GameObject player;
	protected bool isBubble = false;

	protected void Start()
	{
		player = GameManager.Instance.player;
		myAnimator = GetComponentInChildren<Animator>();
		playerCollider2D = player.GetComponent<Collider2D>();
		myRigidbody2D = GetComponent<Rigidbody2D>();
		bubbleCollider2D = bubble.GetComponent<Collider2D>();
		bubble.SetActive(false);
	}

	public bool IsBubble
	{
		get { return isBubble; }
		set { isBubble = value; }
	}

	public new void Die()
	{
		base.Die();
		GameManager.Instance.RemoveEnemy(this.gameObject);
	}

	protected void BubbleMove()
	{
		if (!bubble.activeSelf)
		{
			myAnimator.SetTrigger("InBubble");
			startSinPos = transform.position;
            SoundManager.Instance.PlaySound(SoundManager.Sound.BUBBLING);
			isBubble = true;
			GetComponentInChildren<SpriteRenderer>().sprite = bubbleSprite;
			bubble.SetActive(true);
			// Avoids the bubble collider from being triggered by enemy's own colliders.
			myRigidbody2D.gravityScale = 0;
		}

		CheckCollisionsInBubble();
		SinMove();
	}

	private void SinMove()
	{
		startSinPos += (Vector2)transform.up * Time.deltaTime * bubbleSpeed;
		myRigidbody2D.MovePosition(startSinPos + Vector2.right * Mathf.Sin(sinTimer * frequency) * amplitude);
		sinTimer += Time.deltaTime;
	}

	void CheckCollisionsInBubble()
	{
		if (bubbleCollider2D.IsTouching(playerCollider2D))
		{
			for (int i = 0; i <= Random.Range(minItemScoreDropped, maxItemScoreDropped); i++)
			{
				ItemScore spawn = Instantiate(itemscorePrefab, transform.position,
					Quaternion.Euler(0, 0, Random.Range(0, 360)));

				Vector2 test = spawn.transform.up * Random.Range(minSpeedItemScore, maxSpeedItemScore);
				spawn.gameObject.GetComponent<Rigidbody2D>().velocity = test;
				Debug.DrawLine(transform.position, transform.position + (Vector3)test);
			}

            SoundManager.Instance.PlaySound(SoundManager.Sound.BUBBLE_POP);
			this.Die();
		}
	}
}
