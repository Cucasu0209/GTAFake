using System.Collections;
using System.Collections.Generic;
using UnityEditor.UI;
using UnityEngine;
using DG.Tweening;
using System;
using System.Reflection;
public class PlayerWeaponManager : MonoBehaviour
{
    private PlayerController Controller;
    [SerializeField] private Transform WeaponPosition;
    private List<BaseWeapon> CurrentWeapons = new List<BaseWeapon>();
    [HideInInspector] public BaseWeapon CurrentWeapon;

    public Action ChangeWeaponDataCallback;
    IEnumerator Start()
    {
        Controller = GetComponent<PlayerController>();
        UserInputController.Instance.OnSwitchWeapon += OnChangeWeapon;
        UserInputController.Instance.OnAimingJoystick += SetAimingState;
        UserInputController.Instance.OnStartAiming += StartAiming;
        UserInputController.Instance.OnCancelAiming += CancelAiming;
        yield return null;
        UserInputController.Instance.OnSwitchWeapon?.Invoke(WeaponType.Melee);
    }
    private void OnDestroy()
    {
        UserInputController.Instance.OnSwitchWeapon -= OnChangeWeapon;
        UserInputController.Instance.OnAimingJoystick -= SetAimingState;
        UserInputController.Instance.OnStartAiming -= StartAiming;
        UserInputController.Instance.OnCancelAiming -= CancelAiming;
    }

    private void OnChangeWeapon(WeaponType type)
    {
        Controller.StartChangeWeapon();
        if (Controller.IsAiming) UserInputController.Instance.OnCancelAiming?.Invoke();
        Controller.ChangeWeaponDataCallback = () => SwitchWeapon(type);
    }
    private void SwitchWeapon(WeaponType type)
    {
        WeaponData data = null;
        if (type == WeaponType.Melee || type == WeaponType.Special)
            data = Resources.Load<WeaponData>(WeaponConfig.AxeLink);
        else if (type == WeaponType.Pistol || type == WeaponType.Rifle)
            data = Resources.Load<WeaponData>(WeaponConfig.GunLink);
        if (data != null)
        {
            PlayerData.SetCurrentWeaponData(data);
            ActiveWeapon(data);
        }
    }
    private void ActiveWeapon(WeaponData data)
    {

        foreach (BaseWeapon weapon in CurrentWeapons)
        {
            if (weapon.Data.name == data.name)
            {
                if (CurrentWeapon != null) CurrentWeapon.gameObject.SetActive(false);
                CurrentWeapon = weapon;
                CurrentWeapon.gameObject.SetActive(true);
                return;
            }
        }

        BaseWeapon wr = Resources.Load<BaseWeapon>(data.LinkPrefab);
        if (wr != null)
        {
            BaseWeapon w = Instantiate(wr, WeaponPosition);
            w.Data = data;
            w.transform.localPosition = Vector3.zero;
            CurrentWeapons.Add(w);
            if (CurrentWeapon != null) CurrentWeapon.gameObject.SetActive(false);
            CurrentWeapon = w;
        }
    }
    private void SetAimingState(float hz, float v)
    {
        ShowAnimAttack();
    }
    private void StartAiming()
    {
        Controller.PlayerAnimator.SetLayerWeight(Controller.PlayerAnimator.GetLayerIndex(Controller.AimAxeLayerName),
            CurrentWeapon.Data.Type == WeaponType.Melee || CurrentWeapon.Data.Type == WeaponType.Special ? 1 : 0);
        Controller.PlayerAnimator.SetLayerWeight(Controller.PlayerAnimator.GetLayerIndex(Controller.AimLayerName),
            CurrentWeapon.Data.Type == WeaponType.Rifle || CurrentWeapon.Data.Type == WeaponType.Pistol ? 1 : 0);
    }
    private void CancelAiming()
    {
        Controller.PlayerAnimator.SetLayerWeight(Controller.PlayerAnimator.GetLayerIndex(Controller.AimLayerName), 0);
        Controller.PlayerAnimator.SetLayerWeight(Controller.PlayerAnimator.GetLayerIndex(Controller.AimAxeLayerName), 0);
    }
    private void ShowAnimAttack()
    {
        Controller.SetAttackAnim();
        Controller.AttackCallback = () => CurrentWeapon.StartAttack(Controller.transform);
    }
}
