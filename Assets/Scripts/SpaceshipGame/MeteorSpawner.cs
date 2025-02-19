using System.Collections.Generic;
using UnityEngine;

public class MeteorSpawner : MonoBehaviour
{
    [SerializeField] private GameObject meteorPrefab = null;
    [SerializeField] private int poolSize = 10;
    [SerializeField] private float minX = -14f;
    [SerializeField] private float maxX = 14f;
    private List<GameObject> pool;
    void Start()
    {
        pool = new List<GameObject>();

        for (int i = 0; i < poolSize; i++)
        {
            GameObject obj = Instantiate(meteorPrefab, transform);
            obj.SetActive(false);
            pool.Add(obj);
        }

        InvokeRepeating(nameof(Spawner), 0f, 1f);
    }

    void Spawner()
    {
        GameObject newMeteor = GetMeteor();
        if (newMeteor != null)
        {
            float xPosition = Random.Range(minX, maxX);
            newMeteor.transform.position = new Vector3(xPosition, transform.position.y, transform.position.z);
            newMeteor.SetActive(true);
            Meteor m = newMeteor.gameObject.GetComponentInParent<Meteor>();
            m.SetNumber(Random.Range(1, 101));
        }
    }
    private GameObject GetMeteor()
    {
        foreach (GameObject obj in pool)
        {
            if (!obj.activeInHierarchy)
            {
                return obj;
            }
        }
        return null;
    }
    public void DeleteMeteor(GameObject obj)
    {
        obj.SetActive(false);
    }
}

