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
        UserInputController.Instance.OnSwitchWeapon += SwitchWeapon;
        UserInputController.Instance.OnAimingJoystick += SetAimingState;
        UserInputController.Instance.OnCancelAiming += CancelAiming;
        yield return null;
        UserInputController.Instance.OnSwitchWeapon?.Invoke(WeaponType.Melee);
    }
    private void OnDestroy()
    {
        UserInputController.Instance.OnSwitchWeapon -= SwitchWeapon;
        UserInputController.Instance.OnAimingJoystick -= SetAimingState;
        UserInputController.Instance.OnCancelAiming -= CancelAiming;
    }
    private void SwitchWeapon(WeaponType Type)
    {
        WeaponData data = null;
        if (Type == WeaponType.Melee || Type == WeaponType.Special)
            data = Resources.Load<WeaponData>(WeaponConfig.AxeLink);
        else if (Type == WeaponType.Pistol || Type == WeaponType.Rifle)
            data = Resources.Load<WeaponData>(WeaponConfig.GunLink);
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
        CurrentWeapon.StartAttack(Controller.transform);
    }
}
