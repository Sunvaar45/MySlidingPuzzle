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
        float centerOffset = (gridSize * tileSpacing - tileSpacing) / 2;

        for (int x = 1; x <= gridSize; x++)
        {
            for (int y = 1; y <= gridSize; y++)
            {
                Vector3 gridPos = new Vector3(
                    (x - 1) * tileSpacing - centerOffset,
                    0,
                    (y - 1) * tileSpacing - centerOffset
                );

                GameObject tile = Instantiate(tilePrefab, gridPos, Quaternion.identity, transform);
                tile.name = "Tile_" + x + "_" + y;
            }
        }
    }

    void ResizeCamera()
    {
        float gridHeight = gridSize * tileSpacing;
        mainCamera.orthographicSize = gridHeight / 1.5f;
    }
}
