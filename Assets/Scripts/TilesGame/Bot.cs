using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Bot : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private float speed;
    private int myType; //desert=1, woods=2; island=3, volcano=4 
    private bool isMoving;
    private Vector3 currentDirection;
    void Start()
    {
        currentDirection= new Vector3(0, -25.6f, 0);
        Vector3 newPosition = transform.position + currentDirection;
        StartCoroutine(Move(newPosition));

    }
    void Update()
    {
        if (!isMoving)
        {
            if (GridManager.Instance.TileIsMyType(transform.position, myType))
            {
                currentDirection = GridManager.Instance.GetTileDirection(transform.position);
            }
            Vector3 newPosition = transform.position + currentDirection;
            if (GridManager.Instance.HasValidTile(newPosition, myType))
            {
                StartCoroutine(Move(newPosition));
            }
        }
    }
    private IEnumerator Move(Vector3 newPosition)
    {
        isMoving = true;
        yield return new WaitForSeconds(1f);
        while (Vector3.Distance(transform.position, newPosition) > 0.01f)
        {
            transform.position = Vector3.MoveTowards(transform.position, newPosition, speed * Time.deltaTime);
            yield return null; 
        }
        isMoving = false;          
    }
    
    public void SetSprite(Sprite sprite, int type)
    {
        spriteRenderer.sprite = sprite;
        myType = type;
    }
}
