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
    [SerializeField] private GameObject goalsInfo;
    [SerializeField] private GameObject newRecord;
    [SerializeField] private GameObject speed;
    [SerializeField] private BotSpawner botSpawner;
    [SerializeField] private Slider speedSlider;
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
    private float[] timeScales = { 0.5f, 1f, 2f };
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
        if (!PlayerPrefs.HasKey(username + "tiles"))
        {
            PlayerPrefs.SetFloat(username + "tiles", float.MaxValue);
            PlayerPrefs.Save();
        }
        isGameOver = false;
        Time.timeScale = 1f;
        currentLevel = 7;
        InstantiateGame();
    }
    void Update()
    {
        if (!isGameOver)
        {
            remainingTime += Time.deltaTime;
            timeText.text = TimeFormat(remainingTime);
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
            levelText.text = "Nivel " + (currentLevel + 1);
            pointsText.text = "Salvados: " + currentSaved;
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
        speed.SetActive(false);
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

    private string TimeFormat(float time)
    {
        int minutes = Mathf.FloorToInt(time / 60);
        int seconds = Mathf.FloorToInt(time % 60);
        return string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    public void SetSpeed()
    {
        Time.timeScale = timeScales[(int)speedSlider.value];
    }
}
