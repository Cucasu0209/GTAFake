using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobileFeatureInput : MonoBehaviour
{
    public FixedJoystick MovementJoystick;
    public FixedJoystick AimingJoystick;
    private bool IsMousePressing = false;

    private void Start()
    {
        AimingJoystick.OnStartDrag += OnStartAiming;
        AimingJoystick.OnEndDrag += OnEndAiming;
    }

    private void OnDestroy()
    {
        AimingJoystick.OnStartDrag -= OnStartAiming;
        AimingJoystick.OnEndDrag -= OnEndAiming;
    }


    private void OnStartAiming()
    {
        UserInputController.Instance.OnStartAiming?.Invoke();
        IsMousePressing = true;
    }
    private void OnEndAiming()
    {
        UserInputController.Instance.OnCancelAiming?.Invoke();
        IsMousePressing = false;

    }
    private void Update()
    {
        UserInputController.Instance.OnMovementJoystick?.Invoke(MovementJoystick.Horizontal, MovementJoystick.Vertical);
        //if (AimingJoystick.Horizontal * AimingJoystick.Horizontal + AimingJoystick.Vertical * AimingJoystick.Vertical < 0.2f)
        //{
        //    if (NotAimingNow == false)
        //    {
        //        UserInputController.Instance.OnCancelAiming?.Invoke();
        //        NotAimingNow = true;
        //    }
        //}
        //else
        //{
        //    UserInputController.Instance.OnAimingJoystick?.Invoke(AimingJoystick.Horizontal, AimingJoystick.Vertical);
        //    NotAimingNow = false;
        //}
        if (IsMousePressing)
        {
            UserInputController.Instance.OnAimingJoystick?.Invoke(AimingJoystick.Horizontal, AimingJoystick.Vertical);
            if (Mathf.Abs(AimingJoystick.Horizontal) > 0.5f)
                UserInputController.Instance.OnCameraAxisChange(AimingJoystick.Horizontal * 10, 0);
        }
    }
}
