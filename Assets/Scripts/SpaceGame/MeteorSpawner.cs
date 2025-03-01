using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteorSpawner : MonoBehaviour
{
    [SerializeField] private PoolGenerator explosionPool = null;
    [SerializeField] private PoolGenerator meteorPool = null;
    [SerializeField] private float minX = -14f;
    [SerializeField] private float maxX = 14f;
    void Start()
    {
        InvokeRepeating(nameof(Spawner), 0f, 1f);
    }

    void Spawner()
    {
        GameObject newMeteor = meteorPool.GetObject();
        if (newMeteor != null)
        {
            newMeteor.transform.parent = this.transform;
            float xPosition = Random.Range(minX, maxX);
            newMeteor.transform.position = new Vector3(xPosition, transform.position.y, transform.position.z);
            Meteor m = newMeteor.gameObject.GetComponentInParent<Meteor>();
            m.SetNumber(Random.Range(1, 101));
        }
    }
    public void DeleteMeteor(GameObject obj)
    {
        GameObject explosion = explosionPool.GetObject();
        explosion.transform.position = obj.transform.position;
        explosion.SetActive(true);
        obj.SetActive(false);
        StartCoroutine(DeleteExplosion(explosion, 1f));
    }

    private IEnumerator DeleteExplosion(GameObject explosion, float tiempo)
    {
        yield return new WaitForSeconds(tiempo);
        explosion.SetActive(false);
    }
}

