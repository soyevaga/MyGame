using System.Collections;
using UnityEngine;

public class Card : MonoBehaviour
{
    [SerializeField] private Renderer frontCard=null;
    [SerializeField] private Renderer backCard = null;
    [SerializeField] private Material backStandard=null;
    [SerializeField] private Material backSelected = null;
    [SerializeField] private bool isFaceDown = true;
    private bool isRotating = false;
    
    public void ExchangeCards(Card other)
    {        
        StartCoroutine(Exchange(gameObject,other.gameObject));
    }

    private IEnumerator Exchange(GameObject c1, GameObject c2)
    {
        yield return new WaitForSeconds(4.2f);

        Vector3 pos1 = c1.transform.position;
        Vector3 pos2 = c2.transform.position;
        Vector3 center = (pos1 + pos2) / 2;
        float radius = Vector3.Distance(pos1, pos2) / 2;
        Vector3 perpendicular = (pos2 - pos1).normalized;
        Vector3 normal = Vector3.Cross(perpendicular, Vector3.up);
        Vector3 mid1 = center + normal * radius + Vector3.forward * 0.5f + Vector3.up * 0.2f; ;
        Vector3 mid2 = center - normal * radius + Vector3.forward * 0.5f + Vector3.up * 0.2f; ;
        Vector3[] path1 = new Vector3[] { pos1, mid1, pos2 };
        Vector3[] path2 = new Vector3[] { pos2, mid2, pos1 };

        iTween.MoveTo(c1, iTween.Hash(
            "path", path1,
            "time", 2f,
            "easetype", iTween.EaseType.easeInOutSine
        ));

        iTween.MoveTo(c2, iTween.Hash(
            "path", path2,
            "time", 2f,
            "easetype", iTween.EaseType.easeInOutSine
        ));
        yield return new WaitForSeconds(2f);

    }
    public void SetMaterial(Material material)
    {
        frontCard.material = material;
    }
    public void FlipCard(float time)
    {
        StartCoroutine(Flip(time));
    }
    private IEnumerator Flip(float time)
    {
        StartCoroutine(Rotate(1));
        yield return new WaitForSeconds(time);
        StartCoroutine(Rotate(-1));
    }

    public void RevealCard()
    {
        StartCoroutine(Rotate(1));
        isFaceDown = false;
    }

    public void HideCard()
    {
        StartCoroutine(Rotate(-1));
        isFaceDown = true;
    }
     
    private IEnumerator Rotate(int dir)
    {
        isRotating = true;
        transform.position = new Vector3(transform.position.x, transform.position.y + 0.2f, transform.position.z);

        yield return new WaitForSeconds(0.2f);
        iTween.RotateBy(gameObject, iTween.Hash(
            "z", dir*0.5f,
            "easeType", iTween.EaseType.easeInOutQuart,
            "time", 0.8f
        ));
        yield return new WaitForSeconds(0.8f);

        transform.position = new Vector3(transform.position.x, transform.position.y - 0.2f, transform.position.z);
        isRotating = false;
    }

    void OnMouseDown()
    {
        if (!CardsGameManager.Instance.GetIsGenerating())
        {
            if (isFaceDown && !isRotating)
            {
                if (backCard.sharedMaterial == backSelected)
                {
                    CardsGameManager.Instance.RemoveSelectedCard(GetComponent<Card>());
                    backCard.material = backStandard;
                }
                else
                {
                    if (CardsGameManager.Instance.AddSelectedCard(GetComponent<Card>()))
                    {
                        backCard.material = backSelected;
                    }
                }
            }
        }
    }

    public void Restart()
    {
        if (!isFaceDown)
        {
            HideCard();
        }
        backCard.material = backStandard;
    }
    public bool Equals(Card other)
    {
        return other.frontCard.sharedMaterial == frontCard.sharedMaterial;
    }
}

