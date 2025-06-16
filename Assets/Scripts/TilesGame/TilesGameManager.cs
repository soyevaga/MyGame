using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using System.Collections;
public class TilesGameManager : GameManager
{
    public static TilesGameManager Instance { get; private set; }

    [SerializeField] private AudioSource main;
    [SerializeField] private AudioClip backgroundMusic;
    [SerializeField] private AudioClip countdown;
    [SerializeField] private AudioClip gameOver;

    [SerializeField] private TextMeshProUGUI countdownText;
    [SerializeField] private TextMeshProUGUI pointsText;
    [SerializeField] private TextMeshProUGUI timeText;
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private TextMeshProUGUI gameOverTimeText;
    [SerializeField] private TextMeshProUGUI gameOverRecordText;
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private GameObject gamePanel;
    [SerializeField] private GameObject endGamePanel;
    [SerializeField] private GameObject tutorialPanel;
    [SerializeField] private GameObject goalsInfo;
    [SerializeField] private GameObject newRecord;
    [SerializeField] private GameObject speed;
    [SerializeField] private BotSpawner botSpawner;
    [SerializeField] private Slider speedSlider;
    [SerializeField] private Button pauseButton;
    [SerializeField] private Sprite pauseSprite;
    [SerializeField] private Sprite playSprite;
    [SerializeField] private Image tutorialImage;
    [SerializeField] private Sprite[] tutorialImages;
    [SerializeField] private TileButton[] buttons;
    [SerializeField] public UnityEvent onRestartLevel;
    [SerializeField] public UnityEvent onNextLevel;
    [SerializeField] public UnityEvent onGameOver;
    private mode gameMode;
    private float remainingTime;
    private float deadTime;
    private int currentLevel;
    private int currentSaved;
    private int currentDead;
    private int currentTutorialImage;
    private bool isGameOver;
    private int[] botTypesPerLevel = {1, 1, 1, 2, 2, 3, 4, 4};
    private float speedScale;
    private int restartCounter;
    private int clicks;
    private int totalRestarts;
    private bool isPaused;
    public class TilesJSON
    {
        public string userID;
        public string difficulty;
        public int orden;
        public string time;
        public int clicks;
        public int level;
        public int restarts;
        public int arrows_deleted;
    }
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
        isPaused = false;
        clicks = 0;
        totalRestarts = 0;
        string type = "Type" + PlayerPrefs.GetInt("CurrentGameNumber");
        if (PlayerPrefs.GetString(type) == "Lineal") gameMode = mode.lineal;
        else gameMode = mode.exponential;
        if (!PlayerPrefs.HasKey(username + "tiles"))
        {
            PlayerPrefs.SetFloat(username + "tiles", float.MaxValue);
            PlayerPrefs.Save();
        }
        isGameOver = false;
        currentLevel = 0;
        remainingTime = 0f;
        GridManager.Instance.GenerateGrid();
        AssignButtons();
        TutorialButton();
    }
    void Update()
    {
        if (!isGameOver)
        {
            remainingTime += Time.deltaTime;
            timeText.text = TimeFormat(remainingTime);
            if (currentSaved == botTypesPerLevel[currentLevel/2])
            {
                onNextLevel.Invoke();
            }
            else if (currentSaved + currentDead == botTypesPerLevel[currentLevel/2])
            {
                if (deadTime <= 0)
                {
                    onRestartLevel.Invoke();
                }
                else
                {
                    deadTime -= Time.deltaTime;
                }
            }
            if (Input.GetMouseButtonDown(0))
            {
                clicks++;
                Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                TileButton button = ToggledButton();
                int id = -1;
                if (button != null)
                {
                    id = button.GetID();
                }
                int change = GridManager.Instance.ChangeTile(mouseWorldPos, id);
                if(button!=null) button.SetNumber(change);
            }
            levelText.text = "Nivel " + (currentLevel/2 + 1);
            pointsText.text = "Salvados: " + currentSaved;
        }
    }
    public void nextLevel()
    {
        DBManager.Instance.GenerateGameJSON(GenerateJSON());
        //currentLevel/2 + 1 is the real level
        if (gameMode==mode.lineal)
        {
            if (restartCounter <= (currentLevel / 2)) // <=level-1 level up
            {
                currentLevel += 2;
            }
            else if (restartCounter >= (currentLevel / 2 + 3)) // >=level+2 level down
            {
                currentLevel -= 2;
                if(currentLevel<0) currentLevel = 0;
            }
            else //alternate with same level map
            {
                if(currentLevel%2==0) currentLevel++;
                else currentLevel--;
            }
        }
        else
        {
            if (restartCounter <= (currentLevel / 2)) // <=level-1 level up
            {
                if (currentLevel / 2 == 4) currentLevel += 2;
                else currentLevel += 4;
            }
            else if (restartCounter >= (currentLevel / 2 + 3)) // >=level+2 level down
            {
                if (currentLevel / 2 == 4) currentLevel -= 2;
                else currentLevel -= 4;

                if (currentLevel < 0) currentLevel = 0;
            }
            else //alternate with same level map
            {
                if (currentLevel % 2 == 0) currentLevel++;
                else currentLevel--;
            }
        }
        if (currentLevel/2 >= botTypesPerLevel.Length || remainingTime>=maxTime)
        {
            onGameOver.Invoke();
        }
        else
        {
            restartCounter = 0;
            GridManager.Instance.GenerateGrid();
            AssignButtons();
            StartCoroutine(InstantiateGame());
        }
    }

    public void RestartLevel()
    {
        botSpawner.PauseAllBots(true);
        restartCounter++;
        totalRestarts++;

        StartCoroutine(InstantiateGame());
    }
    public IEnumerator InstantiateGame()
    {
        SetSpeedScale();
        currentSaved = 0;
        currentDead = 0;
        deadTime = 1f;
        if(isPaused)
        {
            isPaused = false;
            pauseButton.GetComponent<Image>().sprite = pauseSprite;
            Time.timeScale = 1;
        }
        main.clip = countdown;
        main.loop = false;
        main.Play();
        countdownText.text = "3";
        yield return new WaitForSeconds(1f);
        countdownText.text = "2";
        yield return new WaitForSeconds(1f);
        countdownText.text = "1";
        yield return new WaitForSeconds(1f);
        countdownText.text = "";
        yield return new WaitForSeconds(0.5f);
        main.clip = backgroundMusic;
        main.loop = true;
        main.Play();
        Time.timeScale = 1f;

        botSpawner.Spawner(botTypesPerLevel[currentLevel/2]);
    }
    public void GameOver()
    {
        Time.timeScale = 0f;
        main.clip = gameOver;
        main.loop = false;
        main.Play();
        isGameOver = true;
        gamePanel.SetActive(false);
        tutorialPanel.SetActive(false);
        if (remainingTime < PlayerPrefs.GetFloat(username+"tiles"))
        {
            PlayerPrefs.SetFloat(username+"tiles", remainingTime);
            PlayerPrefs.Save();
            newRecord.SetActive(true);
        }
        else
        {
            newRecord.SetActive(false);
        }
        gameOverRecordText.text= TimeFormat(PlayerPrefs.GetFloat(username+"tiles"));
        gameOverTimeText.text = TimeFormat(remainingTime);
        gameOverPanel.SetActive(true);
    }
    public void EndGame()
    {
        Time.timeScale = 0f;
        main.clip = gameOver;
        main.loop = false;
        main.Play();
        isGameOver = true;
        gamePanel.SetActive(false);
        tutorialPanel.SetActive(false);
        if (remainingTime < PlayerPrefs.GetFloat(username + "tiles"))
        {
            PlayerPrefs.SetFloat(username + "tiles", remainingTime);
            PlayerPrefs.Save();
            newRecord.SetActive(true);
        }
        else
        {
            newRecord.SetActive(false);
        }
        gameOverRecordText.text = TimeFormat(PlayerPrefs.GetFloat(username + "tiles"));
        gameOverTimeText.text = TimeFormat(remainingTime);
        gameOverPanel.SetActive(false);
        endGamePanel.SetActive(true);
    }
    public void ReplayButton()
    {
        SceneManager.LoadScene("TilesScene");
    }

    public void TutorialButton()
    {
        Time.timeScale = 0f;
        botSpawner.PauseAllBots(true);
        currentTutorialImage = 0;
        tutorialImage.sprite= tutorialImages[currentTutorialImage];
        tutorialPanel.SetActive(true);
        gameOverPanel.SetActive(false);
        gamePanel.SetActive(false);
    }

    public void ExitTutorialButton()
    {
        Time.timeScale = 1f;
        StartCoroutine(InstantiateGame());
        botSpawner.PauseAllBots(false);
        currentTutorialImage = 0;
        tutorialImage.sprite = tutorialImages[currentTutorialImage];
        tutorialPanel.SetActive(false);
        gameOverPanel.SetActive(false);
        gamePanel.SetActive(true);
    }
    public TileButton ToggledButton()
    {
        TileButton toggled = null;
        foreach (TileButton button in buttons)
        {
            if (button.IsToggled())
            {
                toggled = button;   
                break;
            }
        }
        return toggled;
    }

    public void AssignButtons()
    {
        Map[] maps = GridManager.Instance.GetMaps(); 
        int[] arrows = maps[currentLevel].GetArrows();
        int id = 0;
        foreach(TileButton button in buttons)
        {
            button.gameObject.SetActive(true);
            button.SetID(id);
            button.SetNumberOnce(arrows[id]);
            id++;
        }
    }
    public int GetCurrentLevel()
    {
        return currentLevel;
    }
    public void ChangeButtonNumber(int index, int change)
    {
        buttons[index].SetNumber(change);
    }
    public int GetButtonNumber(int index)
    {
        return buttons[index].GetNumber();
    }
    public void BotInGoal(Bot bot)
    {
        currentSaved++;
        botSpawner.DeleteBot(bot);
    }

    public void AddDead()
    {
        currentDead++;
    }

    public void SetSpeedScale()
    {
        int sliderValue = (int)speedSlider.value;
        if (sliderValue == 0) speedScale = 0.5f;
        else if (sliderValue == 2) speedScale = 2f;
        else speedScale = 1f;
    }
    public float GetSpeedScale()
    {
        return speedScale;
    }
    public void TutorialLeftButton()
    {
        currentTutorialImage--;
        if(currentTutorialImage < 0) currentTutorialImage = tutorialImages.Length-1;
        tutorialImage.sprite = tutorialImages[currentTutorialImage];
    }

    public void TutorialRightButton()
    {
        currentTutorialImage++;
        if (currentTutorialImage > tutorialImages.Length - 1) currentTutorialImage = 0;
        tutorialImage.sprite = tutorialImages[currentTutorialImage];
    }
    public void PauseButton()
    {
        if (isPaused)
        {
            pauseButton.GetComponent<Image>().sprite = pauseSprite;
            isPaused = false;
        }
        else
        {
            pauseButton.GetComponent<Image>().sprite = playSprite;
            isPaused=true;
        }
    }
    private string GenerateJSON()
    {
        string time = DateTime.Now.ToString("HH:mm:ss");
        string difficulty = (gameMode == mode.lineal) ? "Lineal" : "Exponencial";
        TilesJSON data = new TilesJSON
        {
            userID = PlayerPrefs.GetString("username"),
            difficulty = difficulty,
            orden = PlayerPrefs.GetInt("CurrentGameNumber"),
            time = time,
            clicks = clicks,
            level = currentLevel / 2 + 1,
            restarts = totalRestarts,
            arrows_deleted = GridManager.Instance.GetArrowsDeleted()
        };

        return JsonUtility.ToJson(data);
    }
    public bool GetPaused()
    {
        return isPaused;
    }
}
