using UnityEngine;

public class BGMPlayer : MonoBehaviour
{
    public AudioSource bgmSource;

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        bgmSource.Play();
    }

    void Update()
    {
        float volume = PlayerPrefs.GetFloat("MusicVolume", 1f);
        bool mute = PlayerPrefs.GetInt("MusicMute", 0) == 1;

        bgmSource.volume = volume;
        bgmSource.mute = mute;
    }
}
