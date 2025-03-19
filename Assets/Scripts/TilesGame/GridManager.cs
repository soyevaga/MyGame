using System;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GridManager : MonoBehaviour
{
    public static GridManager Instance { get; private set; }

    [SerializeField] private Tilemap tilemap;
    [SerializeField] private Tile[] mainTiles;
    [SerializeField] private Tile[] moveTiles;
    [SerializeField] private Tile[] goalTiles;
    [SerializeField] private int width = 12;
    [SerializeField] private int height = 9;
    [SerializeField] private float cellSize;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void Update()
    {
        
    }
    public int GetWidth()
    {
        return width;
    }

    public int GetHeight()
    {
        return height;
    }
    public void GenerateGrid(int maxGoals)
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Tile selectedTile = mainTiles[UnityEngine.Random.Range(0,2)];  
                Vector3Int cellPosition = new Vector3Int(x, y, 0);  
                tilemap.SetTile(cellPosition, selectedTile);  
            }
        }
        while (maxGoals > 0)
        {
            Vector3Int cellPosition = new Vector3Int(UnityEngine.Random.Range(1, width), UnityEngine.Random.Range(1, height), 0);
            tilemap.SetTile(cellPosition, goalTiles[maxGoals-1]);
            maxGoals--;
        }
    }

    public bool HasTile(Vector3 pos)
    {
        return true;
    }

    public void ChangeTile(Vector3 pos, int tileIndex)
    {
        pos.y -= cellSize/2;
        Vector3Int cellPosition = tilemap.WorldToCell(pos);
        TileBase clickedTile = tilemap.GetTile(cellPosition);

        if (clickedTile != null)
        {
            Tile tile = tilemap.GetTile<Tile>(cellPosition);
            if (tile == moveTiles[tileIndex])
            {
                tilemap.SetTile(cellPosition, mainTiles[0]);
            }
            else if (tile != mainTiles[1] && tile != goalTiles[0] && tile != goalTiles[1] && tile != goalTiles[2] && tile != goalTiles[3])
            {
                tilemap.SetTile(cellPosition, moveTiles[tileIndex]);
            }
        }
    }
}

