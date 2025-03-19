using UnityEngine;
using UnityEngine.UI;

public class Button : MonoBehaviour
{
    private bool isToggled;
    private int id;
    [SerializeField] private Color normalColor = Color.white;
    [SerializeField] private Color toggledColor = Color.green;
    private void Start()
    {
        isToggled = false;
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
}

