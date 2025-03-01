using System.Collections.Generic;
using System.Data.Common;
using UnityEngine;

public class PoolGenerator : MonoBehaviour
{
    [SerializeField] private int poolSize = 20;
    [SerializeField] private GameObject Prefab = null;
    private List<GameObject> pool;
    void Start()
    {
        pool = new List<GameObject>();

        for (int i = 0; i < poolSize; i++)
        {
            GameObject obj = Instantiate(Prefab);
            obj.SetActive(false);
            pool.Add(obj);
        }
    }
    public GameObject GetObject()
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
    public void DeleteObject(GameObject obj)
    {
        obj.SetActive(false);
    }

}
