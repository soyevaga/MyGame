using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GridManager : MonoBehaviour
{
    public static GridManager Instance { get; private set; }

    [SerializeField] private Tilemap tilemap;
    [SerializeField] private Tile[] mainTiles;
    [SerializeField] private List<Tile> moveTiles;
    [SerializeField] private List<Tile> goalTiles;
    [SerializeField] private int width = 12;
    [SerializeField] private int height = 9;
    [SerializeField] private float cellSize;
    private Vector3[] directions = { new Vector3(0, -25.6f, 0),  new Vector3(-25.6f, 0, 0), new Vector3(25.6f, 0, 0), new Vector3(0, 25.6f, 0)};
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
    public void GenerateGrid(int maxGoals)
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Tile selectedTile = mainTiles[0];  
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

    public bool HasValidTile(Vector3 pos, int index)
    {
        pos.y -= cellSize / 2;
        Vector3Int cellPosition = tilemap.WorldToCell(pos);
        TileBase tileBase = tilemap.GetTile(cellPosition);

        if (tileBase != null)
        {
            Tile tile = tilemap.GetTile<Tile>(cellPosition);
            if (tile == mainTiles[1]) return true;
            if (goalTiles.Contains(tile))
            {
                int tileIndex = goalTiles.IndexOf(tile);
                if (tileIndex != index - 1) return false;
            }
        }
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
            else if (tile != mainTiles[1] && !goalTiles.Contains(tile))
            {
                tilemap.SetTile(cellPosition, moveTiles[tileIndex]);
            }
        }
    }

    public bool TileIsMyType(Vector3 pos, int index)
    {
        pos.y -= cellSize / 2;
        Vector3Int cellPosition = tilemap.WorldToCell(pos);
        TileBase tileBase = tilemap.GetTile(cellPosition);
        if (tileBase != null)
        {
            Tile tile = tilemap.GetTile<Tile>(cellPosition);
            if(tile == mainTiles[0] || tile == mainTiles[1]) return false;
            if (moveTiles.Contains(tile))
            {
                int tileIndex = moveTiles.IndexOf(tile);
                if (tileIndex / 4 == 0 || tileIndex / 4 == index) return true;
            }            
        }
        return false;
    }

    public Vector3 GetTileDirection(Vector3 pos)
    {
        pos.y -= cellSize / 2;
        Vector3Int cellPosition = tilemap.WorldToCell(pos);
        TileBase tileBase = tilemap.GetTile(cellPosition);

        if (tileBase != null)
        {
            Tile tile = tilemap.GetTile<Tile>(cellPosition);
            if (moveTiles.Contains(tile))
            {
                int dirIndex = moveTiles.IndexOf(tile)%4;
                return directions[dirIndex];
            }
        }
        return new Vector3(0,0,0);
    }
}

