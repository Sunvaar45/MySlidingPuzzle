using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GridManager gridManager;
    // public GameObject VictoryPanel;

    private void Start()
    {
        InitializePanels();
        InitializeValues();
    }

    void InitializePanels()
    {
        // VictoryPanel.SetActive(false);        
    }

    void InitializeValues()
    {
        gridManager.moveCounter = 0;
    }

    public void WinGame()
    {
        // VictoryPanel.SetActive(true);
    }
}
