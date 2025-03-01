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
}
