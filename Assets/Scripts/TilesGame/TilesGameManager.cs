using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
public class TilesGameManager : GameManager
{
    public static TilesGameManager Instance { get; private set; }

    [SerializeField] private GridManager gridManager;
    [SerializeField] private TextMeshProUGUI pointsText;
    [SerializeField] private GameObject gameOverPanel;
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
        base.Start();
        gridManager.GenerateGrid();
    }
    void Update()
    {
        pointsText.text = "";
    }

    public void ModifyPoints(int points)
    {
    }

    public void ReplayButton()
    {
        SceneManager.LoadScene("TilesScene");
    }
}
