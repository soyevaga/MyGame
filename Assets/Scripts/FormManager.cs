using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FormManager : MonoBehaviour
{
    public static FormManager Instance { get; private set; }

    [SerializeField] private TextMeshProUGUI warningText;
    [SerializeField] private ToggleGroup[] toggleGroups;
    [SerializeField] private GameObject panel1;
    [SerializeField] private GameObject panel2;
    private bool panel1TogglesSelected;
    private Toggle[] toggleSelected;
    private int gameNumber;
    private string gameName;
    public class Form
    {
        public string userID;
        public string game;
        public int question1;
        public int question2;
        public int question3;
        public int question4;
        public int question5;
        public int question6;
        public int question7;
        public int question8;
        public int question9;
        public int question10;
        public int question11;
        public int question12;
        public int question13;
        public int question14;
    }

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
        gameNumber = PlayerPrefs.GetInt("CurrentGameNumber");
        if (PlayerPrefs.GetString("Game" + gameNumber) == "TilesScene")
            gameName = "tiles";
        else if (PlayerPrefs.GetString("Game" + gameNumber) == "CardsScene")
            gameName = "cards";
        else if (PlayerPrefs.GetString("Game" + gameNumber) == "SpaceScene")
            gameName = "space";
        toggleSelected = new Toggle[14];
        panel1.SetActive(true);
        panel2.SetActive(false);
    }

    public void ChangePanel()
    {
        if (panel2.activeInHierarchy)
        {
            panel1.SetActive(true);
            panel2.SetActive(false);
        }
        else
        {
            panel1TogglesSelected = CheckTogglesPanel(1);
            panel1.SetActive(false);
            panel2.SetActive(true);
        }
    }

    public bool CheckTogglesPanel(int number)
    {
        int lowLim, highLim;
        if (number == 1)
        {
            lowLim = 0;
            highLim = 7;
        }
        else
        {
            lowLim = 7;
            highLim = 14;
        }
        bool toReturn = true;
        for (int i = lowLim; i < highLim; i++)
        {
            if (!toggleGroups[i].AnyTogglesOn())
            {
                toReturn = false;
            }
            else
            {
                foreach (Toggle toggle in toggleGroups[i].GetComponentsInChildren<Toggle>())
                {
                    if (toggle.isOn)
                    {
                        toggleSelected[i] = toggle;
                        break;
                    }
                }
            }

        }
        return toReturn;
    }
    public void FinishButton()
    {
        bool allTogglesSelected = panel1TogglesSelected && CheckTogglesPanel(2);

        if (!allTogglesSelected)
        {
            StartCoroutine(Warning());
        }
        else
        {
            DBManager.Instance.GenerateFormJSON(GenerateJSON());    
            if (gameNumber == 3)
            {
                PlayerPrefs.SetInt("end", 1);
                PlayerPrefs.Save();
                SceneManager.LoadScene("MenuScene");
            }
            else
            {
                PlayerPrefs.SetInt("CurrentGameNumber", gameNumber + 1);
                PlayerPrefs.Save();
                string game = "Game" + (gameNumber + 1);
                SceneManager.LoadScene(PlayerPrefs.GetString(game));
            }
        }
    }

    private IEnumerator Warning()
    {
        warningText.gameObject.SetActive(true);
        yield return new WaitForSeconds(2f);
        warningText.gameObject.SetActive(false);
    }    

    public string GenerateJSON()
    {
        
        Form form = new Form
        {
            userID = PlayerPrefs.GetString("username"),
            game = gameName,
            question1 = int.Parse(toggleSelected[0].name.Replace("Toggle", "")),
            question2 = int.Parse(toggleSelected[1].name.Replace("Toggle", "")),
            question3= int.Parse(toggleSelected[2].name.Replace("Toggle", "")),
            question4 = int.Parse(toggleSelected[3].name.Replace("Toggle", "")),
            question5 = int.Parse(toggleSelected[4].name.Replace("Toggle", "")),
            question6 = int.Parse(toggleSelected[5].name.Replace("Toggle", "")),
            question7 = int.Parse(toggleSelected[6].name.Replace("Toggle", "")),
            question8 = int.Parse(toggleSelected[7].name.Replace("Toggle", "")),
            question9 = int.Parse(toggleSelected[8].name.Replace("Toggle", "")),
            question10 = int.Parse(toggleSelected[9].name.Replace("Toggle", "")),
            question11 = int.Parse(toggleSelected[10].name.Replace("Toggle", "")),
            question12 = int.Parse(toggleSelected[11].name.Replace("Toggle", "")),
            question13 = int.Parse(toggleSelected[12].name.Replace("Toggle", "")),
            question14 = int.Parse(toggleSelected[13].name.Replace("Toggle", "")),
        };

        return JsonUtility.ToJson(form);
    }
}
