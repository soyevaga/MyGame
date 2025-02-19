using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public enum mode
    {
        par,
        impar
    }

    [SerializeField] private MeteorSpawner meteorSpawner;
    [SerializeField] private TextMeshProUGUI pointsText;
    [SerializeField] private TextMeshProUGUI objectiveText;
    [SerializeField] private TextMeshProUGUI usernameText;
    [SerializeField] private TextMeshProUGUI recordText;
    [SerializeField] private GameObject gameOverPanel;
    private int currentPoints; 
    private int recordPoints;
    private string username;
    [SerializeField] private mode gameMode;
    [SerializeField] public UnityEvent onGameOver;
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
        Time.timeScale = 1f;
        currentPoints = 0;
        username = PlayerPrefs.GetString("username");
        if (!PlayerPrefs.HasKey(username))
        {
            PlayerPrefs.SetInt(username, 0);
        }
        recordPoints = PlayerPrefs.GetInt(username);
        pointsText.text = "Puntos: " + currentPoints + "\n" + "RECORD: " + recordPoints;
        if (gameMode == mode.par)
        {
            objectiveText.text = "¡Dispara a los pares!";
        }
        else if (gameMode == mode.impar)
        {
            objectiveText.text = "¡Dispara a los impares!";
        }

        usernameText.text = username;
    }
    void Update()
    {
        pointsText.text = "Puntos: " + currentPoints + "\n" + "RECORD: " + recordPoints;
    }

    public void ModifyPoints(int points)
    {
        currentPoints += points;
        if (currentPoints < 0)
        {
            currentPoints = 0;
        }
    }

    public void gameOverAction()
    {
        Time.timeScale = 0f;
        if(currentPoints> PlayerPrefs.GetInt(username))
        {
            PlayerPrefs.SetInt(username, currentPoints);
            recordText.text= "NEW RECORD: "+currentPoints;
        }
        gameOverPanel.SetActive(true); 
    }

    public void MenuButton()
    {
        SceneManager.LoadScene("MenuScene");
    }
    public void ReplayButton()
    {
        SceneManager.LoadScene("GameScene");
    }

    public mode GameMode()
    {
        return gameMode;
    }
}
