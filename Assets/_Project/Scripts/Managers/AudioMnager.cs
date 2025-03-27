using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    public AudioMixer audioMixer;
    public AudioSource musicSource;
    public AudioSource sfxSource;
    public AudioSource uiSource;

    public AudioClip backgroundMusic;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    private void Start()
    {
        // Загружаем сохранённые значения громкости
        float musicVolume = PlayerPrefs.GetFloat("MusicVolume", 1f);  // Если нет сохранённого значения, по умолчанию 1
        float sfxVolume = PlayerPrefs.GetFloat("SFXVolume", 1f);
        float uiVolume = PlayerPrefs.GetFloat("UIVolume", 1f);

        // Применяем громкость
        SetVolume("Volume_Music", musicVolume);
        SetVolume("Volume_SFX", sfxVolume);
        SetVolume("Volume_UI", uiVolume);

        // Инициализация музыки, если необходимо
        if (backgroundMusic != null)
        {
            PlayMusic(backgroundMusic);
        }
    }

    public void PlayMusic(AudioClip clip)
    {
        if (musicSource.isPlaying && musicSource.clip == clip) return;

        musicSource.clip = clip;
        musicSource.loop = true;
        musicSource.Play();
    }

    public void PlaySFX(AudioClip clip)
    {
        sfxSource.PlayOneShot(clip);
    }

    public void PlayUI(AudioClip clip)
    {
        uiSource.PlayOneShot(clip);
    }

    public void SetVolume(string parameter, float volume)
    {
        if (volume == 0f)
        {
            audioMixer.SetFloat(parameter, -80f);  // Полная тишина
        }
        else
        {
            audioMixer.SetFloat(parameter, Mathf.Log10(volume) * 20);
        }
    }
}
