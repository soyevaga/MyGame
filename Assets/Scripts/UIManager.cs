using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [SerializeField] private TMP_InputField usernameInput;
    [SerializeField] private TextMeshProUGUI warningText;
    [SerializeField] private GameObject settingsPanel;
    [SerializeField] private GameObject gamesPanel;
    [SerializeField] private GameObject menuPanel;
    [SerializeField] private TextMeshProUGUI welcomeText;
    private string username;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void GameScene()
    {
        if (string.IsNullOrEmpty(usernameInput.text))
        {
            warningText.text = "You must introduce a username";
        }
        else
        {
            username = usernameInput.text;
            PlayerPrefs.SetString("username",username); 
            PlayerPrefs.Save();         
            ShowGames();
        }   
    }

    public void Settings()
    {
        settingsPanel.SetActive(true);
    }

    public void SpaceGame()
    {
        SceneManager.LoadScene("SpaceScene");
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene("MenuScene");
    }

    public void ShowGames()
    {
        menuPanel.SetActive(false);
        settingsPanel.SetActive(false);
        welcomeText.text = "Welcome, " + username + "!";
        gamesPanel.SetActive(true);
    }

}
