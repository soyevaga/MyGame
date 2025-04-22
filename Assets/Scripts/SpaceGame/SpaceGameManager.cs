using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using System.Collections;
public class SpaceGameManager : GameManager
{
    public static SpaceGameManager Instance { get; private set; }

    [SerializeField] private MeteorSpawner meteorSpawner;
    [SerializeField] private TextMeshProUGUI pointsText;
    [SerializeField] private TextMeshProUGUI objectiveText;
    [SerializeField] private TextMeshProUGUI recordText;
    [SerializeField] private TextMeshProUGUI punctuationText;
    [SerializeField] private TextMeshProUGUI timeText;
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private GameObject gamePanel;
    [SerializeField] private GameObject tutorialPanel;
    [SerializeField] private GameObject endGamePanel;
    [SerializeField] private GameObject explosionPrefab = null;
    [SerializeField] public UnityEvent onGameOver;
    private mode gameMode;
    private int currentPoints; 
    private int recordPoints;
    private float remainingTime;
    private float levelTime;
    private int currentKills;
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
        Time.timeScale = 1f;
        gameMode = mode.lineal;
        meteorSpawner.InitialSpeed(gameMode);
        currentPoints = 0;
        currentKills = 0;
        if (!PlayerPrefs.HasKey(username+"space"))
        {
            PlayerPrefs.SetInt(username+"space", 0);
            PlayerPrefs.Save();
        }
        recordPoints = PlayerPrefs.GetInt(username + "space");
        pointsText.text = "Puntos: " + currentPoints + "\n" + "Record: " + recordPoints;        
        objectiveText.text = "¡Dispara los numeros impares!";        
        remainingTime = 180f;
        TutorialButton();
    }
    void Update()
    {
        remainingTime-= Time.deltaTime;
        levelTime += Time.deltaTime;
        if (levelTime>=20f)
        {
            levelTime = 0f;
            checkLevel();
        }
        if (remainingTime <= 0)
        {
            onGameOver.Invoke();
        }
        timeText.text = TimeFormat(remainingTime);
        pointsText.text = "Puntos: " + currentPoints + "\n" + "Record: " + recordPoints;
    }

    public void ModifyPoints(int points)
    {
        if (points == 1) currentKills++;
        currentPoints += points;
        if (currentPoints < 0)
        {
            currentPoints = 0;
        }
    }

    public void gameOverAction()
    {
        Time.timeScale = 0f;
        if (currentPoints > PlayerPrefs.GetInt(username + "space"))
        {
            PlayerPrefs.SetInt(username + "space", currentPoints);
            PlayerPrefs.Save();
            recordPoints = currentPoints;
            recordText.text = "¡NUEVO RECORD!";
        }
        gamePanel.SetActive(false); 
        tutorialPanel.SetActive(false);
        punctuationText.text = "Puntos: " + currentPoints + "\n" + "Record: " + recordPoints;
        gameOverPanel.SetActive(true);
    }
    public void endGameAction()
    {
        Time.timeScale = 0f;
        if (currentPoints > PlayerPrefs.GetInt(username + "space"))
        {
            PlayerPrefs.SetInt(username + "space", currentPoints);
            PlayerPrefs.Save();
            recordPoints = currentPoints;
            recordText.text = "¡NUEVO RECORD!";
        }
        gamePanel.SetActive(false);
        tutorialPanel.SetActive(false);
        punctuationText.text = "Puntos: " + currentPoints + "\n" + "Record: " + recordPoints;
        gameOverPanel.SetActive(false);
        endGamePanel.SetActive(true);
    }
    public void DestroyPlayer(GameObject player)
    {
        GameObject explosion = Instantiate(explosionPrefab);
        explosion.transform.position = player.transform.position;
        player.SetActive(false);
        ModifyPoints(-3);
        StartCoroutine(DestroyPlayerSequence(explosion, player));
    }
    private IEnumerator DestroyPlayerSequence(GameObject explosion, GameObject player)
    {
        yield return new WaitForSeconds(1f);
        explosion.SetActive(false);
        yield return new WaitForSeconds(0.2f);
        player.SetActive(true);
    }

    private void checkLevel()
    {
        int goal = meteorSpawner.GetMeteorSize()/2;
        float percentage = (float)currentKills / goal;
        if (percentage <= 0.3)
        {
            meteorSpawner.SetMeteorSpeed(-1);
            
        }
        else if (percentage >= 0.8)
        {
            meteorSpawner.SetMeteorSpeed(1);
        }
        currentKills = 0;
    }
    
    public void ReplayButton()
    {
        SceneManager.LoadScene("SpaceScene");
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
    public mode GetGameMode()
    {
        return gameMode;
    }
}
