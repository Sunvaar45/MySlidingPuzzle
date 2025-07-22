using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public GameManager gameManager;
    public UIManager uiManager;
    public GameObject tilePrefab;
    public int gridSize = 4;
    public float tileSpacing = 1.1f;
    public Camera mainCamera;
    public int shuffleAmount = 100;
    public int emptyX, emptyY;


    // [HideInInspector]
    public int moveCounter = 0;

    private GameObject[,] grid;
    private Vector3 emptyTilePosition;
    private float centerOffset;

    void Start()
    {
        centerOffset = (gridSize * tileSpacing - tileSpacing) / 2;
        grid = new GameObject[gridSize, gridSize];
        GenerateGrid();
        ResizeCamera();
        ShuffleGrid();
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
            }
        }
    }

    public void TryMoveTile(int x, int y)
    {
        GameObject tile = grid[x, y];
        if (tile == null) return;

        // get the tile's script
        Tile tileScript = tile.GetComponent<Tile>();

        // move the tile to empty pos
        if (ShuffleStage())
        {
            tile.transform.position = emptyTilePosition;
        }   
        else
        {
            tileScript.MoveTo(emptyTilePosition);
        }
        // tile.transform.position = emptyTilePosition;

        // update the tiles flag and coordinates (name stays with original coordinates)
        tileScript.UpdatePosition(emptyX, emptyY);

        // add the new tile to the grid
        grid[emptyX, emptyY] = tile;
        grid[x, y] = null;

        // update empty pos
        emptyX = x;
        emptyY = y;
        emptyTilePosition = new Vector3(
            emptyX * tileSpacing - centerOffset,
            0,
            emptyY * tileSpacing - centerOffset
        );


        // increment moveCounter
        moveCounter++;
        uiManager.UpdateMoveCounterUI();

        // check for wincon
        if (!ShuffleStage() && CheckWinCon())
        {
            gameManager.WinGame();
        }
    }

    private bool ShuffleStage()
    {
        if (moveCounter < shuffleAmount)
        {
            return true;
        }

        return false;
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

    public void FreezeGrid()
    {
        foreach (var tile in grid)
        {
            if (tile != null)
            {
                Tile tileScript = tile.GetComponent<Tile>();
                tileScript.hasBeenClicked = true;
            }
        }
    }

    private bool CheckWinCon()
    {
        for (int x = 0; x < gridSize; x++)
        {
            for (int y = 0; y < gridSize; y++)
            {
                GameObject tile = grid[x, y];
                if (tile == null) continue;

                Tile tileScript = tile.GetComponent<Tile>();
                if (tileScript.originalX != x || tileScript.originalY != y)
                {
                    return false;
                }
            }
        }

        return true;
    }

    private void ShuffleGrid() // TUESDAY - don't include reverse moves in shuffles
    {
        Vector2Int lastMoveDir = Vector2Int.zero;
        int i = 0;

        while (i < shuffleAmount)
        {
            // get tiles to a list adjacent to empty tile
            List<Vector2Int> adjacentTilePositions = GetAdjacentTilePositions();
            if (adjacentTilePositions.Count == 0) break;

            // filter out the reverse direction
            List<Vector2Int> validChoices = new List<Vector2Int>();
            foreach (Vector2Int pos in adjacentTilePositions)
            {
                Vector2Int moveDir = new Vector2Int(pos.x - emptyX, pos.y - emptyY);
                if (moveDir != -lastMoveDir)
                {
                    validChoices.Add(pos);
                }
            }
            if (validChoices.Count == 0) continue;

            // randomly choose a direction
            Vector2Int chosenPos = validChoices[Random.Range(0, validChoices.Count)];

            // store last move for disallowing reverse move next shuffle
            Vector2Int chosenDir = new Vector2Int(chosenPos.x - emptyX, chosenPos.y - emptyY);
            lastMoveDir = chosenDir;

            // move the piece
            TryMoveTile(chosenPos.x, chosenPos.y);
            i++;
        }
    }

    private List<Vector2Int> GetAdjacentTilePositions()
    {
        List<Vector2Int> positions = new List<Vector2Int>();

        Vector2Int[] directions = {
            new Vector2Int(0, 1), // up
            new Vector2Int(0, -1), // down
            new Vector2Int(-1, 0), // left
            new Vector2Int(1, 0) // right
        };

        // add the adjacent tiles to positions list and return it
        foreach (Vector2Int dir in directions)
        {
            int checkX = emptyX + dir.x;
            int checkY = emptyY + dir.y;

            if (checkX >= 0 && checkX < gridSize && checkY >= 0 && checkY < gridSize) // check if its in grid
            {
                if (grid[checkX, checkY] != null)
                {
                    positions.Add(new Vector2Int(checkX, checkY));
                }
            }
        }

        return positions;
    }

    private void ResizeCamera()
    {
        float gridHeight = gridSize * tileSpacing;
        mainCamera.orthographicSize = gridHeight / 1.5f;
    }
}