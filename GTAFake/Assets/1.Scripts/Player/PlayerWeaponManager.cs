using System.Collections;
using System.Collections.Generic;
using UnityEditor.UI;
using UnityEngine;
using DG.Tweening;
public class PlayerWeaponManager : MonoBehaviour
{
    private PlayerController Controller;
    [SerializeField] private Transform WeaponPosition;
    private List<BaseWeapon> CurrentWeapons = new List<BaseWeapon>();
    [HideInInspector] public BaseWeapon CurrentWeapon;

    IEnumerator Start()
    {
        Controller = GetComponent<PlayerController>();
        UserInputController.Instance.OnChooseWeaponIndex += SwitchWeapon;
        UserInputController.Instance.OnAimingJoystick += SetAimingState;
        UserInputController.Instance.OnCancelAiming += CancelAiming;
        yield return null;
        UserInputController.Instance.OnChooseWeaponIndex?.Invoke(0);
    }
    private void OnDestroy()
    {
        UserInputController.Instance.OnChooseWeaponIndex -= SwitchWeapon;
        UserInputController.Instance.OnAimingJoystick -= SetAimingState;
        UserInputController.Instance.OnCancelAiming -= CancelAiming;
    }
    private void SwitchWeapon(int weaponIndex)
    {
        WeaponData data = null;
        if (weaponIndex == 0 || weaponIndex == 3)
            data = Resources.Load<WeaponData>(WeaponsConfiguration.AxeLink);
        else if (weaponIndex == 1 || weaponIndex == 2)
            data = Resources.Load<WeaponData>(WeaponsConfiguration.GunLink);
        if (data != null) ActiveWeapon(data);
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

        BaseWeapon w = Instantiate(data.BaseWeapon, WeaponPosition);
        w.Data = data;
        w.transform.localPosition = Vector3.zero;
        CurrentWeapons.Add(w);
        if (CurrentWeapon != null) CurrentWeapon.gameObject.SetActive(false);
        CurrentWeapon = w;
    }

    float LastTimeAttack = 0;
    private void SetAimingState(float hz, float v)
    {
        if (CurrentWeapon.Data.Type == WeaponType.Melee || CurrentWeapon.Data.Type == WeaponType.Special) Controller.SetHandInWeaponAnim(2);
        else Controller.SetHandInWeaponAnim(1);


        if (Time.time - LastTimeAttack > CurrentWeapon.Data.Duration)
        {
            ShowAnimAttack();
            LastTimeAttack = Time.time;
        }
    }
    private void CancelAiming()
    {
        Controller.SetHandInWeaponAnim(0);
    }
    private void ShowAnimAttack()
    {
        Controller.SetAttackAnim();
        CurrentWeapon.StartAttack(Controller.transform.forward);
    }
}
