using System.Collections;
using UnityEngine;

public class Bot : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private float speed;
    [SerializeField] private Color normalColor = Color.white;
    [SerializeField] private Color deadColor = Color.red;
    private int myType; //desert=1, woods=2; island=3, volcano=4 
    private bool isMoving;
    private bool isDead;
    private Vector3 currentDirection;
    public void FirstMove()
    {
        SetDead(false);
        currentDirection= new Vector3(0, -25.6f, 0);
        Vector3 newPosition = transform.position + currentDirection;
        StartCoroutine(Move(newPosition));

    }
    void Update()
    {
        if (!isMoving && !isDead)
        {
            //Checks if current tile changes direction
            if (GridManager.Instance.TileIsMyMoveType(transform.position, myType))
            {
                currentDirection = GridManager.Instance.GetTileDirection(transform.position);
            }

            //Checks if current tile is its goal
            if (GridManager.Instance.TileIsMyGoal(transform.position, myType))
            {
                TilesGameManager.Instance.BotInGoal(this);
            }
            else
            {
                Vector3 newPosition = transform.position + currentDirection;
                //Checks if there is no tile
                if (!GridManager.Instance.HasTile(newPosition, myType))
                {
                    StartCoroutine(Move(newPosition));
                    SetDead(true);
                }
                else
                {
                    //Checks if future tile is valid
                    if (GridManager.Instance.HasValidTile(newPosition, myType))
                    {
                        StartCoroutine(Move(newPosition));
                    }
                    else
                    {
                        SetDead(true);
                    }
                }
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

    public void SetDead(bool isDead)
    {
        this.isDead = isDead;
        if (isDead == true)
        {
            spriteRenderer.color = deadColor;
            TilesGameManager.Instance.AddDead();
        }
        else
        {
            spriteRenderer.color = normalColor;
        }
    }
}
