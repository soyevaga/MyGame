using System.Collections;
using System.Collections.Generic;
using System.Text;
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
    public class Pair
    {
        public int question;   
        public int answer;    
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
        toggleSelected = new Toggle[14];
        panel1.SetActive(true);
        panel2.SetActive(false);
    }

    void Update()
    {
        
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
            GenerateJSON();
            int number = PlayerPrefs.GetInt("CurrentGameNumber");
            if (number == 4)
            {
                PlayerPrefs.SetInt("end", 1);
                PlayerPrefs.Save();
                SceneManager.LoadScene("MenuScene");
            }
            else
            {
                string game = "Game" + number;
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


    public void GenerateJSON()
    {
        List<Pair> answers = new List<Pair>();

        for (int i = 0; i < toggleSelected.Length; i++)
        {
            if (toggleSelected[i] != null)
            {
                int numberSelected = int.Parse(toggleSelected[i].name.Replace("Toggle", ""));
                answers.Add(new Pair
                {
                    question = i + 1,
                    answer = numberSelected
                });
            }
        }

        StringBuilder json = new StringBuilder();
        json.Append("{\"form\": [");
        foreach (Pair pair in answers)
        {
            json.Append("{\"question\": " +pair.question+", \"answer\": "+pair.answer+"},");
        }
        json.Length--;
        json.Append("]}");
            
        Debug.Log(json);

    }

}
