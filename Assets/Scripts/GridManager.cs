using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public GameObject tilePrefab;
    public int gridSize = 4;
    public float tileSpacing = 1.1f;
    public Camera mainCamera;

    void Start()
    {
        GenerateGrid();
        ResizeCamera();
    }

    void GenerateGrid()
    {
        float centerOffset = (gridSize - 1) * (tileSpacing / 2);

        for (int x = 0; x < gridSize; x++)
        {
            for (int y = 0; y < gridSize; y++)
            {
                Vector3 gridPos = new Vector3(
                    x * tileSpacing - centerOffset,
                    0,
                    y * tileSpacing - centerOffset
                );

                GameObject tile = Instantiate(tilePrefab, gridPos, Quaternion.identity, transform);
                tile.name = "Tile_" + (x + 1) + "_" + (y + 1);
            }
        }
    }

    void ResizeCamera()
    {
        float gridHeight = gridSize * tileSpacing;
        mainCamera.orthographicSize = gridHeight / 1.5f;
    }
}
