using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Credits : MonoBehaviour {

    [SerializeField] GameObject creditsPanel;
    [SerializeField] GameObject mainMenuPanel;

    public void DisplayCredits()
    {
        creditsPanel.SetActive(true);
        mainMenuPanel.SetActive(false);
    }
}
