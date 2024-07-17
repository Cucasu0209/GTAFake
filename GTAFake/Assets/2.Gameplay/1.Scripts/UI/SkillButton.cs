using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillButton : SerializedMonoBehaviour
{
    public Button Button;
    public Image Icon;
    public Dictionary<CharacterType, Sprite> IconDictionary;
    private void Start()
    {
        Button.onClick.AddListener(DoSkill);
        UserInputController.Instance.OnSwitchModel += OnSwitchModel;
    }
    private void OnDestroy()
    {
        UserInputController.Instance.OnSwitchModel -= OnSwitchModel;

    }
    public void DoSkill()
    {
        UserInputController.Instance.OnPlayerPlaySkill?.Invoke();
    }
    public void OnSwitchModel(CharacterType type)
    {
        Icon.sprite = IconDictionary[type];
        Icon.SetNativeSize();
    }
}
