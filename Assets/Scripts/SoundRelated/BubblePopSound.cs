using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubblePopSound : MonoBehaviour {

    IEnumerator SelfDestroyInSeconds(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        Destroy(gameObject);
    }
}
