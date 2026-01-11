using UnityEngine;
using UnityEngine.UI;

public class CustomToggleVisual : MonoBehaviour
{
    public Toggle toggle;
    public Image background;
    public Color normalColor = new Color(0.2f, 0.2f, 0.2f, 1f);
    public Color selectedColor = Color.white;

    private void Start()
    {
        if (toggle == null) toggle = GetComponent<Toggle>();
        if (background == null) background = toggle.GetComponentInChildren<Image>();
        UpdateVisual();
        toggle.onValueChanged.AddListener(OnToggleChanged);
    }

    private void OnToggleChanged(bool isOn)
    {
        UpdateVisual();
    }

    private void UpdateVisual()
    {
        if (background != null)
        {
            background.color = toggle.isOn ? selectedColor : normalColor;
        }
    }
}