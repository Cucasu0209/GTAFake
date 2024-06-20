using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderButton: MonoBehaviour
{
    [SerializeField] private SettingType SettingType;
    [SerializeField] private Slider SelfSlider;

    private void Start()
    {
        SelfSlider.onValueChanged.AddListener(SetSliderValue);
        InitState();
    }

    float Volume = 1;
    private void InitState()
    {
        //switch (SettingType)
        //{
        //    case SettingType.Sound:
        //        Volume = SoundManager.Instance.SFXVolume;
        //        break;
        //    case SettingType.Music:
        //        Volume = SoundManager.Instance.BGMusicVolume;
        //        break;
        //}
        SelfSlider.value = Volume;
    }

    private void SetSliderValue(float newValue)
    {
        Volume = newValue;

        //switch (SettingType)
        //{
        //    case SettingType.Sound:
        //        SoundManager.Instance.ChangeSFXVolume(Volume);
        //        break;
        //    case SettingType.Music:
        //        SoundManager.Instance.ChangeBGVolume(Volume);
        //        break;
        //}
    }
}
