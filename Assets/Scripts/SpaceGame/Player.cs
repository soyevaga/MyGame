
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private Rigidbody rb = null;
    [SerializeField] private float speed = 10f;
    [SerializeField] private PoolGenerator pool = null;
    [SerializeField] private Transform projectilePoint = null;
    private int shoots;
    private float movingTime;
    private float startMovingTime;
    void Start()
    {
        shoots = 0;
        movingTime = 0;
    }
    void Update()
    {
        if (Input.GetKeyDown("space"))
        {
            GameObject newProjectile = pool.GetObject();
            if (newProjectile != null)
            {
                shoots++;
                newProjectile.transform.parent = null;
                newProjectile.transform.position = projectilePoint.position;
                newProjectile.GetComponent<Projectile>().SetPlayer(this);
            }
        }
    }
    void FixedUpdate()
    {
        if (Input.GetKeyDown("w") || Input.GetKeyDown("up")|| Input.GetKeyDown("s") || Input.GetKeyDown("down"))
        {
            startMovingTime = Time.time;
        }
        if (Input.GetKeyUp("w") || Input.GetKeyUp("up") || Input.GetKeyUp("s") || Input.GetKeyUp("down"))
        {
            movingTime += (Time.time - startMovingTime);
        }


        if (Input.GetKey("up") || Input.GetKey("w"))
        {
            rb.MovePosition(transform.position + (-Vector3.right * speed * Time.fixedDeltaTime));
        }   
        else if (Input.GetKey("down") || Input.GetKey("s"))
        {
            rb.MovePosition(transform.position + (Vector3.right * speed * Time.fixedDeltaTime));
        }
    }
  
    public void DeleteProjectile(GameObject obj)
    {
        pool.DeleteObject(obj);
    }    

    public int GetShoots()
    {
        return shoots;
    }
    public float GetMovingTime()
    {
        return movingTime;
    }
}

