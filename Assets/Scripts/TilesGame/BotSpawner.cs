using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class BotSpawner : MonoBehaviour
{
    [SerializeField] private PoolGenerator pool;
    [SerializeField] private Tilemap tilemap;
    [SerializeField] private float cellSize;
    [SerializeField] private Sprite[] sprites = new Sprite[4];
    private int[] types = { 1,2,3,4};
    private Vector3 initialPos;
    private bool isSpawning=false;
    private void Start()
    {
        initialPos = new Vector3(0 + cellSize/2, GridManager.Instance.GetHeight() * cellSize - cellSize/2, -1);
    }
    public void Spawner(int typesNum)
    {
        StartCoroutine(Spawn(typesNum));
    }

    private IEnumerator Spawn(int typesNum)
    {
        isSpawning = true;
        for (int i = 0; i < typesNum && isSpawning; i++)
        {
            GameObject newBot = pool.GetObject();
            if (newBot != null)
            {
                Bot bot = newBot.GetComponent<Bot>();
                bot.SetSprite(sprites[i], types[i]);
                bot.transform.position = initialPos;
                bot.FirstMove();
            }
            yield return new WaitForSeconds(3f);
            
        }
        isSpawning = false;
    }
    public void DeleteBot(Bot bot)
    {
        pool.DeleteObject(bot.gameObject);
    }
    public void DeleteAllBots()
    {
        StartCoroutine(DeleteAll());
    }
    private IEnumerator DeleteAll()
    {
        isSpawning = false;
        pool.DeleteAllObjects();
        yield return new WaitForSeconds(1f);
    }

}
