using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;

public class MeteorSpawner : MonoBehaviour
{
    [SerializeField] private PoolGenerator explosionPool = null;
    [SerializeField] private PoolGenerator meteorPool = null;
    [SerializeField] private float minX = -7f;
    [SerializeField] private float maxX = 7f;
    private float timeDelay = 1f;
    private int oddMeteors;
    private int evenMeteors;
    private int size;
    private float meteorSpeed;
    void Start()
    {
        size = (int)(20 / timeDelay);
        if (size % 2 != 0) size --;
        oddMeteors = 0;
        evenMeteors = 0;
        InvokeRepeating(nameof(Spawner), 0f, timeDelay);
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
            m.SetSpeed(meteorSpeed);
            int number = Random.Range(1, 101);
            if (number % 2 == 0)
            {
                if (evenMeteors >= size/2)
                {
                    if (number > 0) number--;
                    else number++;
                    oddMeteors++;
                }
                else evenMeteors++;
            }
            else
            {
                if (oddMeteors >= size/2)
                {
                    if (number > 0) number--;
                    else number++;
                    evenMeteors++;
                }
                else
                {
                    oddMeteors++;
                }
            }
            m.SetNumber(number);
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

    public int GetMeteorSize()
    {
        return size;
    }

    
    public void SetMeteorSpeed(float change)
    {
        if (SpaceGameManager.Instance.GetGameMode() == GameManager.mode.lineal)
        {
            meteorSpeed += change;
            timeDelay -= 0.1f * change;
        }
        else
        {
            if (change > 0)
            {
                meteorSpeed *= 1.5f;
            }
            else
            {
                meteorSpeed /= 1.5f;
            }

            timeDelay=1-(((Mathf.Log(meteorSpeed) / Mathf.Log(1.5f))-2)*0.1f);
        }
           
        if (meteorSpeed <= 0) meteorSpeed = 1f;
    }

    public void InitialSpeed(GameManager.mode mode)
    {
        if (mode == GameManager.mode.lineal)
        {
            meteorSpeed = 3f;
        }
        else
        {
            meteorSpeed = 1.5f * 2.25f;
        }
    }
}

