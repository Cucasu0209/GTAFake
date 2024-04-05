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
    }
}
