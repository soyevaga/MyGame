using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private Rigidbody rb = null;
    [SerializeField] private float speed = 10f;
    [SerializeField] private PoolGenerator pool = null;
    [SerializeField] private Transform projectilePoint = null;
    void Update()
    {
        if (Input.GetKeyDown("space"))
        {
            GameObject newProjectile = pool.GetObject();
            newProjectile.transform.parent = null;
            newProjectile.transform.position = projectilePoint.position;
            newProjectile.GetComponent<Projectile>().SetPlayer(this);      
        }
    }
    void FixedUpdate()
    {
        if (Input.GetKey("left") || Input.GetKey("a"))
        {
            rb.MovePosition(transform.position + (-Vector3.right * speed * Time.fixedDeltaTime));
        }   
        else if (Input.GetKey("right") || Input.GetKey("d"))
        {
            rb.MovePosition(transform.position + (Vector3.right * speed * Time.fixedDeltaTime));
        }
    }
  
    public void DeleteProjectile(GameObject obj)
    {
        pool.DeleteObject(obj);
    }    
}

