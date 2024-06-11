using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using System;
using UnityEngine.EventSystems;
public class SwitchWeaponButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private WeaponType Type;
    [SerializeField] private Button Btn;
    [SerializeField] private Image CountdownImg;
    [SerializeField] private Image Icon;
    [SerializeField] private TextMeshProUGUI BulletCount;

    private bool CanClick = true;
    private bool IsPressing = false;
    public void Setup()
    {
        Icon.sprite = Resources.Load<Sprite>(WeaponConfig.GetIconLink(Type));
        CountdownImg.fillAmount = 0;
        OnBulletCountChange();
    }
    private void Start()
    {
        Setup();
        Btn.onClick.AddListener(OnClick);
        UserInputController.Instance.OnSwitchWeapon += OnSwitchWeapon;
        GameManager.Instance.OnPlayerFired += OnBulletCountChange;
    }
    private void OnDestroy()
    {
        UserInputController.Instance.OnSwitchWeapon -= OnSwitchWeapon;
        GameManager.Instance.OnPlayerFired -= OnBulletCountChange;

    }
    private void OnBulletCountChange()
    {
        WeaponData data = Resources.Load<WeaponData>(WeaponConfig.GetDataLink(Type));

        if (data != null)
        {
            if (data.BulletMaxCount > 0)
            {
                BulletCount.transform.eulerAngles = Vector3.zero;
                BulletCount.SetText($"{data.BulletCount}/{data.BulletMaxCount}");
            }
            else
            {
                BulletCount.transform.eulerAngles = Vector3.forward * 90;
                BulletCount.SetText("8");
            }
        }
    }
    private void OnClick()
    {
        if (CanClick && Type != PlayerData.GetCurrentWeaponData().Type)
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

    public void OnPointerDown(PointerEventData eventData)
    {
        if (CanClick && Type == PlayerData.GetCurrentWeaponData().Type)
        {
            UserInputController.Instance.OnStartAiming?.Invoke();
            IsPressing = true;
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (CanClick && Type == PlayerData.GetCurrentWeaponData().Type)
        {
            UserInputController.Instance.OnCancelAiming?.Invoke();
            IsPressing = false;
        }
    }
    private void Update()
    {
        if (IsPressing)
        {
            UserInputController.Instance.OnAimingJoystick?.Invoke(0, 1);
        }
    }
}
