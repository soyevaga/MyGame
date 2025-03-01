using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class CardsGameManager : GameManager
{
    public static CardsGameManager Instance { get; private set; }

    [SerializeField] private TextMeshProUGUI pointsText;
    [SerializeField] private CardSpawner cardSpawner;
    [SerializeField] public UnityEvent onNextLevel;
    private int currentLevel;
    private int[] cardsNumber = { 4, 8, 12, 16, 20, 22 };
    private HashSet<Card> selectedCards;
    private int selectedCardsCount;
    private int hits;
    private int misses;
    private int correctPairs;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        selectedCards= new HashSet<Card> ();
        base.Start();
        currentLevel = 0;
        hits = 0;
        misses = 0;
        correctPairs = 0;
        cardSpawner.Spawner(cardsNumber[currentLevel]);
    }
    void Update()
    {
        if (selectedCardsCount == 2)
        {
            selectedCardsCount = 0;
            CheckPair();
        }
        pointsText.text = "Sel: " + selectedCardsCount+"\n Hits: "+hits+"\n Misses: "+misses;
    }
    private void CheckPair()
    {
        Card[] array = selectedCards.ToArray();
        selectedCards.Clear();
        if (array[0].Equals(array[1]))
        {
            StartCoroutine(DeletePair(array[0], array[1]));
            hits++;
            correctPairs++;
        }
        else
        {
            array[0].FlipCard(1f);
            array[1].FlipCard(1f);
            array[0].Restart();
            array[1].Restart();
            misses++;
        }

        if(correctPairs== cardsNumber[currentLevel] / 2)
        {
            StartCoroutine(NextLevelSequence());
        }
    }
    private IEnumerator NextLevelSequence()
    {
        yield return new WaitForSeconds(3f);
        onNextLevel.Invoke();
    }

    private IEnumerator DeletePair(Card c1, Card c2)
    {
        c1.RevealCard();
        c2.RevealCard();
        yield return new WaitForSeconds(2f);
        cardSpawner.DeleteCard(c1);
        cardSpawner.DeleteCard(c2);
    }
    public void nextLevel()
    {
        correctPairs = 0;
        currentLevel++;
        cardSpawner.Spawner(cardsNumber[currentLevel]);
    }
    public void RemoveSelectedCard(Card card)
    {
        selectedCards.Remove(card);
        selectedCardsCount--;
    }
    public bool AddSelectedCard(Card card)
    {
        if (selectedCards.Count<2)
        {
            selectedCards.Add(card);
            selectedCardsCount++;
            return true;
        }
        else
        {
            return false;
        }
    }
}
