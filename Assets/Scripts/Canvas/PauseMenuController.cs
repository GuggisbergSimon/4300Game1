using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenuController : MonoBehaviour {

    [SerializeField] GameObject pausePanel;

    private void Update()
    {
        if (GameManager.Instance.paused && !GameManager.Instance.hidePausePanel)
        {
            if (!pausePanel.activeSelf)
            {
                pausePanel.SetActive(true);
            }
        }
        else if (GameManager.Instance.hidePausePanel)
        {
            if (pausePanel.activeSelf)
            {
                pausePanel.SetActive(false);
            }
        }
    }
}
