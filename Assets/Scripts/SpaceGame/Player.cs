using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private Rigidbody rb = null;
    [SerializeField] private float speed = 10f;
    [SerializeField] private PoolGenerator pool = null;
    [SerializeField] private Transform projectilePoint = null;
    private int shoots;
    void Start()
    {
        shoots = 0;
    }
    void Update()
    {
        if (Input.GetKeyDown("space"))
        {
            shoots++;
            GameObject newProjectile = pool.GetObject();
            newProjectile.transform.parent = null;
            newProjectile.transform.position = projectilePoint.position;
            newProjectile.GetComponent<Projectile>().SetPlayer(this);      
        }
    }
    void FixedUpdate()
    {
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
}

