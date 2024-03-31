using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobileFeatureInput : MonoBehaviour
{
    public FixedJoystick MovementJoystick;
    public FixedJoystick AimingJoystick;
    public bool NotAimingNow = true;
    private void Update()
    {
        UserInputController.Instance.OnMovementJoystick?.Invoke(MovementJoystick.Horizontal, MovementJoystick.Vertical);
        if (AimingJoystick.Horizontal * AimingJoystick.Horizontal + AimingJoystick.Vertical * AimingJoystick.Vertical < 0.2f)
        {
            if (NotAimingNow == false)
            {
                UserInputController.Instance.OnCancelAiming?.Invoke();
                NotAimingNow = true;
            }
        }
        else
        {
            UserInputController.Instance.OnAimingJoystick?.Invoke(AimingJoystick.Horizontal, AimingJoystick.Vertical);
            NotAimingNow = false;
        }
    }
}
