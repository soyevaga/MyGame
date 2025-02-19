using UnityEngine;
using TMPro;
using System.Text.RegularExpressions;

public class UsernameValidator : MonoBehaviour
{
    [SerializeField] private TMP_InputField usernameInput;
    [SerializeField] private TMP_Text warningText;
    private int maxUsernameLength = 12;
    private string validCharactersPattern = @"^[a-zA-Z0-9_.]+$";

    void Start()
    {
        usernameInput.characterLimit = maxUsernameLength;
        usernameInput.onValueChanged.AddListener(ValidateUsername);
    }

    void ValidateUsername(string input)
    {
        if (input.Length > maxUsernameLength)
        {
            usernameInput.text = input.Substring(0, maxUsernameLength);
        }

        else if (!Regex.IsMatch(input, validCharactersPattern))
        {
            usernameInput.text = Regex.Replace(input, @"[^a-zA-Z0-9_.]", "");
            warningText.text = "Only numbers, letters and symbols _ . are accepted";
        }
        else
        {
            warningText.text = "";
        }
    }
}
