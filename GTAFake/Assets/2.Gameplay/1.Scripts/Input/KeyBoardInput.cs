using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class KeyBoardInput : MonoBehaviour
{
    float HzInput = 0;
    float vInput = 0;

    //Aiming
    bool IsMousePressing = false;
    Vector2 Mousepos;
    Vector3 ScreenSize;
    float MaxAxisValue;
    private void Awake()
    {
        ScreenSize = new Vector3(Screen.width / 2, Screen.height / 2);
        MaxAxisValue = Mathf.Max(Screen.width, Screen.height);
    }
    private void Update()
    {

        if (Input.GetKeyDown(KeyCode.Alpha1) && PlayerData.GetCurrentWeaponData().Type != WeaponType.Melee)
        {
            UserInputController.Instance.OnSwitchWeapon?.Invoke(WeaponType.Melee);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2) && PlayerData.GetCurrentWeaponData().Type != WeaponType.Pistol)
        {
            UserInputController.Instance.OnSwitchWeapon?.Invoke(WeaponType.Pistol);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3) && PlayerData.GetCurrentWeaponData().Type != WeaponType.Rifle)
        {
            UserInputController.Instance.OnSwitchWeapon?.Invoke(WeaponType.Rifle);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4) && PlayerData.GetCurrentWeaponData().Type != WeaponType.Special)
        {
            UserInputController.Instance.OnSwitchWeapon?.Invoke(WeaponType.Special);
        }



        if (Input.GetKeyDown(KeyCode.Z))
        {
            UserInputController.Instance.OnStartFlying?.Invoke();
        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            UserInputController.Instance.OnEndFlying?.Invoke();
        }
        if (Input.GetKey(KeyCode.Space))
        {
            UserInputController.Instance.OnJumpBtnClick?.Invoke();
        }


        if (Input.GetKey(KeyCode.W)) vInput = 1;
        else if (Input.GetKey(KeyCode.S)) vInput = -1;
        else vInput = 0;

        if (Input.GetKey(KeyCode.D)) HzInput = 1;
        else if (Input.GetKey(KeyCode.A)) HzInput = -1;
        else HzInput = 0;

        UserInputController.Instance.OnMovementJoystick?.Invoke(HzInput, vInput);

        if (Input.GetMouseButtonDown(0))
        {
            IsMousePressing = true;
            UserInputController.Instance.OnStartAiming?.Invoke();

        }
        if (Input.GetMouseButtonUp(0))
        {
            IsMousePressing = false;
            UserInputController.Instance.OnCancelAiming?.Invoke();
        }
        if (IsMousePressing)
        {
            Mousepos = Input.mousePosition - ScreenSize;
            UserInputController.Instance.OnAimingJoystick?.Invoke(Mousepos.x / MaxAxisValue, Mousepos.y / MaxAxisValue);

        }

        if (Input.GetKeyDown(KeyCode.Q)) UserInputController.Instance.OnSwitchModel?.Invoke();
    }
}
