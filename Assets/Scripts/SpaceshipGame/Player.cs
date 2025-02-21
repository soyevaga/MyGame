using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private Rigidbody rb = null;
    [SerializeField] private float speed = 10f;
    [SerializeField] private GameObject projectilePrefab = null;
    [SerializeField] private Transform projectilePoint = null;
    [SerializeField] private int poolSize = 20;
    private List<GameObject> pool;
    void Start()
    {
        pool = new List<GameObject>();

        for (int i = 0; i < poolSize; i++)
        {
            GameObject obj = Instantiate(projectilePrefab, projectilePoint);
            obj.transform.parent = null;
            obj.GetComponent<Projectile>().SetPlayer(this);
            obj.SetActive(false);
            pool.Add(obj);
        }
    }

    void Update()
    {
        if (Input.GetKeyDown("space"))
        {
            GameObject newProjectile = GetProjectile();
            if (newProjectile != null)
            {
                newProjectile.transform.position= projectilePoint.position;
            }
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
    private GameObject GetProjectile()
    {
        foreach (GameObject obj in pool)
        {
            if (!obj.activeInHierarchy)
            {
                obj.SetActive(true);
                return obj;
            }
        }
        return null;
    }
    public void DeleteProjectile(GameObject obj)
    {
        obj.SetActive(false);
    }

    
}

