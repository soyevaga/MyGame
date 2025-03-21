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
    private Map[] maps;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            maps = new Map[]
            {
                new Map(new int[,]
                        {
                            { 0, 0, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                            { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1 },
                            { 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                            { 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0 },
                            { 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0 },
                            { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                            { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                            { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                            { 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }
                        },
                        new int[]
                        {
                            0, 0, 0, 0, //general
                            0, 0, 1, 1, //desert
                            0, 0, 0, 0, //woods
                            0, 0, 0, 0, //island
                            0, 0, 0, 0  //volcano
                        }
                ),
                new Map(new int[,]
                        {
                            { 0, 0, 1, 0, 0, 0, 0, 0, 0, 1, 1, 0 },
                            { 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0 },
                            { 0, 1, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0 },
                            { 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 1, 0 },
                            { 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                            { 1, 1, 0, 0, 0, 1, 1, 1, 0, 1, 0, 0 },
                            { 0, 2, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0 },
                            { 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0 },
                            { 1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0 }
                        },
                        new int[]
                        {
                            0, 1, 1, 0, //general
                            1, 0, 0, 1, //desert
                            0, 0, 0, 0, //woods
                            0, 0, 0, 0, //island
                            0, 0, 0, 0  //volcano
                        }
                )
            };

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
        int level = TilesGameManager.Instance.GetCurrentLevel();
        int[,] matrix = maps[level].GetMatrix();
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Tile selectedTile = null;
                if (matrix[y,x] == 1 || matrix[y, x] ==0)
                {
                    selectedTile = mainTiles[matrix[y, x]];
                }
                else
                {
                    selectedTile= goalTiles[matrix[y, x] -2];
                }
                Vector3Int cellPosition = new Vector3Int(x, height-1-y, 0);  
                tilemap.SetTile(cellPosition, selectedTile);  
            }
        }
    }
    public bool HasTile(Vector3 pos, int index)
    {
        pos.y -= cellSize / 2;
        Vector3Int cellPosition = tilemap.WorldToCell(pos);
        TileBase tileBase = tilemap.GetTile(cellPosition);

        if (tileBase != null)
        {
            return true;
        }
        return false;
    }
    public bool HasValidTile(Vector3 pos, int index)
    {
        pos.y -= cellSize / 2;
        Vector3Int cellPosition = tilemap.WorldToCell(pos);
        TileBase tileBase = tilemap.GetTile(cellPosition);

        if (tileBase != null)
        {
            Tile tile = tilemap.GetTile<Tile>(cellPosition);
            if (tile == mainTiles[0]) return true;
            if (goalTiles.Contains(tile))
            {
                int tileIndex = goalTiles.IndexOf(tile);
                if (tileIndex == index - 1) return true;
            }
            if(moveTiles.Contains(tile)) return true;
        }
        return false;
    }

    public int ChangeTile(Vector3 pos, int tileIndex)
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
                return 1;
            }
            else if (tile != mainTiles[1] && !goalTiles.Contains(tile))
            {
                if (TilesGameManager.Instance.GetButtonNumber(tileIndex) > 0)
                {
                    if (moveTiles.Contains(tile))
                    {
                        TilesGameManager.Instance.ChangeButtonNumber(moveTiles.IndexOf(tile), 1);
                    }
                    tilemap.SetTile(cellPosition, moveTiles[tileIndex]);
                    return -1;
                }
            }
        }
        return 0;
    }

    public bool TileIsMyMoveType(Vector3 pos, int index)
    {
        pos.y -= cellSize / 2;
        Vector3Int cellPosition = tilemap.WorldToCell(pos);
        TileBase tileBase = tilemap.GetTile(cellPosition);
        if (tileBase != null)
        {
            Tile tile = tilemap.GetTile<Tile>(cellPosition);
            if (moveTiles.Contains(tile))
            {
                int tileIndex = moveTiles.IndexOf(tile);
                if (tileIndex / 4 == 0 || tileIndex / 4 == index) return true;
            }            
        }
        return false;
    }
    public bool TileIsMyGoal(Vector3 pos, int index)
    {
        pos.y -= cellSize / 2;
        Vector3Int cellPosition = tilemap.WorldToCell(pos);
        TileBase tileBase = tilemap.GetTile(cellPosition);
        if (tileBase != null)
        {
            Tile tile = tilemap.GetTile<Tile>(cellPosition);
            if (goalTiles.Contains(tile))
            {
                int tileIndex = goalTiles.IndexOf(tile);
                if (tileIndex == index - 1) return true;
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
    public Map[] GetMaps()
    {
        return maps;
    }
}

