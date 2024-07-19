using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using AmplifyImpostors;
public class PlayerWeaponManager : MonoBehaviour
{
    private PlayerController Controller;
    [SerializeField] public Transform HandRight;
    [SerializeField] public Transform HandLeft;
    [HideInInspector] public BaseWeapon CurrentWeapon;
    public List<BaseWeapon> CurrentWeapons = new List<BaseWeapon>();

    public Action ChangeWeaponDataCallback;
    private bool reloadingBullet = false;
    private bool CanAttack = true;
    private bool CanSwitchWeapon = true;


    #region Monobehaviour
    IEnumerator Start()
    {
        Controller = GetComponent<PlayerController>();
        if (HandRight == null) HandRight = GetComponentInChildren<PlayerAnimationCallbacks>().HandRight;
        if (HandLeft == null) HandLeft = GetComponentInChildren<PlayerAnimationCallbacks>().HandLeft;
        UserInputController.Instance.OnSwitchWeapon += OnChangeWeapon;
        UserInputController.Instance.OnAimingJoystick += SetAimingState;
        UserInputController.Instance.OnStartAiming += StartAiming;
        UserInputController.Instance.OnCancelAiming += CancelAiming;
        UserInputController.Instance.OnSwitchModel += ChangeModel;
        yield return null;
        UserInputController.Instance.OnSwitchWeapon?.Invoke(WeaponType.Melee);
        yield return null;
        if (HandRight == null) HandRight = GetComponentInChildren<PlayerAnimationCallbacks>().HandRight;
        if (HandLeft == null) HandLeft = GetComponentInChildren<PlayerAnimationCallbacks>().HandLeft;
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
    public void ChangeModel(CharacterType type)
    {
        //StartCoroutine(aaaaaa());
    }
    IEnumerator aaaaaa()
    {
        yield return new WaitForSeconds(0.5f);
        HandRight = GetComponentInChildren<PlayerAnimationCallbacks>().HandRight;
        HandLeft = GetComponentInChildren<PlayerAnimationCallbacks>().HandLeft;
    }
    public void SetCanAttack(bool canAttack)
    {
        CanAttack = canAttack;
    }
    public void SetCanSwitchWeapon(bool canSwitchWeapon)
    {
        CanSwitchWeapon = canSwitchWeapon;
    }
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
        if (CanSwitchWeapon)
        {
            Controller.StartChangeWeapon(type);
            if (Controller.IsAiming) UserInputController.Instance.OnCancelAiming?.Invoke();
            Controller.ChangeWeaponDataCallback = () => SwitchWeapon(type);
        }

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
                if ((GetComponentInChildren<PlayerAnimationCallbacks>().CharacterType == CharacterType.Mech5 || GetComponentInChildren<PlayerAnimationCallbacks>().CharacterType == CharacterType.Mech6))
                {
                    CurrentWeapon.transform.localPosition = Vector3.up * 10000;

                    HandRight = GetComponentInChildren<PlayerAnimationCallbacks>().HandRight;
                    HandLeft = GetComponentInChildren<PlayerAnimationCallbacks>().HandLeft;

                    if (CurrentWeapon is Gun)
                    {
                        ((Gun)CurrentWeapon).HeadGun = new Transform[] { HandRight, HandLeft };
                    }
                }
                else
                {
                    CurrentWeapon.transform.localPosition = Vector3.zero;
                    if (CurrentWeapon is Gun)
                    {
                        ((Gun)CurrentWeapon).HeadGun = new Transform[] { ((Gun)CurrentWeapon).DefaultHeadGun };
                    }

                }
                return;
            }
        }

        BaseWeapon wr = Resources.Load<BaseWeapon>(data.LinkPrefab);
        if (wr != null)
        {
            BaseWeapon w = Instantiate(wr, HandRight);
            w.Data = data;
            w.transform.localPosition = Vector3.zero;
            CurrentWeapons.Add(w);
            if (CurrentWeapon != null) CurrentWeapon.gameObject.SetActive(false);
            CurrentWeapon = w;
            if ((GetComponentInChildren<PlayerAnimationCallbacks>().CharacterType == CharacterType.Mech5 || GetComponentInChildren<PlayerAnimationCallbacks>().CharacterType == CharacterType.Mech6))
            {
                w.transform.localPosition = Vector3.up * 10000;

                HandRight = GetComponentInChildren<PlayerAnimationCallbacks>().HandRight;
                HandLeft = GetComponentInChildren<PlayerAnimationCallbacks>().HandLeft;
                if (w is Gun)
                {
                    ((Gun)w).HeadGun = new Transform[] { HandRight, HandLeft };
                }
            }
            else
            {
                w.transform.localPosition = Vector3.zero;
                if (w is Gun) ((Gun)CurrentWeapon).HeadGun = new Transform[] { ((Gun)CurrentWeapon).DefaultHeadGun };

            }
        }
    }
    private void SetAimingState(float hz, float v)
    {
        if (CanAttack)
        {
            ShowAnimAttack();
        }
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
        if (Controller.IsAiming == false)
            Controller.StartAttackAnim(CurrentWeapon.Data.Type, false);
        if (CurrentWeapon.Data.Type == WeaponType.Melee)
            Controller.SetNextMeeleAttack();
    }
    private void ShowAnimAttack()
    {
        endAttack = false;
        if (CurrentWeapon.Data.Type == WeaponType.Melee)
        {
            Controller.StartAttackAnim(CurrentWeapon.Data.Type, true);
            Controller.AttackCallback = () =>
            {
                CurrentWeapon.Attack(Controller.transform);
                if (CurrentWeapon.CheckRunoutOfBullet())
                {
                    ShowReloadBulletAnim();
                }
            };
        }
        else
        {
            CurrentWeapon.Attack(Controller.transform);

        }

    }

    public void HideWeapon()
    {
        CurrentWeapon.gameObject.SetActive(false);

    }
    public void ShowWeapon()
    {
        CurrentWeapon.gameObject.SetActive(true);

    }
    #endregion  
}
