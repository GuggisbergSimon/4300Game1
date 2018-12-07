using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubblePopScript : MonoBehaviour {

    private GameObject enemy;

    private void Start()
    {
        enemy = GetComponentInParent<CapsuleCollider2D>().gameObject;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.gameObject.tag == "Player")
        {
            Destroy(enemy);
        }
    }
}
