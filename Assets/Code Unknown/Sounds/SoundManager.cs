using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    [Header("SFX Clips")]
    public AudioClip flipper;
    [Range(0f, 1f)] public float flipperVolume = 1f;

    public AudioClip launcher;
    [Range(0f, 1f)] public float launcherVolume = 1f;

    public AudioClip launcherLoop;
    [Range(0f, 1f)] public float launcherLoopVolume = 1f;

    public AudioClip genericCollide;
    [Range(0f, 1f)] public float genericCollideVolume = 1f;

    public AudioClip bumper1;
    [Range(0f, 1f)] public float bumper1Volume = 1f;

    public AudioClip bumper2;
    [Range(0f, 1f)] public float bumper2Volume = 1f;

    public AudioClip rolling;
    [Range(0f, 1f)] public float rollingVolume = 1f;

    public AudioClip monsterCollide1;
    [Range(0f, 1f)] public float monsterCollide1Volume = 1f;

    public AudioClip monsterCollide2;
    [Range(0f, 1f)] public float monsterCollide2Volume = 1f;

    public AudioClip pointAccumulate;
    [Range(0f, 1f)] public float pointAccumulateVolume = 1f;

    public AudioClip goldAccumulate;
    [Range(0f, 1f)] public float goldAccumulateVolume = 1f;

    public AudioClip uiClick;
    [Range(0f, 1f)] public float uiClickVolume = 1f;

    public AudioClip ballRefill;
    [Range(0f, 1f)] public float ballRefillVolume = 1f;

    public AudioClip upgradeSelect;
    [Range(0f, 1f)] public float upgradeSelectVolume = 1f;

    public AudioClip gameStart;
    [Range(0f, 1f)] public float gameStartVolume = 1f;

    [Header("BGM")]
    public AudioClip backgroundMusic;
    [Range(0f, 1f)] public float bgmVolume = 0.5f;


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
            bgmSource.volume = bgmVolume;
            bgmSource.Play();
        }
    }

    public void StopBGM()
    {
        bgmSource.Stop();
    }
    //this is for looping and stop
    public void PlayLoopingSFX(AudioClip clip, ref AudioSource source, float volume = 1f)
    {
        if (clip == null) return;

        if (source == null)
        {
            source = gameObject.AddComponent<AudioSource>();
            source.loop = true;
        }

        if (!source.isPlaying)
        {
            source.clip = clip;
            source.volume = volume;
            source.Play();
        }
        else
        {
            source.volume = volume; 
        }
    }

    // stop loop
    public void StopLoopingSFX(ref AudioSource source)
    {
        if (source != null && source.isPlaying)
        {
            source.Stop();
        }
    }
    public void PlayFlipper() => PlaySFX(flipper, flipperVolume);
    public void PlayLauncher() => PlaySFX(launcher, launcherVolume);
    public void PlayLauncherLoop() => PlaySFX(launcherLoop, launcherLoopVolume);
    public void PlayGenericCollide() => PlaySFX(genericCollide, genericCollideVolume);
    public void PlayBumper1() => PlaySFX(bumper1, bumper1Volume);
    public void PlayBumper2() => PlaySFX(bumper2, bumper2Volume);
    public void PlayRolling() => PlaySFX(rolling, rollingVolume);
    public void PlayMonsterCollide1() => PlaySFX(monsterCollide1, monsterCollide1Volume);
    public void PlayMonsterCollide2() => PlaySFX(monsterCollide2, monsterCollide2Volume);
    public void PlayPointAccumulate() => PlaySFX(pointAccumulate, pointAccumulateVolume);
    public void PlayGoldAccumulate() => PlaySFX(goldAccumulate, goldAccumulateVolume);
    public void PlayUIClick() => PlaySFX(uiClick, uiClickVolume);
    public void PlayBallRefill() => PlaySFX(ballRefill, ballRefillVolume);
    public void PlayUpgradeSelect() => PlaySFX(upgradeSelect, upgradeSelectVolume);
    public void PlayGameStart() => PlaySFX(gameStart, gameStartVolume);
}
