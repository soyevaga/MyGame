using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;
public class TilesGameManager : GameManager
{
    public static TilesGameManager Instance { get; private set; }

    [SerializeField] private TextMeshProUGUI pointsText;
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private BotSpawner botSpawner;
    [SerializeField] private Button[] buttons;
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
        AssignButtonIDs();
        GridManager.Instance.GenerateGrid(1);
        botSpawner.Spawner(4,1);
    }
    void Update()
    {
        if (Input.GetMouseButtonDown(0)) 
        {
            Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Button button = ToggledButton();
            if (button != null)
            {
                GridManager.Instance.ChangeTile(mouseWorldPos, button.GetID());
            }
            
        }
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

    public void AssignButtonIDs()
    {
        int id = 0;
        foreach(Button button in buttons)
        {
            button.SetID(id);
            id++;
        }
    }
}
