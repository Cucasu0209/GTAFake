using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class MobileFeatureInput : MonoBehaviour
{
    public FixedJoystick MovementJoystick;
    public FixedJoystick AimingJoystick;
    public Button JumpBtn;
    private bool IsMousePressing = false;

    private void Start()
    {
        JumpBtn.onClick.AddListener(() => UserInputController.Instance.OnJumpBtnClick?.Invoke());
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
        if (new Vector2(MovementJoystick.Horizontal, MovementJoystick.Vertical).magnitude > 0.7f)
            UserInputController.Instance.OnMovementJoystick?.Invoke(MovementJoystick.Horizontal, MovementJoystick.Vertical);
        else
            UserInputController.Instance.OnMovementJoystick?.Invoke(0, 0);

        if (IsMousePressing)
        {
            UserInputController.Instance.OnAimingJoystick?.Invoke(AimingJoystick.Horizontal, AimingJoystick.Vertical);
            if (Mathf.Abs(AimingJoystick.Horizontal) > 0.8f)
                UserInputController.Instance.OnCameraAxisChange(AimingJoystick.Horizontal * 10, 0);
        }
    }
}
