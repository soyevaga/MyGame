
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
    protected string gameData;
    protected void Start()
    {
        username = PlayerPrefs.GetString("username");
        maxTime = 180f;
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
        SceneManager.LoadScene("FormScene");
    }
}
