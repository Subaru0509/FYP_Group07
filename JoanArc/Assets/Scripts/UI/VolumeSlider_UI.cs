using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeSlider_UI : MonoBehaviour
{
    public Slider slider;
    public string parameter;

    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private float multiplier;

    public void SliderValue(float _value)
    {
        audioMixer.SetFloat(parameter, Mathf.Log10(_value) * multiplier);
    }

    public void LoadVolumeSlider(float _value)
    {
        if (_value >= 0.001f)
        {
            slider.value = _value;
        }
    }
}
