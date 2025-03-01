using UnityEngine;
using System.Collections;

public class RotateSample : MonoBehaviour
{	
    void Start()
    {
        FlipCard();
    }
	void FlipCard(){
        iTween.MoveBy(gameObject, iTween.Hash(
                "y", 0.2,
                "time", 0.2,
                "easeType", "easeOutQuad",
                "onComplete", "RotateCard" 
            ));
        
    }
    void RotateCard()
    {
        iTween.RotateBy(gameObject, iTween.Hash(
            "z", .5, 
            "easeType", "easeInOutQuart", 
            "time", 0.8,
            "delay", 0.2,
            "onComplete", "DownCard"));

    }
    void DownCard()
    {
        iTween.MoveBy(gameObject, iTween.Hash(
            "y", 0.2,
            "time", 0.2,
            "easeType", "easeInQuad"
        ));
    }

}

