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

    private void OnStartAiming()
    {
        if (Controller.ChangingWeapon == false)
        {
            if (Controller.GetComponent<PlayerWeaponManager>().CurrentWeapon is Gun)
            {
                DOVirtual.DelayedCall(0.01f, () =>
                {
                    RhandConstraint.weight = 1.0f;
                    LhandConstraint.weight = 1.0f;
                    HeadConstraint.weight = 1.0f;
                    ShoulderConstraint.weight = 1.0f;
                });


            }
            else
            {
                RhandConstraint.weight = 0f;
                LhandConstraint.weight = 0f;
                HeadConstraint.weight = 0f;
                ShoulderConstraint.weight = 0f;
            }

            Controller.PlayerAnimator.SetLayerWeight(Controller.PlayerAnimator.GetLayerIndex(Controller.AimLayerName), 1);
        }
    }
    private void OnCancelAiming()
    {
        RhandConstraint.weight = 0f;
        LhandConstraint.weight = 0f;

        HeadConstraint.weight = 0f;
        ShoulderConstraint.weight = 0f;
    }
    #region Animation callbacks
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
    #endregion
}
