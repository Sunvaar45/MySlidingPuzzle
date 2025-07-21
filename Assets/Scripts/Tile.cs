using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public int x;
    public int y;

    private GridManager gridManager;
    private bool hasBeenClicked;

    public void Init(int x, int y, GridManager gridManager)
    {
        this.x = x;
        this.y = y;
        this.gridManager = gridManager;
        hasBeenClicked = false;
    }

    private void OnMouseUp()
    {
        if (hasBeenClicked == true || gridManager == null) return;

        hasBeenClicked = true;
        gridManager.TryMoveTile(x, y);
    }
}
