using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public GameObject tilePrefab;
    public int gridSize = 4;
    public float tileSpacing = 1.1f;
    public Camera mainCamera;

    private GameObject[,] grid;
    private Vector2 emptyTilePosition;

    void Start()
    {
        grid = new GameObject[gridSize - 1, gridSize - 1];
        GenerateGrid();
        ResizeCamera();
    }

    void GenerateGrid()
    {
        float centerOffset = (gridSize * tileSpacing - tileSpacing) / 2;

        // choose a random empty tile position
        int emptyX = Random.Range(1, gridSize + 1);
        int emptyY = Random.Range(1, gridSize + 1);
        emptyTilePosition = new Vector2(emptyX, emptyY); // use this later for win-con

        // iterate trough every tile space in grid
        for (int x = 1; x <= gridSize; x++)
        {
            for (int y = 1; y <= gridSize; y++)
            {
                // skip the empty tile
                if (x == emptyX && y == emptyY) continue;

                // calculate position for tile
                Vector3 gridPos = new Vector3(
                    (x - 1) * tileSpacing - centerOffset,
                    0,
                    (y - 1) * tileSpacing - centerOffset
                );

                // place the tile
                GameObject tile = Instantiate(tilePrefab, gridPos, Quaternion.identity, transform);

                // attach the tile to array
                grid[x - 1, y - 1] = tile;

                // rename tile
                tile.name = "Tile_" + x.ToString() + "_" + y.ToString();

                // assign position data to each tile
                Tile tileScript = tile.GetComponent<Tile>();
                tileScript.Init(x - 1, y - 1, this);
            }
        }
    }

    public void TryMoveTile(int x, int y)
    {
        // do this monday
    }

    void ResizeCamera()
    {
        float gridHeight = gridSize * tileSpacing;
        mainCamera.orthographicSize = gridHeight / 1.5f;
    }
}
