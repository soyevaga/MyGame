using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
public class TilesGameManager : GameManager
{
    public static TilesGameManager Instance { get; private set; }

    [SerializeField] private TextMeshProUGUI pointsText;
    [SerializeField] private TextMeshProUGUI timeText;
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private GameObject goalsInfo;
    [SerializeField] private BotSpawner botSpawner;
    [SerializeField] public float totalTime = 300f;
    [SerializeField] private Button[] buttons;
    [SerializeField] public UnityEvent onRestartLevel;
    [SerializeField] public UnityEvent onNextLevel;
    [SerializeField] public UnityEvent onGameOver;
    private float remainingTime;
    private float deadTime;
    private int currentLevel;
    private int currentSaved;
    private int currentDead;
    private bool isGameOver;
    private int[] botTypesPerLevel = {1, 1, 1, 2, 2, 3, 4, 4};
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
        isGameOver = false;
        remainingTime = totalTime;
        Time.timeScale = 1f;
        currentLevel = 7;
        InstantiateGame();
    }
    void Update()
    {
        if (!isGameOver)
        {
            remainingTime -= Time.deltaTime;
            if (remainingTime <= 0)
            {
                onGameOver.Invoke();
            }
            int minutes = Mathf.FloorToInt(remainingTime / 60);
            int seconds = Mathf.FloorToInt(remainingTime % 60);
            timeText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
            if (currentSaved == botTypesPerLevel[currentLevel])
            {
                onNextLevel.Invoke();
            }
            else if (currentSaved + currentDead == botTypesPerLevel[currentLevel])
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
            levelText.text = "Level: " + (currentLevel + 1);
            pointsText.text = "Saved: " + currentSaved;
        }
    }
    public void nextLevel()
    {
        currentLevel++;
        if (currentLevel >= botTypesPerLevel.Length)
        {
            onGameOver.Invoke();
        }
        else
        {
            InstantiateGame();
        }
    }
    public void InstantiateGame()
    {
        AssignButtons();
        GridManager.Instance.GenerateGrid();
        botSpawner.DeleteAllBots();
        botSpawner.Spawner(botTypesPerLevel[currentLevel]);
        currentSaved = 0;
        currentDead = 0;
        deadTime = 3f;
    }
    public void GameOver()
    {
        Time.timeScale = 0f;
        isGameOver = true;
        timeText.gameObject.SetActive(false);
        pointsText.gameObject.SetActive(false);
        usernameText.gameObject.SetActive(false);
        goalsInfo.SetActive(false);
        gameOverPanel.SetActive(true);
    }
    public void ReplayButton()
    {
        SceneManager.LoadScene("TilesScene");
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
}
