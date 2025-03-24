using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using System.Collections;
public class SpaceGameManager : GameManager
{
    public static SpaceGameManager Instance { get; private set; }

    public enum mode
    {
        par,
        impar
    }

    [SerializeField] private MeteorSpawner meteorSpawner;
    [SerializeField] private TextMeshProUGUI pointsText;
    [SerializeField] private TextMeshProUGUI objectiveText;
    [SerializeField] private TextMeshProUGUI recordText;
    [SerializeField] private TextMeshProUGUI punctuationText;
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private GameObject explosionPrefab = null;
    private int currentPoints; 
    private int recordPoints;
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

    new void Start()
    {
        base.Start();
        Time.timeScale = 1f;
        currentPoints = 0;
        if (!PlayerPrefs.HasKey(username+"space"))
        {
            PlayerPrefs.SetInt(username+"space", 0);
            PlayerPrefs.Save();
        }
        recordPoints = PlayerPrefs.GetInt(username + "space");
        pointsText.text = "Score: " + currentPoints + "\n" + "High Score: " + recordPoints;
        if (gameMode == mode.par)
        {
            objectiveText.text = "Shoot the even numbers!";
        }
        else if (gameMode == mode.impar)
        {
            objectiveText.text = "Shoot the odd numbers!";
        }

    }
    void Update()
    {
        pointsText.text = "Score: " + currentPoints + "\n" + "High Score: " + recordPoints;
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
        if (currentPoints > PlayerPrefs.GetInt(username + "space"))
        {
            PlayerPrefs.SetInt(username + "space", currentPoints);
            PlayerPrefs.Save();
            recordPoints = currentPoints;
            recordText.text = "NEW RECORD!";
        }
        pointsText.gameObject.SetActive(false);
        usernameText.gameObject.SetActive(false);
        objectiveText.gameObject.SetActive(false);
        punctuationText.text = "Score: " + currentPoints + "\n" + "High Score: " + recordPoints;
        gameOverPanel.SetActive(true);
    }

    public void DestroyPlayer(GameObject player)
    {
        GameObject explosion = Instantiate(explosionPrefab);
        explosion.transform.position = player.transform.position;
        player.SetActive(false);
        StartCoroutine(GameOverSequence(explosion));
    }
    private IEnumerator GameOverSequence(GameObject explosion)
    {
        yield return new WaitForSeconds(1f);
        onGameOver.Invoke();
        explosion.SetActive(false);
    }
    
    public void ReplayButton()
    {
        SceneManager.LoadScene("SpaceScene");
    }

    public mode GameMode()
    {
        return gameMode;
    }
}
