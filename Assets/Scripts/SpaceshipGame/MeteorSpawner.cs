using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteorSpawner : MonoBehaviour
{
    [SerializeField] private GameObject meteorPrefab = null;
    [SerializeField] private GameObject explosionPrefab = null;
    [SerializeField] private int poolSize = 10;
    [SerializeField] private float minX = -14f;
    [SerializeField] private float maxX = 14f;
    private List<GameObject> Meteorpool;
    private List<GameObject> Explosionpool;
    void Start()
    {
        Meteorpool = new List<GameObject>();
        Explosionpool = new List<GameObject>();

        for (int i = 0; i < poolSize; i++)
        {
            GameObject obj = Instantiate(meteorPrefab, transform);
            obj.SetActive(false);
            Meteorpool.Add(obj);
            obj = Instantiate(explosionPrefab, transform);
            obj.SetActive(false);
            Explosionpool.Add(obj);
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
        foreach (GameObject obj in Meteorpool)
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
        GameObject explosion = GetExplosion();
        explosion.transform.position= obj.transform.position;
        explosion.SetActive(true);
        obj.SetActive(false);
        StartCoroutine(DeleteExplosion(explosion, 1f)); 
    }

    private IEnumerator DeleteExplosion(GameObject explosion, float tiempo)
    {
        yield return new WaitForSeconds(tiempo);
        explosion.SetActive(false);
    }

    private GameObject GetExplosion()
    {
        foreach (GameObject obj in Explosionpool)
        {
            if (!obj.activeInHierarchy)
            {
                return obj;
            }
        }
        return null;
    }
}

