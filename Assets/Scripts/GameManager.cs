using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public abstract class GameManager : MonoBehaviour
{
    public enum mode
    {
        lineal,
        exponential
    }
    protected string username;
    protected float maxTime;
    protected void Start()
    {
        username = PlayerPrefs.GetString("username");
        maxTime = 10f;
    }
    public void MenuButton()
    {
        PlayerPrefs.SetInt("ShowGamesPanel", 1); 
        PlayerPrefs.Save();
        SceneManager.LoadScene("MenuScene");
    }
    public string TimeFormat(float time)
    {
        int minutes = Mathf.FloorToInt(time / 60);
        int seconds = Mathf.FloorToInt(time % 60);
        return string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    public void NextButton()
    {
        int currentGame = PlayerPrefs.GetInt("CurrentGameNumber") + 1;
        PlayerPrefs.SetInt("CurrentGameNumber", currentGame);
        PlayerPrefs.Save();
        SceneManager.LoadScene("FormScene");
    }
}
