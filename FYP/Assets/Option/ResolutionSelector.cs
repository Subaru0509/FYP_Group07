using UnityEngine;
using TMPro;

public class ResolutionSelector : MonoBehaviour
{
    public TMP_Dropdown resolutionDropdown;

    private Resolution[] availableResolutions = new Resolution[]
    {
        new Resolution { width = 1920, height = 1080 },
        new Resolution { width = 1280, height = 720 },
        new Resolution { width = 1024, height = 768 }
    };

    void Start()
    {
        resolutionDropdown.onValueChanged.AddListener(OnResolutionChanged);
        Debug.Log("Resolution dropdown initialized");
    }

    void OnResolutionChanged(int index)
    {
        if (index >= 0 && index < availableResolutions.Length)
        {
            Resolution res = availableResolutions[index];
            Debug.Log("Resolution changed to: " + res.width + "x" + res.height);
            Screen.SetResolution(res.width, res.height, FullScreenMode.Windowed);
        }
    }
}
