using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
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
        gameNumber = PlayerPrefs.GetInt("CurrentGameNumber");
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

        string gameName = string.Empty;
        if (PlayerPrefs.GetString("Game" + gameNumber) == "TilesScene")
            gameName = "Tiles";
        else if (PlayerPrefs.GetString("Game" + gameNumber) == "CardsScene")
            gameName = "Cards";
        else if (PlayerPrefs.GetString("Game" + gameNumber) == "SpaceScene")
            gameName = "Space";

        string data = $@"{{
            ""username"":""TFGEvaAtencion"",
            ""token"":  ""VS71Ozn0WJpvd73FnhzLIRrImE+bNVkzlwMnOj4yNymB"",
            ""table"": ""test"",
            ""data"": {{
            ""userID"":""{PlayerPrefs.GetString("username")}"", 
            ""gender"":""{PlayerPrefs.GetString("gender")}"",
            ""age"":{PlayerPrefs.GetInt("age")},
            ""birthday"":""{PlayerPrefs.GetString("birthday")}"",
            ""game"": ""{gameName}"", 
            ""difficulty"": ""{PlayerPrefs.GetString("Type" + gameNumber)}"", 
            ""order"": ""{gameNumber}"",
        ";
        StringBuilder json = new StringBuilder();
        json.Append(data);
        json.Append("\"form\": [");
        foreach (Pair pair in answers)
        {
            json.Append("{\"question\": " + pair.question + ", \"answer\": " + pair.answer + "},");
        }
        json.Length--;
        json.Append("]}}");
        Debug.Log(json.ToString());
        //StartCoroutine(PostRequest(json.ToString()));

    }
    IEnumerator PostRequest(string data)
    {

        UnityWebRequest request = UnityWebRequest.PostWwwForm("https://tfvj.etsii.urjc.es/insert", data);

        // Configurar la solicitud (headers, etc.) si es necesario
        request.SetRequestHeader("Content-Type", "application/x-www-form-urlencoded");

        // Enviar la solicitud y esperar la respuesta
        yield return request.SendWebRequest();

        // Verificar si hay errores
        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.Log("Error: " + request.error);
        }
        else
        {
            // La solicitud fue exitosa, puedes acceder a la respuesta
            Debug.Log("Respuesta: " + request.downloadHandler.text);
        }

    }
}
