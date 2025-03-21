using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Button : MonoBehaviour
{
    private bool isToggled;
    private int id;
    private int number=0;
    [SerializeField] private Color normalColor = Color.white;
    [SerializeField] private Color toggledColor = Color.green;
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private Image image;
    private void Start()
    {
        isToggled = false;
    }
    private void Update()
    {
        text.text = number + "";
    }
    public void ButtonColor()
    {
        Button toggled = TilesGameManager.Instance.ToggledButton();
        if (toggled==this)
        {
            SetToggled(false);
        }
        else if (toggled == null)
        {
            SetToggled(true);
        }
        else {
            toggled.SetToggled(false);
            SetToggled(true);
        }
    }
    public bool IsToggled()
    {
        return isToggled;
    }

    public void SetToggled(bool isToggled)
    {
        this.isToggled = isToggled;
        if (isToggled == true)
        {
            GetComponent<Image>().color = toggledColor;
        }
        else
        {
            GetComponent<Image>().color = normalColor;
        }
    }

    public void SetID(int id)
    {
        this.id = id;
    }

    public int GetID()
    {
        return id;  
    }

    public void SetNumberOnce(int number)
    {
        this.number = number;
        if (number != 0)
        {
            text.gameObject.SetActive(true);
            image.gameObject.SetActive(true);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    public void SetNumber(int sum)
    {
        number +=sum;
    }
    public int GetNumber()
    {
        return number;
    }
}

