using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardSpawner : MonoBehaviour
{
    [SerializeField] private PoolGenerator pool = null;
    private Vector3[] Positions;
    private static int maxCards = 22;
    [SerializeField] private Material[] materials = new Material[maxCards / 2];
    void Start()
    {
        AssignPositions(); 
    }
    public void Spawner(int cardsToCreate)
    {
        Dictionary<int, int> dict = new Dictionary<int, int>(cardsToCreate / 2);
        for (int i = 0; i < cardsToCreate; i++)
        {
            GameObject newCard = pool.GetObject();
            if (newCard != null)
            {
                newCard.transform.position = Positions[i];    
                Card card = newCard.GetComponent<Card>();
                card.SetMaterial(FindMaterial(dict,cardsToCreate / 2));
                card.FlipCard(3f);
            }
        }
    }
    public void Exchange(int times)
    {
        List<GameObject> cards = pool.GetActiveObjs();
        HashSet<int> indx = new HashSet<int>();
        for (int i = 0; i < times; i++)
        {
            Card card1 = cards[findIndex(indx,cards.Count)].GetComponent<Card>();
            Card card2 = cards[findIndex(indx, cards.Count)].GetComponent<Card>();
            card1.ExchangeCards(card2);
        }
    }
    private int findIndex(HashSet<int> set, int max)
    {
        int aux = Random.Range(0, max);
        while (set.Contains(aux))
        {
            aux = Random.Range(0, max);
        }
        set.Add(aux);
        return aux;
    }
    public void DeleteCard(Card card)
    {
        StartCoroutine(Delete(card));
    }
    private IEnumerator Delete(Card card)
    {
        card.Restart();
        yield return new WaitForSeconds(1.1f);
        pool.DeleteObject(card.gameObject);
    }
    private Material FindMaterial(Dictionary<int,int> dict, int maxMaterial)
    {
        while (true)
        {
            int myMaterial = Random.Range(0, maxMaterial);
            if (dict.ContainsKey(myMaterial))
            {
                if (dict[myMaterial] == 1)
                {
                    dict[myMaterial] = 2;
                    return materials[myMaterial];
                }
            }
            else
            {
                dict.Add(myMaterial, 1);
                return materials[myMaterial];
            }
        }
    }
    
    private void AssignPositions()
    {
        Positions = new Vector3[maxCards];
        //X: -5 -3.5 -2 -0.5 1 2.5 4 5.5
        //Y: 6.8
        //Z: 5.5 7.5 9.5
        Positions[0] = new Vector3(-0.5f, 6.8f, 7.5f);
        Positions[1] = new Vector3(1f, 6.8f, 7.5f);
        Positions[2] = new Vector3(-0.5f, 6.8f, 9.5f);
        Positions[3] = new Vector3(1f, 6.8f, 9.5f);
        Positions[4] = new Vector3(-0.5f, 6.8f, 5.5f);
        Positions[5] = new Vector3(1f, 6.8f, 5.5f);

        Positions[6] = new Vector3(-2f, 6.8f, 7.5f);
        Positions[7] = new Vector3(2.5f, 6.8f, 7.5f);
        Positions[8] = new Vector3(-2f, 6.8f, 9.5f);
        Positions[9] = new Vector3(2.5f, 6.8f, 9.5f);
        Positions[10] = new Vector3(-2f, 6.8f, 5.5f);
        Positions[11] = new Vector3(2.5f, 6.8f, 5.5f);

        Positions[12] = new Vector3(-3.5f, 6.8f, 7.5f);
        Positions[13] = new Vector3(4f, 6.8f, 7.5f);
        Positions[14] = new Vector3(-3.5f, 6.8f, 9.5f);
        Positions[15] = new Vector3(4f, 6.8f, 9.5f);
        Positions[16] = new Vector3(-3.5f, 6.8f, 5.5f);
        Positions[17] = new Vector3(4f, 6.8f, 5.5f);

        Positions[18] = new Vector3(-5f, 6.8f, 7.5f);
        Positions[19] = new Vector3(5.5f, 6.8f, 7.5f);

        Positions[20] = new Vector3(-5f, 6.8f, 9.5f);
        Positions[21] = new Vector3(5.5f, 6.8f, 9.5f);
    }
}
