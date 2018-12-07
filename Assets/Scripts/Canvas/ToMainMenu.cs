using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToMainMenu : MonoBehaviour {

    [SerializeField] GameObject mainMenuPanel;

    public void GoToMainMenu()
    {
        GameManager.Instance.ResetGameManager();

        mainMenuPanel.SetActive(true);
    }
}
