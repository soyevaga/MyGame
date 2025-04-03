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
    [SerializeField] private TextMeshProUGUI gameOverTimeText;
    [SerializeField] private Image timeImage;
    [SerializeField] private GameObject newRecord;
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private GameObject gamePanel;
    [SerializeField] private GameObject tutorialPanel;
    [SerializeField] private CardSpawner cardSpawner;
    [SerializeField] public UnityEvent onNextLevel;
    [SerializeField] public UnityEvent onGameOver;
    private float remainingTime;
    private int currentLevel;
    private int[] cardsNumber = { 4, 8, 12, 16, 22};
    private int[] exchangeNum = { 0, 1,  1,  1,  2};
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

    new void Start()
    {
        base.Start();
        if (!PlayerPrefs.HasKey(username + "cards"))
        {
            PlayerPrefs.SetFloat(username + "cards", 999999f);
            PlayerPrefs.Save();
        }
        Time.timeScale = 1f;
        selectedCards = new HashSet<Card> ();
        remainingTime = 0f;
        currentLevel = 0;
        misses = 0;
        correctPairs = 0;
        TutorialButton();
        StartCoroutine(NewLevel());
    }
    void Update()
    {
        remainingTime += Time.deltaTime;       
        if (selectedCardsCount == 2)
        {
            selectedCardsCount = 0;
            CheckPair();
        }
        timeText.text = TimeFormat(remainingTime);
        pointsText.text =""+ misses;
        levelText.text = "NIVEL " + (currentLevel+1);
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
            StartCoroutine(NewLevel());
        }
        else
        {
            onGameOver.Invoke();
        }
    }
    private IEnumerator NewLevel()
    {
        yield return new WaitForSeconds(0.5f);
        newlevelText.text = "NIVEL " + (currentLevel+1);       
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
    public void GameOver()
    {
        StartCoroutine(GameOverCoroutine());
        
    }

    private IEnumerator GameOverCoroutine()
    {
        yield return new WaitForSeconds(1f);
        gamePanel.SetActive(false);
        tutorialPanel.SetActive(false);
        gameOverPanel.SetActive(true);
        gameOverTimeText.text = TimeFormat(remainingTime);
        if (remainingTime < PlayerPrefs.GetFloat(username + "cards"))
        {
            PlayerPrefs.SetFloat(username + "cards", remainingTime);
            PlayerPrefs.Save();
            StartCoroutine(NewRecord());
        }
        else
        {
            newRecord.SetActive(false);
        }
    }
    private IEnumerator NewRecord()
    {
        for (int i = 0; i < 3; i++)
        {
            newRecord.SetActive(true);
            yield return new WaitForSeconds(0.6f);
            newRecord.SetActive(false);
            yield return new WaitForSeconds(0.6f);
        }
        newRecord.SetActive(true);
    }
    public void TutorialButton()
    {
        Time.timeScale = 0f;
        tutorialPanel.SetActive(true);
        gameOverPanel.SetActive(false);
        gamePanel.SetActive(false);
    }
    public void ExitTutorialButton()
    {
        Time.timeScale = 1f;
        tutorialPanel.SetActive(false);
        gameOverPanel.SetActive(false);
        gamePanel.SetActive(true);
    }
    public void ReplayButton()
    {
        SceneManager.LoadScene("CardsScene");
    }
    
}

