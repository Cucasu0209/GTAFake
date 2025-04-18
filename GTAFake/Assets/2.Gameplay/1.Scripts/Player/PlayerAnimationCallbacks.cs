using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class PlayerAnimationCallbacks : MonoBehaviour
{
    [HideInInspector] public PlayerController Controller;
    public CharacterType CharacterType;
    public Transform HandRight;
    public Transform HandLeft;
    public MultiAimConstraint RHandConstraint;

    public void Start()
    {
        if (Controller == null) Controller = GetComponentInParent<PlayerController>();
        UserInputController.Instance.OnStartAiming += OnStartAiming;
        UserInputController.Instance.OnCancelAiming += OnCancelAiming;
    }
    public void OnDestroy()
    {
        UserInputController.Instance.OnStartAiming -= OnStartAiming;
        UserInputController.Instance.OnCancelAiming -= OnCancelAiming;
    }
    private void SetConstraintAimingState(bool isActive)
    {
        if (Controller.CurrentWeaponType == WeaponType.Pistol || Controller.CurrentWeaponType == WeaponType.Rifle)
        {
            RHandConstraint.weight = isActive ? 1 : 0;
        }
        else
        {
            RHandConstraint.weight = 0;
        }
    }
    private void OnStartAiming()
    {
        if (Controller.ChangingWeapon == false)
        {
            SetConstraintAimingState(true);
        }

    }
    private void OnCancelAiming()
    {
        SetConstraintAimingState(false);
    }

    #region Animation callbacks (dont delete me please, unless you will regret)
    public void ChangeWeaponData()
    {
        Controller.ChangeWeaponDataCallback?.Invoke();
    }
    public void EndChangeWeaponAnim()
    {
        Controller.EndChangeWeapon();
    }
    public void Attack()
    {
        Controller.AttackCallback?.Invoke();
    }
    public void EndAttack(string log)
    {
        Debug.Log(log);
        Controller.EndAttackCallback?.Invoke();
        if (Controller.IsFiring == false)
        {

            SetConstraintAimingState(false);

        }
    }
    public void StartReloadBullet()
    {
        DOVirtual.DelayedCall(0.01f, () =>
        {
            SetConstraintAimingState(false);
        });
    }
    public void ReloadBullet()
    {
        Controller.ReloadBulletCallback?.Invoke();
    }
    public void EndReloadBullet()
    {
        DOVirtual.DelayedCall(0.01f, () =>
        {
            SetConstraintAimingState(Controller.IsAiming);
        });
        Controller.EndReloadBulletCallback?.Invoke();
    }
    public void TakeDmgSkill()
    {
        Controller.OnTakeDmgSkill?.Invoke();
    }
    public void EndSkill()
    {
        Controller.OnEndSkill?.Invoke();
    }
    #endregion
}
