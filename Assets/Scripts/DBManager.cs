using System.Collections;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public class DBManager : MonoBehaviour
{
    public static DBManager Instance { get; private set; }

    private int gameNumber;
    private string gameName;
    private string token;
    
    public class TokenResponse
    {
        public string result;
        public string token;
        public string until;
    }
    public class User
    {
        public string userID;
        public string gender;
        public int age;
        public string birthday;

        public string game1;
        public string type1;
        public string game2;
        public string type2;
        public string game3;
        public string type3;
    }
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
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
    }

    public void GenerateFormJSON(string form)
    {
        StartCoroutine(GenerateForm(form));       

    }
    IEnumerator GenerateForm(string form)
    {
        yield return StartCoroutine(LoginRequest());
        
        string data = $@"{{       
            ""username"": ""TFGEvaAtencion"",
            ""token"" : ""{token}"",
            ""table"" : ""form"",
            ""data"" :{form}
        }}";

        yield return StartCoroutine(PostRequest(data));
    }
    IEnumerator PostRequest(string data)
    {

        UnityWebRequest request = new UnityWebRequest("https://tfvj.etsii.urjc.es/rest/insert", "POST");

        byte[] bodyRaw = Encoding.UTF8.GetBytes(data);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();


        // Configurar la solicitud (headers, etc.) si es necesario
        request.SetRequestHeader("Content-Type", "application/json");

        // Enviar la solicitud y esperar la respuesta
        yield return request.SendWebRequest();

        // Verificar si hay errores
        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogWarning("Error: " + request.error);
        }

    }

    IEnumerator LoginRequest()
    {
        string data = @"{
            ""username"":""TFGEvaAtencion"",
            ""password"":""2025TFGEGA""
        }";
        UnityWebRequest request = new UnityWebRequest("https://tfvj.etsii.urjc.es/rest/login", "POST");

        byte[] bodyRaw = Encoding.UTF8.GetBytes(data);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();


        // Configurar la solicitud (headers, etc.) si es necesario
        request.SetRequestHeader("Content-Type", "application/json");

        // Enviar la solicitud y esperar la respuesta
        yield return request.SendWebRequest();

        // Verificar si hay errores
        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogWarning("Error: " + request.error);
        }
        else
        {
            // La solicitud fue exitosa, puedes acceder a la respuesta
            TokenResponse response = JsonUtility.FromJson<TokenResponse>(request.downloadHandler.text);
            token = response.token;
        }

    }
    public void GenerateUserJSON(string gender, int age)
    {
        StartCoroutine(GenerateUser(gender, age));
    }

    IEnumerator GenerateUser(string gender, int age)
    {
        yield return StartCoroutine(LoginRequest());
        User newUser = new User
        {
            userID = PlayerPrefs.GetString("username"),
            gender = gender,
            age = age,
            birthday = PlayerPrefs.GetString("birthday"),
            game1 = PlayerPrefs.GetString("Game1"),
            type1 = PlayerPrefs.GetString("Type1"),
            game2 = PlayerPrefs.GetString("Game2"),
            type2 = PlayerPrefs.GetString("Type2"),
            game3 = PlayerPrefs.GetString("Game3"),
            type3 = PlayerPrefs.GetString("Type3")
        };

        string user = JsonUtility.ToJson(newUser);
        string data = $@"{{       
            ""username"": ""TFGEvaAtencion"",
            ""token"" : ""{token}"",
            ""table"" : ""users"",
            ""data"" :{user}
        }}";
        yield return StartCoroutine(PostRequest(data));
    }

}
