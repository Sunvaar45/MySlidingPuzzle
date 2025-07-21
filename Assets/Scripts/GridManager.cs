using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public GameObject tilePrefab;
    public int gridSize = 4;
    public float tileSpacing = 1.1f;
    public Camera mainCamera;

    private GameObject[,] grid;
    private Vector3 emptyTilePosition;
    private float centerOffset;
    private int emptyX, emptyY;

    void Start()
    {
        centerOffset = (gridSize * tileSpacing - tileSpacing) / 2;
        grid = new GameObject[gridSize, gridSize];
        GenerateGrid();
        ResizeCamera();
    }

    void GenerateGrid()
    {
        // choose a random empty tile position
        emptyX = Random.Range(0, gridSize);
        emptyY = Random.Range(0, gridSize);
        emptyTilePosition = new Vector3(emptyX, 0, emptyY); // use this later for win-con

        // iterate trough every tile space in grid
        for (int x = 0; x < gridSize; x++)
        {
            for (int y = 0; y < gridSize; y++)
            {
                // skip the empty tile
                if (x == emptyX && y == emptyY)
                {
                    emptyTilePosition = new Vector3(
                        emptyX * tileSpacing - centerOffset,
                        0,
                        emptyY * tileSpacing - centerOffset
                    );

                    grid[x, y] = null;
                    continue;
                }

                // calculate position for tile
                Vector3 gridPos = new Vector3(
                    x * tileSpacing - centerOffset,
                    0,
                    y * tileSpacing - centerOffset
                );

                // place the tile
                GameObject tile = Instantiate(tilePrefab, gridPos, Quaternion.identity, transform);

                // attach the tile to array
                grid[x, y] = tile;

                // assign position data to each tile
                Tile tileScript = tile.GetComponent<Tile>();
                tileScript.Init(x, y, this);
                // tileScript.Rename(x, y);
            }
        }
    }

    public void TryMoveTile(int x, int y)
    {
        GameObject tile = grid[x, y];
        if (tile == null) return;

        // move the tile to empty pos
        tile.transform.position = emptyTilePosition;

        // update the tiles flag and coordinates (name stays with original coordinates)
        Tile tileScript = tile.GetComponent<Tile>();
        tileScript.UpdatePosition(emptyX, emptyY);

        // add the new tile to the grid
        grid[emptyX, emptyY] = tile;
        grid[x, y] = null;

        // update empty pos
        emptyTilePosition = new Vector3(
            x * tileSpacing - centerOffset,
            0,
            y * tileSpacing - centerOffset
        );
        emptyX = x;
        emptyY = y;
    }

    public bool TileIsAdjacentToEmpty(int x, int y)
    {
        // find distance between empty and clicked positions in x and y coordinates
        int deltaX = Mathf.Abs(x - emptyX);
        int deltaY = Mathf.Abs(y - emptyY);

        // adjacent = 1 in one direction and 0 in other direction
        if ((deltaX == 1 && deltaY == 0) || (deltaX == 0 && deltaY == 1))
        {
            return true;
        }
        return false;
    }

    void ResizeCamera()
    {
        float gridHeight = gridSize * tileSpacing;
        mainCamera.orthographicSize = gridHeight / 1.5f;
    }
}
