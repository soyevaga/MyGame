using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class TilesGameManager : GameManager
{
    public static TilesGameManager Instance { get; private set; }

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
    [SerializeField] private Image tutorialImage;
    [SerializeField] private Sprite[] tutorialImages;
    [SerializeField] private Button[] buttons;
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
        string type = "Type" + PlayerPrefs.GetInt("CurrentGameNumber");
        if (PlayerPrefs.GetString(type) == "Lineal") gameMode = mode.lineal;
        else gameMode = mode.exponential;
        if (!PlayerPrefs.HasKey(username + "tiles"))
        {
            PlayerPrefs.SetFloat(username + "tiles", float.MaxValue);
            PlayerPrefs.Save();
        }
        isGameOver = false;
        Time.timeScale = 1f;
        currentLevel = 0;
        InstantiateGame();
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
                Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Button button = ToggledButton();
                if (button != null)
                {
                    int change = GridManager.Instance.ChangeTile(mouseWorldPos, button.GetID());
                    button.SetNumber(change);
                }

            }
            levelText.text = "Nivel " + (currentLevel/2 + 1);
            pointsText.text = "Salvados: " + currentSaved;
        }
    }
    public void nextLevel()
    {
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
            InstantiateGame();
        }
    }

    public void RestartLevel()
    {
        restartCounter++;
        InstantiateGame();
    }
    public void InstantiateGame()
    {
        AssignButtons();
        SetSpeedScale();
        GridManager.Instance.GenerateGrid();
        botSpawner.Spawner(botTypesPerLevel[currentLevel/2]);
        currentSaved = 0;
        currentDead = 0;
        deadTime = 3f;
    }
    public void GameOver()
    {
        Time.timeScale = 0f;
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
        botSpawner.PauseAllBots(false);
        currentTutorialImage = 0;
        tutorialImage.sprite = tutorialImages[currentTutorialImage];
        tutorialPanel.SetActive(false);
        gameOverPanel.SetActive(false);
        gamePanel.SetActive(true);
    }
    public Button ToggledButton()
    {
        Button toggled = null;
        foreach (Button button in buttons)
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
        foreach(Button button in buttons)
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
}
