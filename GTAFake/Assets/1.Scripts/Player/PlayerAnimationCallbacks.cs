using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class PlayerAnimationCallbacks : MonoBehaviour
{
    [HideInInspector] public PlayerController Controller;
    public MultiAimConstraint RhandConstraint;
    public TwoBoneIKConstraint LhandConstraint;
    public MultiAimConstraint HeadConstraint;
    public MultiAimConstraint ShoulderConstraint;

    public Transform RightHand;

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
        RhandConstraint.weight = isActive ? 1.0f : 0;
        LhandConstraint.weight = isActive ? 1.0f : 0;
        HeadConstraint.weight = isActive ? 1.0f : 0;
        ShoulderConstraint.weight = isActive ? 1.0f : 0;
    }
    private void OnStartAiming()
    {
        if (Controller.ChangingWeapon == false)
        {
            if (Controller.GetComponent<PlayerWeaponManager>().CurrentWeapon is Gun)
            {
                DOVirtual.DelayedCall(0.01f, () =>
                {
                    SetConstraintAimingState(true);
                });


            }
            else
            {
                SetConstraintAimingState(false);
            }

            Controller.PlayerAnimator.SetLayerWeight(Controller.PlayerAnimator.GetLayerIndex(Controller.AimLayerName), 1);
        }
    }
    private void OnCancelAiming()
    {
        //  SetConstraintAimingState(false);
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
    public void EndAttack()
    {
        Controller.EndAttackCallback?.Invoke();
        if (Controller.IsFiring == false)
        {
            DOVirtual.DelayedCall(0.01f, () =>
            {
                SetConstraintAimingState(false);
            });
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
    #endregion
}
