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
    public bool frozen;
    public bool isMoving;
    public float moveSpeed = 5f;


    private GridManager gridManager;

    public void Init(int x, int y, GridManager gridManager)
    {
        this.x = x;
        this.originalX = x;

        this.y = y;
        this.originalY = y;

        this.gridManager = gridManager;

        int tileNumber = x * gridManager.gridSize + y + 1;
        tmp.text = tileNumber.ToString();

        Rename(originalX, originalY);
        frozen = false;
    }

    public void UpdatePosition(int newX, int newY)
    {
        this.x = newX;
        this.y = newY;
    }

    private void Rename(int x, int y)
    {
        this.name = "Tile_" + x.ToString() + "_" + y.ToString();
    }

    private void OnMouseUp()
    {       
        if (isMoving == true || gridManager == null)
        {
            return;
        }

        if (gridManager.TileIsAdjacentToEmpty(x, y) == false)
        {
            return;
        }

        gridManager.TryMoveTile(x, y);
    }

    public void MoveTo(Vector3 targetPos)
    {
        if (!isMoving && !frozen)
            StartCoroutine(MoveTile(targetPos));
    }

    private IEnumerator MoveTile(Vector3 targetPos)
    {
        isMoving = true;

        while (Vector3.Distance(transform.position, targetPos) > .01f)
        {
            transform.position = Vector3.MoveTowards(
                transform.position,
                targetPos,
                moveSpeed * Time.deltaTime
            );
            yield return null;
        }

        // snap at exact position at end
        transform.position = targetPos;

        isMoving = false;
    }
}