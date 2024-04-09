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
    public Action<int> OnChooseWeaponIndex;
    public Action OnJumpBtnClick;
    public Action OnStartAiming;
    public Action OnCancelAiming;
    public Action OnStartFlying;
    public Action OnEndFlying;

    private void Awake()
    {
        Instance = this;
    }
}
