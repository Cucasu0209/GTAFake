using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using TMPro;

public class Home_Popup_Setting : Home_Popup
{

    public Button AcceptButton;
    public Slider MusicSlider;
    public Slider SoundSlider;
    private const string MusicKey = "MUSIC_SETTING_VALUE";
    private const string SoundKey = "SOUND_SETTING_VALUE";
    private float LastMusicValue;
    private float LastSoundValue;
    protected override void Start()
    {
        base.Start();
        AcceptButton.onClick.AddListener(SaveConfig);
    }
    protected override void OpenPopup()
    {
        LastMusicValue = PlayerPrefs.GetFloat(MusicKey, 0.7f);
        LastSoundValue = PlayerPrefs.GetFloat(SoundKey, 0.5f);
        MusicSlider.SetValueWithoutNotify(LastMusicValue);
        SoundSlider.SetValueWithoutNotify(LastSoundValue);
        MusicSlider.onValueChanged.AddListener(OnMusicVolumeChange);
        SoundSlider.onValueChanged.AddListener(OnSoundVolumeChange);
    }
    public void SaveConfig()
    {
        base.OnClosePopup();
        PlayerPrefs.SetFloat(MusicKey, MusicSlider.value);
        PlayerPrefs.SetFloat(SoundKey, SoundSlider.value);
    }
    protected override void OnClosePopup()
    {
        base.OnClosePopup();
        SoundManager.Instance.ChangeBGVolume(LastMusicValue);
        SoundManager.Instance.ChangeSFXVolume(LastSoundValue);
    }
    private void OnMusicVolumeChange(float value)
    {
        SoundManager.Instance.ChangeBGVolume(value);
    }

    private void OnSoundVolumeChange(float value)
    {
        SoundManager.Instance.ChangeBGVolume(value);
    }
}
