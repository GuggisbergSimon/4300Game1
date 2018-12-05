using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public static GameManager Instance { get; private set; } = null; // Self reference for use in other scripts.

    private void Start()
    {
        Instance = this;
    }
}
