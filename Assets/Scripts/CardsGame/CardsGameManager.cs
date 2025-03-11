using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class CardsGameManager : GameManager
{
    public static CardsGameManager Instance { get; private set; }

    [SerializeField] private TextMeshProUGUI newlevelText;
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private TextMeshProUGUI pointsText;
    [SerializeField] private Image pointsImage;
    [SerializeField] private TextMeshProUGUI timeText;
    [SerializeField] private Image timeImage;
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private CardSpawner cardSpawner;
    [SerializeField] public float totalTime = 20f;
    [SerializeField] public UnityEvent onNextLevel;
    private float remainingTime;
    private int currentLevel;
    private int[] cardsNumber = { 4, 8, 12, 14, 16, 18, 20, 22, 22, 22};
    private int[] exchangeNum = { 0, 1,  1,  1,  2,  2,  3,  3,  4,  5};
    private HashSet<Card> selectedCards;
    private int selectedCardsCount;
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
        if (!PlayerPrefs.HasKey(username))
        {
            PlayerPrefs.SetInt(username, int.MaxValue);
        }
        Time.timeScale = 1f;
        selectedCards = new HashSet<Card> ();
        base.Start();
        remainingTime = totalTime;
        currentLevel = 0;
        misses = 0;
        correctPairs = 0;
        StartCoroutine(NewLevel());
    }
    void Update()
    {
        remainingTime-= Time.deltaTime;
        if(remainingTime <= 0)
        {
            GameOver();
        }
        if (selectedCardsCount == 2)
        {
            selectedCardsCount = 0;
            CheckPair();
        }
        int minutes = Mathf.FloorToInt(remainingTime / 60);
        int seconds = Mathf.FloorToInt(remainingTime % 60);
        timeText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        pointsText.text =""+ misses;
        levelText.text = "LEVEL " + (currentLevel+1);
    }
    private void CheckPair()
    {
        Card[] array = selectedCards.ToArray();
        selectedCards.Clear();
        if (array[0].Equals(array[1]))
        {
            StartCoroutine(DeletePair(array[0], array[1]));
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
        if (currentLevel < cardsNumber.Length-1)
        {
            currentLevel++;
        }
        else
        {
            currentLevel=cardsNumber.Length-1;
        }
        StartCoroutine(NewLevel());
    }
    private IEnumerator NewLevel()
    {
        yield return new WaitForSeconds(0.5f);
        newlevelText.text = "LEVEL " + (currentLevel+1);       
        yield return new WaitForSeconds(1f);
        newlevelText.text = "";
        yield return new WaitForSeconds(0.5f);
        cardSpawner.Spawner(cardsNumber[currentLevel]);
        cardSpawner.Exchange(exchangeNum[currentLevel]);
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
    private void GameOver()
    {
        Time.timeScale = 0f;
        timeImage.gameObject.SetActive(false);
        pointsImage.gameObject.SetActive(false);
        timeText.gameObject.SetActive(false);
        pointsText.gameObject.SetActive(false);
        usernameText.gameObject.SetActive(false);
        gameOverPanel.SetActive(true); 
        if (misses < PlayerPrefs.GetInt(username))
        {
            PlayerPrefs.SetInt(username, misses);
        }
    }
    public void ReplayButton()
    {
        SceneManager.LoadScene("CardsScene");
    }
}

