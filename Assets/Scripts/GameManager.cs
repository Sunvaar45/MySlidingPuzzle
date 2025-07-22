using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GridManager gridManager;
    public UIManager uiManager;
    public TextMeshProUGUI moveCounterUI;

    private void Start()
    {
        InitializeValues();
    }

    void InitializeValues()
    {
        gridManager.moveCounter = 0;
    }

    public void WinGame()
    {
        uiManager.victoryPanel.SetActive(true);
        gridManager.FreezeGrid();
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}