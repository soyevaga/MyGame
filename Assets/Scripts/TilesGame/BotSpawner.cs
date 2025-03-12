using UnityEngine;
using UnityEngine.Tilemaps;

public class BotSpawner : MonoBehaviour
{
    [SerializeField] private PoolGenerator pool;
    [SerializeField] private Tilemap tilemap;
    [SerializeField] private float cellSize;
    [SerializeField] private Sprite[] sprites = new Sprite[5];
    private Vector3[] Positions;
    private void Start()
    {
        AssignPositions();  
    }
    public void Spawner(int types, int botsOfEachType)
    {
        for (int i = 0; i < types; i++)
        {
            Vector3 pos = Positions[Random.Range(0, Positions.Length)];
            for(int j=0; j< botsOfEachType; j++)
            {
                GameObject newBot = pool.GetObject();
                if (newBot != null)
                {
                    Bot bot = newBot.GetComponent<Bot>();
                    bot.SetSprite(sprites[i]);
                    bot.SetTilemap(tilemap);
                    bot.transform.position = pos;   
                }
            }
            
        }
    }
    private void AssignPositions()
    {
        Positions = new Vector3[4];
        Positions[0] = new Vector3(0 + 12.8f, GridManager.Instance.GetHeight()* cellSize - cellSize/2, -1);
        Positions[1] = new Vector3(GridManager.Instance.GetWidth() * cellSize - cellSize / 2, GridManager.Instance.GetHeight() * cellSize - cellSize / 2, -1);
        Positions[2] = new Vector3(0 + 12.8f, 0 + 12.8f, -1);
        Positions[3] = new Vector3(GridManager.Instance.GetWidth() * cellSize - cellSize / 2, 0 + 12.8f, -1);
    }
}
