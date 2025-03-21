using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
public class TilesGameManager : GameManager
{
    public static TilesGameManager Instance { get; private set; }

    [SerializeField] private TextMeshProUGUI pointsText;
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private BotSpawner botSpawner;
    [SerializeField] private Button[] buttons;
    [SerializeField] public UnityEvent onNextLevel;
    [SerializeField] public UnityEvent onGameOver;
    private int currentLevel=0;
    private int currentWins = 0;
    private int[] botTypesPerLevel = { 1, 1 };
    private int[] botNumPerLevel = { 1, 5 };
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
        Time.timeScale = 1f;
        AssignButtons();
        GridManager.Instance.GenerateGrid();
        botSpawner.Spawner(botTypesPerLevel[currentLevel], botNumPerLevel[currentLevel]);
        currentLevel = 0;
        currentWins = 0;
    }
    void Update()
    {
        if (currentWins == botTypesPerLevel[currentLevel]* botNumPerLevel[currentLevel])
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
    }
    public void nextLevel()
    {
        currentLevel++;
        AssignButtons();
        GridManager.Instance.GenerateGrid();
        botSpawner.Spawner(botTypesPerLevel[currentLevel], botNumPerLevel[currentLevel]);
        currentWins = 0;
    }
    public void GameOver()
    {
        Time.timeScale = 0f;
        pointsText.gameObject.SetActive(false);
        usernameText.gameObject.SetActive(false);
        gameOverPanel.SetActive(true);
    }
    public void ModifyPoints(int points)
    {
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
            button.gameObject.SetActive(true );
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
        onGameOver.Invoke();
    }
}
