using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Bot : MonoBehaviour
{

    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private float speed;
    private Vector3[] directions = {new Vector3(0, 25.6f, 0), new Vector3(0, -25.6f, 0), new Vector3(25.6f,0, 0), new Vector3(-25.6f, 0, 0) };
    private BotSpawner.type myType;
    void Start()
    {
    }

    void Update()
    {
        Vector3 newPosition = transform.position + directions[1];

        if (GridManager.Instance.HasTile(newPosition))
        {
            StartCoroutine(Move(newPosition));            
        }
    }
    private IEnumerator Move(Vector3 newPosition)
    {
        yield return new WaitForSeconds(2f);
        transform.position = Vector3.MoveTowards(transform.position, newPosition, speed * Time.deltaTime);
    }
    
    public void SetSprite(Sprite sprite, BotSpawner.type type)
    {
        spriteRenderer.sprite = sprite;
        myType = type;
    }
}
