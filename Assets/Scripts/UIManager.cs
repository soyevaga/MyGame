﻿using TMPro;
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
        if (PlayerPrefs.GetInt("ShowGamesPanel", 0) == 1)
        {
            PlayerPrefs.SetInt("ShowGamesPanel", 0); 
            PlayerPrefs.Save();
            ShowGames();
        }
        else
        {
            ShowMenu(); 
        }
    }
    public void GameScene()
    {
        if (string.IsNullOrEmpty(usernameInput.text))
        {
            warningText.text = "Debes introducir un usuario";
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
        settingsPanel.SetActive(false);
        gamesPanel.SetActive(false);
    }

    public void ShowGames()
    {
        menuPanel.SetActive(false);
        settingsPanel.SetActive(false);
        gamesPanel.SetActive(true);
    }

    public void QuitButton()
    {
        Application.Quit();
    }

}
