using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;
using UnityEngine.SceneManagement;
//using Lofelt.NiceVibrations;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            SetBGMusicState();
            SetSFXState();
            SetHapticState();
        }
        else
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }

    #region LOAD BG MUSIC
    private void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        switch (scene.name)
        {
            case GameConfig.HOME_SCENE:
                BGAudioSource.mute = false;
                break;
            default:
                BGAudioSource.mute = true;
                break;
        }
    }
    #endregion LOAD BG MUSIC


    #region Background Music
    [SerializeField] private AudioSource BGAudioSource;
    private const string BGMusicSetting = "BGMusicSetting";
    public float BGMusicVolume { get; private set; } = 1;
    private void SetBGMusicState()
    {
        BGAudioSource.Play();
        if (PlayerPrefs.HasKey(BGMusicSetting) == false)
        {
            PlayerPrefs.SetInt(BGMusicSetting, 1);
        }
        BGMusicVolume = PlayerPrefs.GetFloat(BGMusicSetting, 1);
        BGAudioSource.volume = BGMusicVolume;
    }
    /// <summary>
    /// Bật tắt background music
    /// </summary>
    public void ChangeBGVolume(float newVolume)
    {
        BGMusicVolume = newVolume;
        BGAudioSource.volume = BGMusicVolume;
        PlayerPrefs.SetFloat(BGMusicSetting, BGMusicVolume);
    }

    public void MuteBackgroundMusic()
    {
        BGAudioSource.mute = true;
    }
    #endregion Background Music

    #region SOUND EFFECT
    [SerializeField] private AudioSource SFXAudioSource;
    [SerializeField] private AudioClip ButtonSound;
    [SerializeField] private AudioClip UpgradeSound;
    [SerializeField] private AudioClip CollectCoinSound;
    [SerializeField] private AudioClip RankUpSound;
    [SerializeField] private AudioClip CupLevelUpSound;
    [SerializeField] private AudioClip ShootSound;
    [SerializeField] private AudioClip ExplosionSound;
    [SerializeField] private AudioClip MovingSound;
    [SerializeField] private AudioClip WinSound;
    [SerializeField] private AudioClip LoseSound;

    private const string SFXSetting = "SFXSetting";
    public float SFXVolume { get; private set; } = 1;

    private void SetSFXState()
    {
        if (PlayerPrefs.HasKey(SFXSetting) == false)
        {
            PlayerPrefs.SetInt(SFXSetting, 1);
        }
        SFXVolume = PlayerPrefs.GetFloat(SFXSetting, 1);
        SFXAudioSource.volume = SFXVolume;
    }
    /// <summary>
    /// Bật tắt sound effect
    /// </summary>
    public void ChangeSFXVolume(float newVolume)
    {
        SFXVolume = newVolume;
        SFXAudioSource.volume = SFXVolume;
        PlayerPrefs.SetFloat(SFXSetting, SFXVolume);
    }
    public void PlayButtonSound()
    {
        PlayEffect(ButtonSound);
    }

    public void PlayUpgradeSound()
    {
        PlayEffect(UpgradeSound);
    }
    /// <summary>
    /// Sound khi nhặt coin
    /// </summary>
    public void PlayCollectCoinSound()
    {
        PlayEffect(CollectCoinSound);
    }
    /// <summary>
    /// Sound khi lên cup qua mốc (Gold I lên Gold II)
    /// </summary>
    public void PlayRankUpSound()
    {
        PlayEffect(RankUpSound);
    }
    /// <summary>
    /// Sound khi tăng cup
    /// </summary>
    public void PlayCupLevelUpSound()
    {
        PlayEffect(CupLevelUpSound);
    }
    public void PlayShootSound()
    {
        PlayEffect(ShootSound);
    }

    public void PlayExplosionSound()
    {
        PlayEffect(ExplosionSound);
    }

    public void PlayMovingSound()
    {
    }

    public void StopMovingSound()
    {
    }

    public void PlayWinSound()
    {
        PlayEffect(WinSound);
    }
    public void PlayLoseSound()
    {
        PlayEffect(LoseSound);
    }
    public void PlayEffect(AudioClip clip, float pitch = 1, float volume = 1)
    {
        if (clip == null)
        {
            Debug.LogError("null sound");
            return;
        }
        SFXAudioSource.pitch = pitch;
        SFXAudioSource.PlayOneShot(clip, volume);
    }

    public void PlayEffectWithDelay(AudioClip clip, Action CallbackAction = null, float delayTime = 0.25f, float pitch = 1, float volume = 1)
    {
        if (CallbackAction != null)
        {
            DOVirtual.DelayedCall(delayTime, () => CallbackAction());
        }
        PlayEffect(clip, pitch, volume);
    }
    #endregion SOUND EFFECT


    #region SKILL SOUND
    //private Dictionary<SkillType, float> SoundTimerDict = new Dictionary<SkillType, float>();
    //public void PlaySkillHitSound(AudioClip clip, SkillType skillType)
    //{
    //    if (SoundTimerDict.ContainsKey(skillType) == false)
    //    {
    //        SoundTimerDict.Add(skillType, -1);
    //    }
    //    else
    //    {
    //        if (SoundTimerDict[skillType] + 0.15f > Time.realtimeSinceStartup)
    //        {
    //            return;
    //        }
    //    }
    //    SoundTimerDict[skillType] = Time.realtimeSinceStartup;
    //    PlayEffect(clip);
    //}

    //private Dictionary<SkillType, float> SkillSoundTimerDict = new Dictionary<SkillType, float>();
    //public void PlaySkillSound(AudioClip clip, SkillType skillType)
    //{
    //    if (SkillSoundTimerDict.ContainsKey(skillType) == false)
    //    {
    //        SkillSoundTimerDict.Add(skillType, -1);
    //    }
    //    else
    //    {
    //        if (SkillSoundTimerDict[skillType] + 0.1f > Time.realtimeSinceStartup)
    //        {
    //            return;
    //        }
    //    }
    //    SkillSoundTimerDict[skillType] = Time.realtimeSinceStartup;
    //    PlayEffect(clip);
    //}
    #endregion SKILL SOUND
    #region Haptic
    private const string HapticSetting = "HapticSetting";
    public bool IsHapticOn { get; private set; } = true;
    private void SetHapticState()
    {
        //if (PlayerPrefs.HasKey(HapticSetting) == false)
        //{
        //    PlayerPrefs.SetInt(HapticSetting, 1);
        //}
        //IsHapticOn = PlayerPrefs.GetInt(HapticSetting, 0) == 1;
    }
    /// <summary>
    /// Bật tắt rung
    /// </summary>
    public void SwitchHaptic()
    {
        //IsHapticOn = !IsHapticOn;

        //PlayerPrefs.SetInt(HapticSetting, IsHapticOn ? 1 : 0);
    }
    public void Vibrate()
    {
        //IsHapticOn = PlayerPrefs.GetInt(HapticSetting, 0) == 1;
        //if (IsHapticOn)
        //{
        //    HapticPatterns.PlayPreset(HapticPatterns.PresetType.HeavyImpact);
        //}
    }
    #endregion Haptic
}
