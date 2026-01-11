using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ResolutionSelector : MonoBehaviour
{
    public TMP_Dropdown resolutionDropdown;
    public Toggle fullscreenToggle;

    private Resolution[] availableResolutions = new Resolution[]
    {
        new Resolution { width = 1920, height = 1080 },
        new Resolution { width = 1280, height = 720 },
        new Resolution { width = 1024, height = 768 }
    };

    void Start()
    {
        resolutionDropdown.onValueChanged.AddListener(OnResolutionChanged);
        fullscreenToggle.onValueChanged.AddListener(OnFullscreenChanged);
        Debug.Log("Resolution + Fullscreen initialized");
    }

    void OnResolutionChanged(int index)
    {
        if (index >= 0 && index < availableResolutions.Length)
        {
            Resolution res = availableResolutions[index];
            bool isFullscreen = fullscreenToggle.isOn;
            Screen.SetResolution(res.width, res.height, isFullscreen);
            Debug.Log("Resolution set to: " + res.width + "x" + res.height + " Fullscreen: " + isFullscreen);
        }
    }

    void OnFullscreenChanged(bool isFullscreen)
    {
        int index = resolutionDropdown.value;
        if (index >= 0 && index < availableResolutions.Length)
        {
            Resolution res = availableResolutions[index];
            Screen.SetResolution(res.width, res.height, isFullscreen);
            Debug.Log("Fullscreen toggled: " + isFullscreen);
        }
    }
}
