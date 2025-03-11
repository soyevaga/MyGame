using UnityEngine;
using UnityEngine.Tilemaps;

public class GridManager : MonoBehaviour
{
    [SerializeField] private Tilemap tilemap;
    [SerializeField] private Tile[] tiles;
    [SerializeField] private int width = 11;
    [SerializeField] private int height = 9;

    void Start()
    {
    }

    public void GenerateGrid()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Tile selectedTile = tiles[Random.Range(5,10)];  
                Vector3Int cellPosition = new Vector3Int(x, y, 0);  
                tilemap.SetTile(cellPosition, selectedTile);  
            }
        }
    }
}

