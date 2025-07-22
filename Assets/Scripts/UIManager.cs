using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public GridManager gridManager;
    public TextMeshProUGUI moveCounterUI;
    public GameObject victoryPanel;

    private void Start()
    {
        InitializeUI();
    }

    private void InitializeUI()
    {
        victoryPanel.SetActive(false);
        UpdateMoveCounterUI();
    }

    public void UpdateMoveCounterUI()
    {
        moveCounterUI.text = gridManager.moveCounter.ToString();
    }
}