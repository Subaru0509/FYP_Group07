using UnityEngine;
using UnityEngine.UI;

public class GameplayOptionToggle_UI : MonoBehaviour
{
    public string optionName;
    [SerializeField] private Toggle optionToggle;

    public bool GetToggleValue()
    {
        return optionToggle.isOn;
    }

    public void SetToggleValue(bool _value)
    {
        optionToggle.isOn = _value;
    }
}
