using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    [Header("SFX")]
    public AudioClip flipper;
    public AudioClip launcher;
    public AudioClip genericCollide;
    public AudioClip bumper1;
    public AudioClip bumper2;
    public AudioClip rolling;
    public AudioClip monsterCollide1;
    public AudioClip monsterCollide2;
    public AudioClip pointAccumulate;
    public AudioClip GoldAccumulate;
    public AudioClip uiClick;
    public AudioClip ballRefill;
    public AudioClip upgradeSelect;
    public AudioClip gameStart;

    [Header("BGM")]
    public AudioClip backgroundMusic;

    private AudioSource sfxSource;
    private AudioSource bgmSource;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            sfxSource = gameObject.AddComponent<AudioSource>();
            bgmSource = gameObject.AddComponent<AudioSource>();

            bgmSource.loop = true;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlaySFX(AudioClip clip, float volume = 1f)
    {
        if (clip != null)
            sfxSource.PlayOneShot(clip, volume);
    }

    public void PlayBGM()
    {
        if (backgroundMusic != null)
        {
            bgmSource.clip = backgroundMusic;
            bgmSource.Play();
        }
    }

    public void StopBGM()
    {
        bgmSource.Stop();
    }
}
