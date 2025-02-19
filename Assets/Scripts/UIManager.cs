using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
public class UIManager : MonoBehaviour
{
    [SerializeField] private TMP_InputField usernameInput;
    [SerializeField] private TextMeshProUGUI warningText;
    public void GameScene()
    {
        if (string.IsNullOrEmpty(usernameInput.text))
        {
            warningText.text = "You must introduce a username";
        }
        else
        {
            PlayerPrefs.SetString("username", usernameInput.text); 
            PlayerPrefs.Save();  
            SceneManager.LoadScene("GameScene");
        }   
    }

    public void SettingsScene()
    {
        SceneManager.LoadScene("SettingsScene");
    }
}
