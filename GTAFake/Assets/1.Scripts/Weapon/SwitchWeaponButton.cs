using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using System;
public class SwitchWeaponButton : MonoBehaviour
{
    [SerializeField] private WeaponType Type;
    [SerializeField] private Button Btn;
    [SerializeField] private Image CountdownImg;
    [SerializeField] private Image Icon;
    [SerializeField] private TextMeshProUGUI BulletCount;

    private bool CanClick = true;
    public void Setup()
    {
        Icon.sprite = Resources.Load<Sprite>(WeaponConfig.GetIconLink(Type));
        BulletCount.SetText("10/100");
        CountdownImg.fillAmount = 0;
    }
    private void Start()
    {
        Setup();
        Btn.onClick.AddListener(OnClick);
        UserInputController.Instance.OnSwitchWeapon += OnSwitchWeapon;
    }
    private void OnDestroy()
    {
        UserInputController.Instance.OnSwitchWeapon -= OnSwitchWeapon;

    }
    private void OnClick()
    {
        if (CanClick)
        {
            UserInputController.Instance.OnSwitchWeapon?.Invoke(Type);
        }
    }
    private void OnSwitchWeapon(WeaponType Type)
    {
        CountdownTime(1f);
    }

    private void CountdownTime(float duration, Action OnComplete = null)
    {
        CanClick = false;
        CountdownImg.fillAmount = 1;
        CountdownImg.DOFillAmount(0, duration).SetEase(Ease.Linear).OnComplete(() =>
        {
            CanClick = true;
            OnComplete?.Invoke();
        });
    }

}
