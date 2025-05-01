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
    [SerializeField] private GameObject endGamePanel;
    [SerializeField] private GameObject gamePanel;
    [SerializeField] private GameObject tutorialPanel;
    [SerializeField] private CardSpawner cardSpawner;
    [SerializeField] public UnityEvent onNextLevel;
    [SerializeField] public UnityEvent onGameOver;
    private mode gameMode;
    private float remainingTime;
    private float levelTime;
    private int currentLevel;
    private int cardsNumber;
    private int exchangeNumber;
    private bool exchangeAdded;
    private HashSet<Card> selectedCards;
    private int selectedCardsCount;
    private int misses;
    private int currentMisses;
    private int clicks;
    private int correctPairs;
    private int totalCorrect;
    private bool isGenerating;
    private bool isChecking;
    private bool newGame;
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
        string type = "Type"+  PlayerPrefs.GetInt("CurrentGameNumber");
        if (PlayerPrefs.GetString(type) == "Lineal") gameMode = mode.lineal;
        else gameMode = mode.exponential;
        if (!PlayerPrefs.HasKey(username + "cards"))
        {
            PlayerPrefs.SetFloat(username + "cards", 999999f);
            PlayerPrefs.Save();
        }
        cardsNumber = 4;
        exchangeNumber = 0; 
        exchangeAdded = false;
        Time.timeScale = 1f;
        selectedCards = new HashSet<Card> ();
        remainingTime = 0f;
        currentLevel = 1;
        misses = 0;
        clicks = 0;
        correctPairs = 0;
        totalCorrect = 0;
        newGame = true;
        TutorialButton();
        StartCoroutine(NewLevel());
    }
    void Update()
    {
        if (Input.GetMouseButtonDown(0)) clicks++;
        remainingTime += Time.deltaTime;       
        levelTime += Time.deltaTime;
        if (selectedCardsCount == 2 && !isGenerating && !isChecking)
        {
            selectedCardsCount = 0;
            StartCoroutine(CheckPair());
        }
        timeText.text = TimeFormat(remainingTime);
        pointsText.text =""+ misses;
        levelText.text = "NIVEL " + currentLevel;
    }
    private IEnumerator CheckPair()
    {
        isChecking = true;
        Card[] array = selectedCards.ToArray();
        selectedCards.Clear();
        if (array[0].Equals(array[1]))
        {
            StartCoroutine(DeletePair(array[0], array[1]));
            correctPairs++;
            totalCorrect++;
        }
        else
        {
            array[0].FlipCard(1f);
            array[1].FlipCard(1f);
            array[0].Restart();
            array[1].Restart();
            misses++;
            currentMisses++;
        }
        yield return new WaitForSeconds(2.5f);
        isChecking = false;
        if(correctPairs== cardsNumber / 2)
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
        StartCoroutine(NewLevel());
        if (remainingTime >= maxTime)
        {
            onGameOver.Invoke();
        }
        
    }
    private IEnumerator NewLevel()
    {
        isGenerating = true;
        if (currentLevel > 1)
        {
            int perfectTime = 4 * cardsNumber  + 3 * exchangeNumber;
            if (gameMode == mode.lineal)
            {
                if(3*currentMisses<=cardsNumber && levelTime <= perfectTime) // level up
                {
                    currentLevel++;
                    if (cardsNumber % 4 == 0 && !exchangeAdded)
                    {
                        exchangeAdded = true;
                        exchangeNumber++;
                    }
                    else
                    {
                        exchangeAdded = false;
                        if (cardsNumber < 22) cardsNumber += 2;
                        else exchangeNumber++;
                    }

                }
                else if(3*currentMisses>=2*cardsNumber && levelTime>=perfectTime+10) //level down
                {
                    currentLevel--;
                    if (currentLevel < 1) currentLevel = 1;

                    if (cardsNumber % 4 == 0 && exchangeAdded)
                    {
                        exchangeAdded = false;
                        exchangeNumber--;
                    }
                    else
                    {
                        exchangeAdded = true;
                        if (cardsNumber > 6) cardsNumber -= 2;
                        else exchangeNumber--;

                        if (exchangeNumber < 0) exchangeNumber = 0;
                        if (cardsNumber < 6) cardsNumber = 6;
                    }

                }
                
            }
            else if (gameMode == mode.exponential)
            {
                if (3 * currentMisses <= cardsNumber && levelTime <= perfectTime) // level up
                {
                    currentLevel++;
                    int modify = (int)Mathf.Pow(1.5f, currentLevel- 1);
                    exchangeNumber = (int)Mathf.Log(modify, 2);
                    if(currentLevel==7) exchangeNumber++;
                    cardsNumber = (modify - exchangeNumber) * 2 + 4;
                    if (cardsNumber > 22)
                    {
                        cardsNumber = 22;
                        exchangeNumber = modify - 9;
                    }
                }
                else if (3 * currentMisses >= 2 * cardsNumber && levelTime >= perfectTime + 10) //level down
                {
                    currentLevel--;
                    int modify = (int)Mathf.Pow(1.5f, currentLevel- 1);
                    exchangeNumber = (int)Mathf.Log(modify, 2);
                    if (currentLevel == 7) exchangeNumber++;
                    cardsNumber = (modify - exchangeNumber) * 2 + 4;
                    if (cardsNumber < 4)
                    {
                        cardsNumber = 4;
                        exchangeNumber = 0;
                    }
                }
                
            }
        }
        else
        {
            if (!newGame)
            {
                currentLevel++;
                cardsNumber += 2;
            }
            else
            {
                newGame = false;
            }
        }
        currentMisses = 0;
        levelTime = 0f;
        yield return new WaitForSeconds(0.5f);
        newlevelText.text = "NIVEL " + currentLevel;       
        yield return new WaitForSeconds(1f);
        newlevelText.text = "";
        yield return new WaitForSeconds(0.5f);
        cardSpawner.Spawner(cardsNumber);
        cardSpawner.Exchange(exchangeNumber);
        yield return new WaitForSeconds(4f + 2f * exchangeNumber);
        isGenerating=false;
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
    public void EndGame()
    {
        StartCoroutine(EndGameCoroutine());
    }
    private IEnumerator EndGameCoroutine()
    {
        yield return new WaitForSeconds(1f);
        gamePanel.SetActive(false);
        tutorialPanel.SetActive(false);
        gameOverPanel.SetActive(false);
        endGamePanel.SetActive(true);
        gameOverTimeText.text = TimeFormat(remainingTime);

        string data = $@"
            ""clicks"":{clicks},
            ""level"":{currentLevel},
            ""correct_pairs"":{totalCorrect},
            ""wrong_pairs"":{misses}
        }}";
        //FormManager.Instance.SetGameData(data);

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
    
    public bool GetIsGenerating()
    {
        return isGenerating;
    }

    public bool GetIsChecking()
    {
        return isChecking;
    }
}

