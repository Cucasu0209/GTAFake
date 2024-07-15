using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class PlayerWeaponManager : MonoBehaviour
{
    private PlayerController Controller;
    [SerializeField] public Transform WeaponPosition;
    [HideInInspector] public BaseWeapon CurrentWeapon;
    public List<BaseWeapon> CurrentWeapons = new List<BaseWeapon>();

    public Action ChangeWeaponDataCallback;
    private bool reloadingBullet = false;

    #region Monobehaviour
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
    #endregion

    #region Action
    private void ShowReloadBulletAnim()
    {
        reloadingBullet = true;
        // Controller.PlayerAnimator.SetLayerWeight(Controller.PlayerAnimator.GetLayerIndex(Controller.PistolLayerName), 1);
        Controller.SetAnimReload();
        Controller.ReloadBulletCallback = ReloadBullet;
        Controller.EndReloadBulletCallback = EndReloadBullet;
    }
    private void ReloadBullet()
    {
        if (CurrentWeapon.CheckRunoutOfBullet())
        {
            CurrentWeapon.ReloadBullet();
        }
    }
    private void EndReloadBullet()
    {
        reloadingBullet = false;
        //   if (Controller.IsAiming == false) Controller.PlayerAnimator.SetLayerWeight(Controller.PlayerAnimator.GetLayerIndex(Controller.PistolLayerName), 0);

    }
    private void OnChangeWeapon(WeaponType type)
    {
        Controller.StartChangeWeapon(type);
        if (Controller.IsAiming) UserInputController.Instance.OnCancelAiming?.Invoke();
        Controller.ChangeWeaponDataCallback = () => SwitchWeapon(type);
    }
    private void SwitchWeapon(WeaponType type)
    {
        //Controller.OnEndChangeWeapon = () =>
        //{
        //    if (CurrentWeapon.CheckRunoutOfBullet())
        //    {
        //        ShowReloadBulletAnim();
        //    }
        //};
        WeaponData data = Resources.Load<WeaponData>(WeaponConfig.GetDataLink(type));
        if (data != null)
        {
            Debug.Log(type.ToString() + data.LinkPrefab);

            PlayerData.SetCurrentWeaponData(data);
            ActiveWeapon(data);
        }
        else
        {
            Debug.Log("????");
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
        Controller.EndAttackCallback = EndAttack;
    }
    private void CancelAiming()
    {
        CurrentWeapon.StopAttack(Controller.transform);
        //if (endAttack)
        //{
        //    Controller.StartAttackAnim(CurrentWeapon.Data.Type, false);
        //    Debug.Log(CurrentWeapon.Data.Type.ToString() + false);
        //}
    }
    bool endAttack = false;
    private void EndAttack()
    {
        endAttack = true;
        Controller.StartAttackAnim(CurrentWeapon.Data.Type, false);
    }
    private void ShowAnimAttack()
    {
        endAttack = false;
        if (CurrentWeapon.Data.Type == WeaponType.Melee)
        {
            Controller.StartAttackAnim(CurrentWeapon.Data.Type, true);
        }
        else
        {
            CurrentWeapon.Attack(Controller.transform);

        }
        Controller.AttackCallback = () =>
        {
            CurrentWeapon.Attack(Controller.transform);
            if (CurrentWeapon.CheckRunoutOfBullet())
            {
                ShowReloadBulletAnim();
            }
        };
    }
    #endregion  
}
