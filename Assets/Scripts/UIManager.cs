using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [SerializeField] private AudioSource click;
    [SerializeField] private AudioSource main;
    [SerializeField] private TMP_InputField ageInput;
    [SerializeField] private TMP_InputField hoursInput;
    [SerializeField] private TextMeshProUGUI warningText;
    [SerializeField] private TMP_Dropdown birthdayDropdown;
    [SerializeField] private TMP_Dropdown genderDropdown;
    [SerializeField] private GameObject endPanel;
    [SerializeField] private GameObject gamesPanel;
    [SerializeField] private GameObject menuPanel;
    [SerializeField] private GameObject beginPanel;
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

    void Start()
    {
        username = PlayerPrefs.GetString("username");
        ShowBegin();
    }
    public void GameScene()
    {
        if (string.IsNullOrEmpty(ageInput.text) || string.IsNullOrEmpty(hoursInput.text))
        {
            warningText.text = "Debes completar todos los campos";
        }
        else
        {
            if (int.TryParse(ageInput.text, out int myYear) && myYear >= 1 && myYear <= 101 && int.TryParse(hoursInput.text, out int hours) && hours >= 0 && hours <= 150)
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
                    PlayerPrefs.SetString("Type2", "Lineal");
                    PlayerPrefs.SetString("Type3", "Lineal");
                }
                else // Odd
                {
                    PlayerPrefs.SetString("birthday", "Impar");
                    PlayerPrefs.SetString("Type1", "Exponencial");
                    PlayerPrefs.SetString("Type2", "Exponencial");
                    PlayerPrefs.SetString("Type3", "Exponencial");
                }
                
                if (PlayerPrefs.GetString("Game1") == "CardsScene")
                {
                    if (birthdayDropdown.value == 0) // Even
                    {
                        PlayerPrefs.SetString("Type1", "Exponencial");
                    }
                    else // Odd
                    {
                        PlayerPrefs.SetString("Type1", "Lineal");
                    }
                }
                else if (PlayerPrefs.GetString("Game2") == "CardsScene")
                {
                    if (birthdayDropdown.value == 0) // Even
                    {
                        PlayerPrefs.SetString("Type2", "Exponencial");
                    }
                    else // Odd
                    {
                        PlayerPrefs.SetString("Type2", "Lineal");
                    }
                }
                else
                {
                    if (birthdayDropdown.value == 0) // Even
                    {
                        PlayerPrefs.SetString("Type3", "Exponencial");
                    }
                    else // Odd
                    {
                        PlayerPrefs.SetString("Type3", "Lineal");
                    }
                }
                
                PlayerPrefs.SetInt("CurrentGameNumber", 1);
                DBManager.Instance.GenerateUserJSON(gender, myYear, hours);
                main.Pause();
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
        beginPanel.SetActive(false);
    }

    public void ShowEnd()
    {
        main.Play();
        menuPanel.SetActive(false);
        endPanel.SetActive(true);
        gamesPanel.SetActive(false); 
        beginPanel.SetActive(false);
    }
    public void ShowBegin()
    {
        main.Play();
        menuPanel.SetActive(false);
        endPanel.SetActive(false);
        gamesPanel.SetActive(false);
        beginPanel.SetActive(true);
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
    public void ValidateHours(string input)
    {
        if (int.TryParse(input, out int hours))
        {
            if (hours >= 0 && hours <= 150)
            {
                warningText.text = "";
            }
            else
            {
                warningText.text = "Introduce un número válido";
            }
        }
        else
        {
            warningText.text = "Introduce un número válido";
            hoursInput.text = "";
        }
    }
    public void Click()
    {
        click.Play();
    }
    public void Play()
    {
        main.Play();
    }
}
