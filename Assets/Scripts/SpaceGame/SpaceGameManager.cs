using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using System.Collections;
using System;
public class SpaceGameManager : GameManager
{
    public static SpaceGameManager Instance { get; private set; }

    [SerializeField] private AudioSource main;
    [SerializeField] private AudioSource explosionSound;
    [SerializeField] private AudioClip backgroundMusic;
    [SerializeField] private AudioClip gameOver;

    [SerializeField] private MeteorSpawner meteorSpawner;
    [SerializeField] private Player player;
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
    private int selfKills;
    private int oddKills;
    private int evenKills;

    public class SpaceJSON
    {
        public string userID;
        public string difficulty;
        public int orden;
        public string time;
        public int shoots;
        public int evens_killed;
        public int odds_killed;
        public float speed;
        public int self_killed;
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
        main.clip = backgroundMusic;
        main.loop = true;
        main.volume = 0.2f;
        main.Play();
        Time.timeScale = 1f;
        string type = "Type" + PlayerPrefs.GetInt("CurrentGameNumber");
        if (PlayerPrefs.GetString(type) == "Lineal") gameMode = mode.lineal;
        else gameMode = mode.exponential;
        meteorSpawner.InitialSpeed(gameMode);
        currentPoints = 0;
        currentKills = 0;
        selfKills = 0;
        oddKills = 0;
        evenKills = 0;
        if (!PlayerPrefs.HasKey(username+"space"))
        {
            PlayerPrefs.SetInt(username+"space", 0);
            PlayerPrefs.Save();
        }
        recordPoints = PlayerPrefs.GetInt(username + "space");
        pointsText.text = "Puntos: " + currentPoints + "\n" + "Record: " + recordPoints;        
        objectiveText.text = "¡Dispara los numeros impares!";        
        remainingTime = maxTime;
        TutorialButton();
    }
    void Update()
    {
        remainingTime-= Time.deltaTime;
        levelTime += Time.deltaTime;
        if (levelTime>=20f)
        {
            levelTime = 0f;
            if (remainingTime <= 0)
            {
                onGameOver.Invoke();
            }
            else
            {
                checkLevel();
            }
        }
        timeText.text = TimeFormat(remainingTime);
        pointsText.text = "Puntos: " + currentPoints + "\n" + "Record: " + recordPoints;
    }

    public void ModifyPoints(int points)
    {
        if (points == 1)
        {
            currentKills++;
            oddKills++;
        }
        else
        {
            evenKills++;
        }
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
        main.clip = gameOver;
        main.loop = false;
        main.volume = 1f;
        main.Play();
        DBManager.Instance.GenerateGameJSON(GenerateJSON());
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
        selfKills++;
        StartCoroutine(DestroyPlayerSequence(explosion, player));
    }
    private IEnumerator DestroyPlayerSequence(GameObject explosion, GameObject player)
    {
        explosionSound.Play();
        yield return new WaitForSeconds(0.8f);
        explosion.SetActive(false);
        player.SetActive(true);
    }

    private void checkLevel()
    {
        DBManager.Instance.GenerateGameJSON(GenerateJSON());
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
    private string GenerateJSON()
    {
        string time = DateTime.Now.ToString("HH:mm:ss");
        string difficulty = (gameMode == mode.lineal) ? "Lineal" : "Exponencial";
        SpaceJSON data = new SpaceJSON
        {
            userID = PlayerPrefs.GetString("username"),
            difficulty=difficulty, 
            orden= PlayerPrefs.GetInt("CurrentGameNumber"),
            time= time,
            shoots= player.GetShoots(),
            evens_killed= evenKills,
            odds_killed= oddKills,
            speed = meteorSpawner.GetMeteorSpeed(), 
            self_killed= selfKills
        };

        return JsonUtility.ToJson(data);
    }
}
