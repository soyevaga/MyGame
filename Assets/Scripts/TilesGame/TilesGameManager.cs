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
    [SerializeField] private BotSpawner botSpawner;
    [SerializeField] public float totalTime = 300f;
    [SerializeField] private Button[] buttons;
    [SerializeField] public UnityEvent onRestartLevel;
    [SerializeField] public UnityEvent onNextLevel;
    [SerializeField] public UnityEvent onGameOver;
    private float remainingTime;
    private int currentLevel;
    private int currentWins;
    private int[] botTypesPerLevel = { 1, 1, 1, 2, 2, 3, 4, 4};
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
        base.Start();
        remainingTime = totalTime;
        Time.timeScale = 1f;
        currentLevel = 3;
        InstantiateGame();
    }
    void Update()
    {
        remainingTime -= Time.deltaTime;
        if (remainingTime <= 0)
        {
            onGameOver.Invoke();
        }
        int minutes = Mathf.FloorToInt(remainingTime / 60);
        int seconds = Mathf.FloorToInt(remainingTime % 60);
        timeText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        if (currentWins == botTypesPerLevel[currentLevel])
        {
            onNextLevel.Invoke();
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
        levelText.text = "Level: " + (currentLevel+1);
        pointsText.text = "Saved: " + currentWins;
    }
    public void nextLevel()
    {
        currentLevel++;
        InstantiateGame();
    }
    public void restartLevel()
    {
        InstantiateGame();
    }
    private void InstantiateGame()
    {
        AssignButtons();
        GridManager.Instance.GenerateGrid();
        botSpawner.DeleteAllBots();
        botSpawner.Spawner(botTypesPerLevel[currentLevel]);
        currentWins = 0;

    }
    public void GameOver()
    {
        Time.timeScale = 0f;
        timeText.gameObject.SetActive(false);
        pointsText.gameObject.SetActive(false);
        usernameText.gameObject.SetActive(false);
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
        currentWins++;
        botSpawner.DeleteBot(bot);
    }

    public void BotOutOfBounds(Bot bot)
    {
        botSpawner.DeleteBot(bot);
        onRestartLevel.Invoke();
    }
}
