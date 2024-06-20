using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SwitchButton: MonoBehaviour
{
    [SerializeField] private SettingType SettingType;
    [SerializeField] private Button SelfButton;
    [SerializeField] private Image SwitchImage;
    [SerializeField] private Sprite SwitchSprite_On, SwitchSprite_Off;

    private Dictionary<SettingType, Tuple<string, int>> SettingKeysDict = new Dictionary<SettingType, Tuple<string, int>>()
    {
        {SettingType.ReduceVFX, ("Setting_ReduceVFX", 0).ToTuple() },
        {SettingType.ShowJoystick, ("Setting_ShowJoystick", 1).ToTuple() }
    };
    private void Start()
    {
        SelfButton.onClick.AddListener(Switch);
        InitState();
    }

    bool IsOn = true;
    private void InitState()
    {
        switch (SettingType)
        {
            case SettingType.Vibration:
                //IsOn = SoundManager.Instance.IsHapticOn;
                break;
            case SettingType.ReduceVFX:
            case SettingType.ShowJoystick:
                // TOAN TOAN TOAN
                // CHƯA SETTING LÊN GAME MANAGER
                IsOn = PlayerPrefs.GetInt(SettingKeysDict[SettingType].Item1, SettingKeysDict[SettingType].Item2) == 1;
                break;
        }
        SetState();
    }

    private void Switch()
    {
        IsOn = !IsOn;
        SetState();

        switch (SettingType)
        {
            case SettingType.Vibration:
                //SoundManager.Instance.SwitchHaptic();
                break;
            case SettingType.ReduceVFX:
            case SettingType.ShowJoystick:
                // TOAN TOAN TOAN
                // CHƯA SETTING LÊN GAME MANAGER
                PlayerPrefs.SetInt(SettingKeysDict[SettingType].Item1, IsOn ? 1 : 0);
                break;
        }
    }

    private void SetState()
    {
        SwitchImage.sprite = IsOn ? SwitchSprite_On : SwitchSprite_Off;
    }
}
public enum SettingType
{
    Sound = 0,
    Music = 1,
    Vibration = 2,
    ReduceVFX = 3,
    ShowJoystick = 4,
}
