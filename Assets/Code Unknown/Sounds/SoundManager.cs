using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    [Header("SFX Sources")]
    public AudioSource flipperSource;
    [Range(0f, 1f)] public float flipperVolume = 1f;

    public AudioSource launcherSource;
    [Range(0f, 1f)] public float launcherVolume = 1f;

    public AudioSource launcherLoopSource;
    [Range(0f, 1f)] public float launcherLoopVolume = 1f;

    public AudioSource genericCollideSource;
    [Range(0f, 1f)] public float genericCollideVolume = 1f;

    public AudioSource bumper1Source;
    [Range(0f, 1f)] public float bumper1Volume = 1f;

    public AudioSource bumper2Source;
    [Range(0f, 1f)] public float bumper2Volume = 1f;

    public AudioSource rollingSource;
    [Range(0f, 1f)] public float rollingVolume = 1f;

    public AudioSource monsterCollide1Source;
    [Range(0f, 1f)] public float monsterCollide1Volume = 1f;

    public AudioSource monsterCollide2Source;
    [Range(0f, 1f)] public float monsterCollide2Volume = 1f;

    public AudioSource pointAccumulateSource;
    [Range(0f, 1f)] public float pointAccumulateVolume = 1f;

    public AudioSource goldAccumulateSource;
    [Range(0f, 1f)] public float goldAccumulateVolume = 1f;

    public AudioSource uiClickSource;
    [Range(0f, 1f)] public float uiClickVolume = 1f;

    public AudioSource ballRefillSource;
    [Range(0f, 1f)] public float ballRefillVolume = 1f;

    public AudioSource upgradeSelectSource;
    [Range(0f, 1f)] public float upgradeSelectVolume = 1f;

    public AudioSource gameStartSource;
    [Range(0f, 1f)] public float gameStartVolume = 1f;

    public AudioSource spinnerSource;
    [Range(0f, 1f)] public float spinnerVolume = 1f;

    public AudioSource bossDamagerSource;
    [Range(0f, 1f)] public float bossDamagerVolume = 1f;

    public AudioSource teleporterSource;
    [Range(0f, 1f)] public float teleporterVolume = 1f;

    [Header("BGM Source")]
    public AudioSource bgmSource;
    [Range(0f, 1f)] public float BGMVolume = 1f;

    [Tooltip("All available background musics for random stage switching")]
    public List<AudioClip> bgmClips = new List<AudioClip>(); //add in Inspector
    private int currentBGMIndex = -1;
    [Header("Master Volume")]
    [Range(0f, 1f)] public float sfxVolume = 1f;
    [Range(0f, 1f)] public float bgmVolume = 0.5f;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        UpdateAllVolumes();
    }

    private void Update()
    {
        bool shouldPlayRolling = false;

        AbstractBall[] balls = FindObjectsByType<AbstractBall>(FindObjectsSortMode.None);
        foreach (AbstractBall ball in balls)
        {
            if (ball.GetVelocity().magnitude > 4.0f)
            {
                shouldPlayRolling = true;
                break;
            }
        }

        if (!rollingSource.isPlaying && shouldPlayRolling)
        {
            PlayLoopingSFX(rollingSource, rollingVolume);
        } else if (rollingSource.isPlaying && !shouldPlayRolling)
        {
            rollingSource.Stop();
        }
    }

    public void PlaySFX(AudioSource source, float volume)
    {
        if (source == null || source.clip == null) return;
        source.volume = volume * sfxVolume;
        source.PlayOneShot(source.clip, source.volume);
    }


    public void PlayLoopingSFX(AudioSource source, float volume)
    {
        if (source == null || source.clip == null) return;
        source.volume = volume * sfxVolume;
        source.loop = true;
        if (!source.isPlaying) source.Play();
    }

    public void StopLoopingSFX(AudioSource source)
    {
        if (source != null && source.isPlaying) source.Stop();
    }


    public void PlayBGM()
    {
        if (bgmSource == null) return;

        // 如果没有clip但列表中有内容，从中选一个随机播放
        if (bgmSource.clip == null && bgmClips.Count > 0)
        {
            currentBGMIndex = Random.Range(0, bgmClips.Count);
            bgmSource.clip = bgmClips[currentBGMIndex];
        }

        bgmSource.volume = bgmVolume;
        bgmSource.loop = true;
        if (!bgmSource.isPlaying && bgmSource.clip != null)
            bgmSource.Play();
    }

    public void StopBGM()
    {
        if (bgmSource != null) bgmSource.Stop();
    }
    public void PlayNextRandomBGM()
    {
        if (bgmClips.Count == 0 || bgmSource == null) return;

        int newIndex;
        if (bgmClips.Count == 1)
        {
            newIndex = 0;
        }
        else
        {
            // makesure different than current one
            do
            {
                newIndex = Random.Range(0, bgmClips.Count);
            } while (newIndex == currentBGMIndex);
        }

        currentBGMIndex = newIndex;
        bgmSource.clip = bgmClips[newIndex];
        bgmSource.volume = bgmVolume;
        bgmSource.loop = true;
        bgmSource.Play();

        Debug.Log($"[SoundManager] Switched to new BGM: {bgmSource.clip.name}");
    }
    public void UpdateAllVolumes()
    {
  
        if (bgmSource != null) bgmSource.volume = bgmVolume;

    }

    public void PlayFlipper() => PlaySFX(flipperSource, flipperVolume);
    public void PlayLauncher() => PlaySFX(launcherSource, launcherVolume);
    public void PlayLauncherLoop() => PlayLoopingSFX(launcherLoopSource, launcherLoopVolume);
    public void StopLauncherLoop() => StopLoopingSFX(launcherLoopSource);
    public void PlayGenericCollide() => PlaySFX(genericCollideSource, genericCollideVolume);
    public void PlayBumper1() => PlaySFX(bumper1Source, bumper1Volume);
    public void PlayBumper2() => PlaySFX(bumper2Source, bumper2Volume);
    public void PlayRolling() => PlaySFX(rollingSource, rollingVolume);
    public void PlayMonsterCollide1() => PlaySFX(monsterCollide1Source, monsterCollide1Volume);
    public void PlayMonsterCollide2() => PlaySFX(monsterCollide2Source, monsterCollide2Volume);
    public void PlayPointAccumulate() => PlaySFX(pointAccumulateSource, pointAccumulateVolume);
    public void PlayGoldAccumulate() => PlaySFX(goldAccumulateSource, goldAccumulateVolume);
    public void PlayUIClick() => PlaySFX(uiClickSource, uiClickVolume);
    public void PlayBallRefill() => PlaySFX(ballRefillSource, ballRefillVolume);
    public void PlayUpgradeSelect() => PlaySFX(upgradeSelectSource, upgradeSelectVolume);
    public void PlayGameStart() => PlaySFX(gameStartSource, gameStartVolume);
    public void PlaySpinner() => PlaySFX(spinnerSource, spinnerVolume);
    public void PlayBossDamager() => PlaySFX(bossDamagerSource, bossDamagerVolume);
    public void PlayTeleporter() => PlaySFX(teleporterSource, teleporterVolume);
}
