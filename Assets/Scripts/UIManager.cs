using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [SerializeField] private TMP_InputField ageInput;
    [SerializeField] private TextMeshProUGUI warningText;
    [SerializeField] private TMP_Dropdown birthdayDropdown;
    [SerializeField] private TMP_Dropdown genderDropdown;
    [SerializeField] private GameObject endPanel;
    [SerializeField] private GameObject gamesPanel;
    [SerializeField] private GameObject menuPanel;
    private string username;
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
    private void Start()
    {
        username = PlayerPrefs.GetString("username");
        if (PlayerPrefs.HasKey("end") && PlayerPrefs.GetInt("end") == 1)
        {
            PlayerPrefs.SetInt("end", 0);
            PlayerPrefs.Save();
            ShowEnd();
        }
        else
        {
            ShowMenu(); 
        }
    }
    public void GameScene()
    {
        if (string.IsNullOrEmpty(ageInput.text))
        {
            warningText.text = "Debes completar todos los campos";
        }
        else
        {
            if (int.TryParse(ageInput.text, out int myYear) && myYear >= 1 && myYear <= 101)
            {
                username = DateTime.Now.ToString("ddMMyyyyHHmmss");
                PlayerPrefs.SetString("username", username);
                string gender = genderDropdown.options[genderDropdown.value].text;

                string[] games = { "SpaceScene", "CardsScene", "TilesScene" };
                // Random asignation
                for (int i = games.Length - 1; i > 0; i--)
                {
                    int j = UnityEngine.Random.Range(0, i + 1);
                    string temp = games[i];
                    games[i] = games[j];
                    games[j] = temp;
                }
                PlayerPrefs.SetString("Game1", games[0]);
                PlayerPrefs.SetString("Game2", games[1]);
                PlayerPrefs.SetString("Game3", games[2]);

                if (birthdayDropdown.value == 0) // Even
                {
                    PlayerPrefs.SetString("birthday", "Par");
                    PlayerPrefs.SetString("Type1", "Lineal");
                    PlayerPrefs.SetString("Type2", "Exponencial");
                    PlayerPrefs.SetString("Type3", "Lineal");
                }
                else // Odd
                {
                    PlayerPrefs.SetString("birthday", "Impar");
                    PlayerPrefs.SetString("Type1", "Exponencial");
                    PlayerPrefs.SetString("Type2", "Lineal");
                    PlayerPrefs.SetString("Type3", "Exponencial");
                }
                PlayerPrefs.SetInt("CurrentGameNumber", 1);
                PlayerPrefs.SetInt("end", 0);
                PlayerPrefs.Save();
                DBManager.Instance.GenerateUserJSON(gender, myYear);
                SceneManager.LoadScene(PlayerPrefs.GetString("Game1"));
            }
        }
        
    }

    public void SpaceGame()
    {
        SceneManager.LoadScene("SpaceScene");
    }

    public void CardsGame()
    {
        SceneManager.LoadScene("CardsScene");
    }

    public void TilesGame()
    {
        SceneManager.LoadScene("TilesScene");
    }


    public void ShowMenu()
    {
        menuPanel.SetActive(true);
        endPanel.SetActive(false);
        gamesPanel.SetActive(false);
    }

    public void ShowEnd()
    {
        menuPanel.SetActive(false);
        endPanel.SetActive(true);
        gamesPanel.SetActive(false);
    }

    public void QuitButton()
    {
        Application.Quit();
    }
    public void ValidateAge(string input)
    {
        if (int.TryParse(input, out int myAge))
        {
            if (myAge >= 1 && myAge <= 101)
            {
                warningText.text = "";
            }
            else
            {
                warningText.text = "Introduce una edad válida";
            }
        }
        else
        {
            warningText.text = "Introduce una edad válida";
            ageInput.text = "";
        }
    }

    
}
