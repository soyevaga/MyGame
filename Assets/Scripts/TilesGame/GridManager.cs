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
    [SerializeField] private GameObject[] goals;
    [SerializeField] private GameObject[] icons;
    [SerializeField] private int width = 12;
    [SerializeField] private int height = 8;
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
                new Map(new int[,] //LEVEL 1
                        {
                            { 0, 0, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                            { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1 },
                            { 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                            { 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0 },
                            { 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0 },
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
                new Map(new int[,] //LEVEL 1
                    {
                        { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                        { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                        { 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0 },
                        { 0, 0, 0, 0, 0, 1, 2, 0, 0, 0, 0, 0 },
                        { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                        { 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0 },
                        { 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0 },
                        { 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0 }
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

                new Map(new int[,] //LEVEL 2
                        {
                            { 0, 0, 1, 0, 0, 0, 0, 0, 0, 1, 1, 0 },
                            { 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 0, 0 },
                            { 0, 1, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0 },
                            { 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 1, 0 },
                            { 1, 1, 0, 0, 0, 1, 1, 1, 0, 0, 0, 0 },
                            { 0, 2, 1, 0, 0, 1, 0, 0, 0, 0, 0, 0 },
                            { 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0 },
                            { 1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1, 0 }
                        },
                        new int[]
                        {
                            0, 1, 1, 0, //general
                            1, 0, 0, 1, //desert
                            0, 0, 0, 0, //woods
                            0, 0, 0, 0, //island
                            0, 0, 0, 0  //volcano
                        }
                ), 
                new Map(new int[,] //LEVEL 2
                        {
                            { 0, 1, 0, 1, 1, 1, 0, 0, 0, 0, 0, 0 },
                            { 0, 1, 0, 0, 0, 1, 2, 1, 0, 0, 0, 1 },
                            { 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1 },
                            { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                            { 1, 0, 0, 0, 0, 1, 0, 1, 0, 1, 1, 0 },
                            { 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                            { 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0 },
                            { 0, 0, 1, 0, 0, 1, 0, 0, 0, 0, 0, 0 }
                        },
                        new int[]
                        {
                            0, 1, 1, 0, //general
                            1, 0, 0, 1, //desert
                            0, 0, 0, 0, //woods
                            0, 0, 0, 0, //island
                            0, 0, 0, 0  //volcano
                        }
                ),

                new Map(new int[,] //LEVEL 3
                        {
                            { 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1 },
                            { 1, 1, 0, 1, 0, 0, 0, 1, 1, 1, 0, 1 },
                            { 1, 1, 0, 0, 0, 1, 0, 1, 0, 1, 1, 0 },
                            { 0, 1, 0, 0, 1, 0, 0, 1, 1, 0, 0, 1 },
                            { 1, 0, 0, 0, 0, 0, 0, 1, 0, 1, 1, 1 },
                            { 1, 1, 0, 1, 1, 1, 0, 0, 0, 1, 0, 1 },
                            { 0, 1, 1, 0, 0, 1, 1, 0, 0, 2, 1, 1 },
                            { 1, 1, 1, 0, 1, 1, 1, 0, 0, 1, 0, 1 }
                        },
                        new int[]
                        {
                            2, 0, 2, 0, //general
                            1, 0, 2, 0, //desert
                            0, 0, 0, 0, //woods
                            0, 0, 0, 0, //island
                            0, 0, 0, 0  //volcano
                        }
                ),
                new Map(new int[,] //LEVEL 3
                        {
                            { 0, 0, 0, 0, 0, 1, 0, 0, 1, 0, 1, 1 },
                            { 1, 0, 0, 0, 0, 1, 1, 0, 0, 0, 0, 1 },
                            { 1, 1, 1, 0, 0, 0, 0, 1, 1, 1, 1, 0 },
                            { 0, 1, 0, 1, 0, 0, 0, 1, 1, 1, 0, 0 },
                            { 0, 0, 0, 1, 1, 1, 0, 0, 0, 1, 1, 1 },
                            { 0, 0, 1, 0, 1, 0, 0, 1, 0, 2, 1, 1 },
                            { 1, 0, 1, 1, 1, 0, 1, 1, 1, 1, 0, 0 },
                            { 0, 1, 1, 0, 0, 0, 0, 0, 1, 1, 1, 1 }
                        },
                        new int[]
                        {
                            2, 0, 2, 0, //general
                            1, 0, 2, 0, //desert
                            0, 0, 0, 0, //woods
                            0, 0, 0, 0, //island
                            0, 0, 0, 0  //volcano
                        }
                ),

                new Map(new int[,] //LEVEL 4
                        {
                            { 0, 0, 0, 0, 0, 0, 0, 2, 0, 0, 1, 0 },
                            { 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0 },
                            { 0, 0, 0, 1, 1, 0, 0, 1, 0, 0, 0, 1 },
                            { 0, 0, 0, 0, 0, 0, 0, 1, 0, 1, 1, 1 },
                            { 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0 },
                            { 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0 },
                            { 0, 1, 0, 0, 0, 1, 1, 0, 1, 0, 0, 0 },
                            { 1, 0, 0, 0, 0, 0, 0, 1, 1, 1, 0, 1 }
                        },
                        new int[]
                        {
                            0, 0, 0, 0, //general
                            0, 0, 1, 0, //desert
                            0, 0, 1, 0, //woods
                            0, 0, 0, 0, //island
                            0, 0, 0, 0  //volcano
                        }
                ),
                new Map(new int[,] //LEVEL 4
                        {
                            { 0, 0, 0, 1, 1, 0, 0, 0, 0, 0, 0, 0 },
                            { 0, 1, 0, 0, 0, 0, 1, 0, 0, 1, 1, 0 },
                            { 0, 1, 0, 0, 0, 0, 1, 1, 0, 0, 1, 0 },
                            { 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0 },
                            { 0, 0, 0, 0, 0, 0, 0, 0, 2, 0, 0, 0 },
                            { 1, 0, 0, 0, 0, 1, 1, 1, 0, 0, 0, 0 },
                            { 0, 0, 1, 0, 0, 0, 1, 0, 0, 0, 0, 0 },
                            { 0, 0, 0, 0, 0, 1, 1, 0, 0, 1, 1, 0 }
                        },
                        new int[]
                        {
                            0, 0, 0, 0, //general
                            0, 0, 1, 0, //desert
                            0, 0, 1, 0, //woods
                            0, 0, 0, 0, //island
                            0, 0, 0, 0  //volcano
                        }
                ),

                new Map(new int[,] //LEVEL 5
                        {
                            { 0, 0, 0, 0, 0, 1, 1, 1, 0, 0, 0, 0 },
                            { 0, 1, 0, 1, 0, 0, 1, 1, 1, 1, 1, 1 },
                            { 0, 0, 1, 1, 1, 0, 0, 0, 3, 0, 1, 0 },
                            { 0, 1, 0, 0, 0, 0, 1, 1, 0, 1, 1, 1 },
                            { 0, 0, 1, 1, 1, 0, 0, 0, 0, 1, 1, 1 },
                            { 1, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0 },
                            { 1, 2, 0, 1, 1, 0, 0, 0, 0, 1, 1, 0 },
                            { 1, 0, 0, 1, 1, 0, 0, 0, 1, 1, 0, 1 }
                        },
                        new int[]
                        {
                            1, 0, 1, 0, //general
                            0, 0, 0, 0, //desert
                            0, 0, 2, 1, //woods
                            0, 0, 0, 0, //island
                            0, 0, 0, 0  //volcano
                        }
                ),
                new Map(new int[,] //LEVEL 5
                        {
                            { 0, 1, 0, 0, 1, 0, 0, 1, 0, 0, 0, 1 },
                            { 0, 0, 0, 1, 0, 0, 0, 3, 0, 1, 0, 0 },
                            { 1, 1, 0, 0, 1, 0, 1, 0, 1, 1, 1, 1 },
                            { 1, 0, 0, 0, 0, 0, 1, 1, 1, 0, 1, 0 },
                            { 0, 0, 0, 0, 1, 0, 1, 1, 0, 0, 1, 0 },
                            { 1, 1, 2, 0, 0, 1, 0, 1, 1, 1, 1, 0 },
                            { 1, 1, 0, 0, 1, 1, 1, 1, 1, 1, 1, 0 },
                            { 0, 1, 0, 1, 0, 1, 1, 1, 0, 1, 0, 0 }
                        },
                        new int[]
                        {
                            1, 0, 1, 0, //general
                            0, 0, 0, 0, //desert
                            0, 0, 2, 1, //woods
                            0, 0, 0, 0, //island
                            0, 0, 0, 0  //volcano
                        }
                ),

                new Map(new int[,] //LEVEL 6
                        {
                            { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1 },
                            { 0, 0, 0, 1, 1, 0, 1, 0, 0, 0, 1, 0 },
                            { 0, 0, 1, 0, 1, 0, 0, 0, 0, 0, 0, 0 },
                            { 0, 0, 1, 1, 0, 1, 0, 0, 0, 0, 1, 4 },
                            { 1, 0, 0, 0, 1, 0, 0, 1, 0, 0, 0, 0 },
                            { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                            { 0, 0, 0, 0, 0, 1, 1, 3, 0, 0, 1, 0 },
                            { 0, 0, 0, 1, 0, 1, 0, 1, 0, 2, 0, 0 }
                        },
                        new int[]
                        {
                            1, 0, 1, 0, //general
                            0, 0, 1, 0, //desert
                            0, 1, 0, 0, //woods
                            1, 0, 1, 0, //island
                            0, 0, 0, 0  //volcano
                        }

                ),
                new Map(new int[,] //LEVEL 6
                        {
                            { 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 1 },
                            { 0, 0, 1, 1, 0, 0, 0, 0, 1, 0, 1, 0 },
                            { 0, 0, 0, 0, 0, 0, 1, 0, 0, 1, 1, 0 },
                            { 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0 },
                            { 0, 1, 0, 0, 0, 0, 0, 0, 2, 0, 0, 0 },
                            { 1, 0, 0, 1, 0, 1, 1, 0, 0, 0, 0, 0 },
                            { 1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0 },
                            { 0, 1, 0, 3, 0, 0, 0, 0, 0, 0, 1, 4 }
                        },
                        new int[]
                        {
                            1, 0, 1, 0, //general
                            0, 0, 1, 0, //desert
                            0, 1, 0, 0, //woods
                            1, 0, 1, 0, //island
                            0, 0, 0, 0  //volcano
                        }
                ),

                new Map(new int[,] //LEVEL 7
                        {
                            { 0, 1, 0, 1, 1, 1, 0, 0, 0, 0, 0, 4 },
                            { 0, 0, 0, 1, 1, 0, 0, 0, 0, 0, 1, 1 },
                            { 0, 0, 0, 0, 1, 0, 1, 0, 1, 0, 0, 0 },
                            { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0 },
                            { 1, 0, 0, 1, 0, 0, 0, 5, 0, 0, 1, 0 },
                            { 1, 3, 1, 0, 1, 0, 0, 1, 1, 0, 1, 1 },
                            { 0, 0, 0, 0, 0, 0, 0, 1, 1, 0, 1, 1 },
                            { 0, 2, 0, 1, 0, 0, 0, 0, 0, 1, 1, 1 }
                        },
                        new int[]
                        {
                            1, 0, 1, 0, //general
                            1, 1, 0, 0, //desert
                            1, 0, 0, 0, //woods
                            0, 0, 1, 1, //island
                            1, 0, 0, 0  //volcano
                        }
                ), 
                new Map(new int[,] //LEVEL 7
                        {
                            { 0, 0, 1, 0, 1, 0, 1, 0, 0, 0, 0, 0 },
                            { 0, 0, 1, 0, 1, 0, 0, 1, 1, 1, 1, 0 },
                            { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0 },
                            { 1, 0, 0, 1, 0, 0, 5, 0, 0, 0, 0, 1 },
                            { 0, 0, 1, 2, 0, 0, 0, 0, 3, 0, 0, 0 },
                            { 0, 1, 1, 1, 0, 1, 0, 0, 1, 0, 1, 4 },
                            { 0, 0, 0, 1, 0, 0, 0, 1, 1, 0, 0, 0 },
                            { 1, 1, 1, 1, 0, 0, 1, 0, 0, 1, 0, 0 }
                        },
                        new int[]
                        {
                            1, 0, 1, 0, //general
                            1, 1, 0, 0, //desert
                            1, 0, 0, 0, //woods
                            0, 0, 1, 1, //island
                            1, 0, 0, 0  //volcano
                        }
                ),

                new Map(new int[,] //LEVEL 8
                        {
                            { 0, 1, 1, 0, 1, 0, 0, 0, 0, 0, 1, 1 },
                            { 0, 0, 0, 0, 1, 3, 1, 0, 0, 0, 1, 0 },
                            { 1, 1, 1, 0, 0, 0, 1, 0, 0, 1, 0, 1 },
                            { 0, 2, 1, 0, 0, 0, 0, 0, 0, 1, 0, 0 },
                            { 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1 },
                            { 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 1, 0 },
                            { 1, 0, 1, 1, 0, 1, 1, 0, 0, 0, 4, 1 },
                            { 0, 1, 0, 1, 5, 1, 0, 0, 0, 1, 1, 0 }
                        },
                        new int[]
                        {
                            2, 0, 3, 0, //general
                            0, 1, 0, 1, //desert
                            0, 0, 0, 1, //woods
                            0, 0, 0, 0, //island
                            1, 0, 0, 0  //volcano
                        }
                ),
                new Map(new int[,] //LEVEL 8
                        {
                            { 0, 0, 0, 0, 1, 0, 0, 3, 0, 1, 0, 1 },
                            { 1, 1, 0, 0, 0, 0, 0, 0, 1, 0, 1, 1 },
                            { 1, 1, 1, 0, 0, 1, 1, 0, 0, 1, 1, 1 },
                            { 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1 },
                            { 0, 1, 1, 0, 0, 0, 2, 1, 0, 1, 0, 1 },
                            { 1, 0, 0, 1, 5, 1, 0, 0, 0, 0, 0, 0 },
                            { 1, 1, 1, 1, 0, 1, 1, 0, 0, 4, 1, 0 },
                            { 0, 1, 1, 1, 0, 0, 1, 1, 0, 0, 0, 0 }
                        },
                        new int[]
                        {
                            2, 0, 3, 0, //general
                            0, 1, 0, 1, //desert
                            0, 0, 0, 1, //woods
                            0, 0, 0, 0, //island
                            1, 0, 0, 0  //volcano
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
        HideObjects();  
        int level = TilesGameManager.Instance.GetCurrentLevel();
        int[,] matrix = maps[level].GetMatrix();
        for (int x = 0; x < width; x++)
        {
            for (int y = 1; y < height; y++)
            {
                Tile selectedTile = null;
                if (matrix[y-1,x] == 1 || matrix[y-1, x] ==0)
                {
                    selectedTile = mainTiles[matrix[y - 1, x]];
                }
                else
                {
                    selectedTile= goalTiles[matrix[y-1, x] -2];
                    goals[matrix[y - 1, x] - 2].SetActive(true);
                }
                Vector3Int cellPosition = new Vector3Int(x, height-1-y, 0);  
                tilemap.SetTile(cellPosition, selectedTile);  
            }
        }
        Vector3Int iniPosition = new Vector3Int(0, height-1, 0);
        tilemap.SetTile(iniPosition, mainTiles[2]);
        int[] arrows = maps[level].GetArrows();
        bool arrowFound = false;
        for(int i=0; i<arrows.Length;i++)
        {
            if(arrows[i] != 0) arrowFound = true;
            if (i % 4 == 3)
            {
                if(arrowFound) icons[i/4].SetActive(true);
                arrowFound = false;
            } 
        }
    }
    private Tile GetTile(Vector3 pos)
    {
        pos.y -= cellSize / 2;
        Vector3Int cellPosition = tilemap.WorldToCell(pos);
        Tile tile = tilemap.GetTile<Tile>(cellPosition);
        return tile;
    }
    public bool HasValidTile(Vector3 pos, int index)
    {
        Tile tile = GetTile(pos);
        if (tile != null)
        {
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
        Tile tile = tilemap.GetTile<Tile>(cellPosition);
        if (tile != null)
        {
            if (tile != mainTiles[1] && !goalTiles.Contains(tile))
            {
                if (tileIndex == -1)
                {
                    if (moveTiles.Contains(tile))
                    {
                        tilemap.SetTile(cellPosition, mainTiles[0]);
                        TilesGameManager.Instance.ChangeButtonNumber(moveTiles.IndexOf(tile), 1);
                    }
                }
                else
                {
                    if (tile == mainTiles[0])
                    {
                        if (TilesGameManager.Instance.GetButtonNumber(tileIndex) > 0)
                        {
                            tilemap.SetTile(cellPosition, moveTiles[tileIndex]);
                            return -1;
                        }
                    }
                    else
                    {
                        if(tile == moveTiles[tileIndex])
                        {
                            tilemap.SetTile(cellPosition, mainTiles[0]);
                            return 1;
                        }
                        else
                        {
                            if (TilesGameManager.Instance.GetButtonNumber(tileIndex) > 0)
                            {
                                TilesGameManager.Instance.ChangeButtonNumber(moveTiles.IndexOf(tile), 1);
                                tilemap.SetTile(cellPosition, moveTiles[tileIndex]);
                                return -1;
                            }
                            else
                            {
                                tilemap.SetTile(cellPosition, mainTiles[0]);
                                TilesGameManager.Instance.ChangeButtonNumber(moveTiles.IndexOf(tile), 1);
                            }

                        }                      

                    }
                }
            }
        }
        return 0;
    }
    public bool TileIsMyGoal(Vector3 pos, int index)
    {
        Tile tile = GetTile(pos);
        if (tile != null)
        {
            if (goalTiles.Contains(tile))
            {
                int tileIndex = goalTiles.IndexOf(tile);
                if (tileIndex == index - 1) return true;
            }
        }
        return false;
    }
    public Vector3 GetTileDirection(Vector3 pos, int index)
    {
        Tile tile = GetTile(pos);
        if (tile != null)
        {
            if (moveTiles.Contains(tile))
            {
                int tileIndex = moveTiles.IndexOf(tile);
                if (tileIndex / 4 == 0 || tileIndex / 4 == index)
                {
                    int dirIndex = moveTiles.IndexOf(tile) % 4;
                    return directions[dirIndex];
                }
            }
        }
        return new Vector3(0,0,0);
    }
    public Map[] GetMaps()
    {
        return maps;
    }

    public void HideObjects()
    {
        foreach(GameObject goal in goals)
        {
            goal.SetActive(false);
        }
        foreach(GameObject icon in icons)
        {
            icon.SetActive(false);
        }
    }
}

