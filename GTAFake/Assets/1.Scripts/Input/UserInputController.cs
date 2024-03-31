using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserInputController : MonoBehaviour
{
    public static UserInputController Instance;

    public Action<float, float> OnMovementJoystick;
    public Action<float, float> OnAimingJoystick;
    public Action<float, float> OnCameraAxisChange;
    public Action OnJumpBtnClick;
    public Action OnStartAiming;
    public Action OnCancelAiming;

    private void Awake()
    {
        Instance = this;
    }
}
