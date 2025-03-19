using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BotSpawner : MonoBehaviour
{
    public enum type
    {
        desert,
        woods,
        island,
        volcano
    }

    [SerializeField] private PoolGenerator pool;
    [SerializeField] private Tilemap tilemap;
    [SerializeField] private float cellSize;
    [SerializeField] private Sprite[] sprites = new Sprite[4];
    private type[] types;
    private Vector3 initialPos;
    private void Start()
    {
        types = (type[]) Enum.GetValues(typeof(type));
        initialPos = new Vector3(0 + cellSize/2, GridManager.Instance.GetHeight() * cellSize + cellSize/2, -1);
    }
    public void Spawner(int typesNum, int botsOfEachType)
    {
        StartCoroutine(Spawn(typesNum, botsOfEachType));
    }

    private IEnumerator Spawn(int typesNum, int botsOfEachType)
    {
        for (int i = 0; i < typesNum; i++)
        {
            for (int j = 0; j < botsOfEachType; j++)
            {
                GameObject newBot = pool.GetObject();
                if (newBot != null)
                {
                    Bot bot = newBot.GetComponent<Bot>();
                    bot.SetSprite(sprites[i], types[i]);
                    bot.transform.position = initialPos;
                }
                yield return new WaitForSeconds(3f);
            }
        }
    }
}
