using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Tile : MonoBehaviour
{
    public int x;
    public int y;
    public TextMeshPro tmp;
    public int originalX, originalY;

    private GridManager gridManager;
    private bool hasBeenClicked;


    public void Init(int x, int y, GridManager gridManager)
    {
        this.x = x;
        this.originalX = x;

        this.y = y;
        this.originalY = y;

        this.gridManager = gridManager;

        int tileNumber = x * gridManager.gridSize + y + 1;
        tmp.text = tileNumber.ToString();

        Rename(x, y);
        hasBeenClicked = false;
    }

    public void UpdatePosition(int newX, int newY)
    {
        this.x = newX;
        this.y = newY;
        
        Rename(newX, newY);
        hasBeenClicked = false;
    }

    private void Rename(int x, int y)
    {
        this.name = "Tile_" + x.ToString() + "_" + y.ToString();
    }

    private void OnMouseUp()
    {
        if (hasBeenClicked == true || gridManager == null) return;

        if (gridManager.TileIsAdjacentToEmpty(x, y) == false) return;

        hasBeenClicked = true;
        gridManager.TryMoveTile(x, y);
    }
}