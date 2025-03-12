using System;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GridManager : MonoBehaviour
{
    public static GridManager Instance { get; private set; }

    [SerializeField] private Tilemap tilemap;
    [SerializeField] private Tile[] tiles;
    [SerializeField] private int width = 12;
    [SerializeField] private int height = 9;
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

    public int GetWidth()
    {
        return width;
    }

    public int GetHeight()
    {
        return height;
    }
    public void GenerateGrid()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Tile selectedTile = tiles[UnityEngine.Random.Range(5,10)];  
                Vector3Int cellPosition = new Vector3Int(x, y, 0);  
                tilemap.SetTile(cellPosition, selectedTile);  
            }
        }
    }

    public bool HasTile(Vector3 pos)
    {
        return true;
    }
}

