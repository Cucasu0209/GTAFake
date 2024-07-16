
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float DefaultSpeed = 9;
    [SerializeField] private float CurrentSpeed = 7;
    private PlayerController Controller;
    private float LastMoveInput = 0;
    public bool Stop = false;

    private void Start()
    {
        CurrentSpeed = DefaultSpeed;
        Controller = GetComponent<PlayerController>();
        UserInputController.Instance.OnMovementJoystick += MovePlayer;
    }
    private void OnDestroy()
    {
        UserInputController.Instance.OnMovementJoystick -= MovePlayer;
    }
    public void SetScaleSpeed(float scale)
    {
        CurrentSpeed = DefaultSpeed * scale;
    }
    private void MovePlayer(float hzInput, float vInput)
    {
        //if (Controller.IsAiming == false)
        //{
        if (!Stop)
        {
            Vector2 MoveInput = new Vector2(hzInput, vInput);
            Vector2 Direction = (new Vector2(hzInput, vInput)).normalized;
            if (MoveInput.magnitude > 0.1f)
            {

                float rotation = Mathf.Atan2(Direction.x, Direction.y) * Mathf.Rad2Deg + Controller.GetCameraAngle();
                rotation = (rotation % 360 + 360) % 360;
                float currentRotation = (transform.eulerAngles.y % 360 + 360) % 360;
                if (Mathf.Abs(currentRotation - rotation) > 180)
                {
                    if (currentRotation < 180) currentRotation += 360;
                    else rotation += 360;
                }
                transform.eulerAngles = Vector3.up * Mathf.Lerp(currentRotation, rotation, 10 * Time.deltaTime);
                Controller.charController.Move(transform.forward * CurrentSpeed * Time.deltaTime);
            }
            LastMoveInput = Mathf.Lerp(LastMoveInput, MoveInput.magnitude, 20 * Time.deltaTime);
            Controller.SetSpeedAnim(LastMoveInput);
        }

        //}
    }

}
