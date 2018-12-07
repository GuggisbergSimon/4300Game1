using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatsPanelController : MonoBehaviour {

    [SerializeField] GameObject statsPanel;

    private void Update()
    {
        if (!GameManager.Instance.paused)
        {
            if (!statsPanel.activeSelf)
            {
                statsPanel.SetActive(true);
            }
        }
        else
        {
            if (statsPanel.activeSelf)
            {
                statsPanel.SetActive(false);
            }
        }
    }
}
