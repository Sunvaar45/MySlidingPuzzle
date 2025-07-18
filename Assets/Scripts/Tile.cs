using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public int x;
    public int y;
    private GridManager gridManager;

    public void Init(int x, int y, GridManager gridManager)
    {
        this.x = x;
        this.y = y;
        this.gridManager = gridManager;
    }

    private void OnMouseUp()
    {
        gridManager.TryMoveTile(x, y);
    }
}
