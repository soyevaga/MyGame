using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;

public class MeteorSpawner : MonoBehaviour
{
    [SerializeField] private AudioSource explosionSound;
    [SerializeField] private PoolGenerator explosionPool = null;
    [SerializeField] private PoolGenerator meteorPool = null;
    [SerializeField] private float[] posX = {-7f,-5f, -3f, -1f, 1f,3f,5f,7f};
    private float timeDelay = 1.1f;
    private int oddMeteors;
    private int evenMeteors;
    private int size;
    private float meteorSpeed;
    private bool speedChange;
    private int lastIndx;
    void Start()
    {
        lastIndx = 0;
        size = (int)(20 / timeDelay);
        if (size % 2 != 0) size --;
        oddMeteors = 0;
        evenMeteors = 0;
        speedChange= false;        
        InvokeRepeating(nameof(Spawner), 0f, timeDelay);
    }

    void Spawner()
    {
        if (speedChange)
        {
            speedChange = false;
            StartCoroutine(Wait());
        }
        GameObject newMeteor = meteorPool.GetObject();
        if (newMeteor != null)
        {
            newMeteor.transform.parent = transform;
            int xIndx = Random.Range(0, posX.Length);
            if (xIndx == lastIndx)
            {
                if (xIndx > 0) xIndx--;
                else xIndx++;
            }
            lastIndx= xIndx;
            float xPosition = posX[xIndx];
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
    private IEnumerator Wait()
    {
        yield return new WaitForSeconds(1f);
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
        explosionSound.Play();
        yield return new WaitForSeconds(tiempo);
        explosion.SetActive(false);
    }

    public int GetMeteorSize()
    {
        return size;
    }

    
    public void SetMeteorSpeed(float change)
    {
        speedChange = true;
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
    public float GetMeteorSpeed()
    {
        return meteorSpeed;
    }
    public void InitialSpeed(GameManager.mode mode)
    {
        if (mode == GameManager.mode.lineal)
        {
            meteorSpeed = 2f;
        }
        else
        {
            meteorSpeed = 2.25f;
        }
    }
}

