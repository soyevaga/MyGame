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
            "easeType", "easeInOutQuart",
            "time", 0.8f
        ));
        yield return new WaitForSeconds(0.8f);

        transform.position = new Vector3(transform.position.x, transform.position.y - 0.2f, transform.position.z);
        isRotating = false;
    }

    void OnMouseDown()
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

