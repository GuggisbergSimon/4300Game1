using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerKillerScript : MonoBehaviour {

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            // freeze and Display death animation
            // remove a life
        }
    }
}
