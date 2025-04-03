using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public abstract class GameManager : MonoBehaviour
{
    [SerializeField] protected TextMeshProUGUI usernameText;
    protected string username;
    protected void Start()
    {
        username = PlayerPrefs.GetString("username");
        usernameText.text = username;
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

}
