using UnityEngine;
using UnityEngine.UI;

public class BGMOptions : MonoBehaviour
{
    public Slider musicSlider;
    public Toggle muteToggle;

    void Start()
    {
        musicSlider.onValueChanged.AddListener(SetMusicVolume);
        muteToggle.onValueChanged.AddListener(SetMuteState);

        musicSlider.value = PlayerPrefs.GetFloat("MusicVolume", 1f);
        muteToggle.isOn = PlayerPrefs.GetInt("MusicMute", 0) == 1;
    }

    void SetMusicVolume(float volume)
    {
        PlayerPrefs.SetFloat("MusicVolume", volume);
    }

    void SetMuteState(bool isMuted)
    {
        PlayerPrefs.SetInt("MusicMute", isMuted ? 1 : 0);
    }
}
