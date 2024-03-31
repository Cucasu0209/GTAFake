using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;
using DG.Tweening;
public class PlayerAiming : MonoBehaviour
{
    private float Speed = 5;
    [SerializeField] private float AimingSpeed = 5;
    private PlayerController Controller;
    //[SerializeField] private PlayerEnemyDetection detection;
    private void Start()
    {
        Speed = AimingSpeed;
        Controller = GetComponent<PlayerController>();
        UserInputController.Instance.OnMovementJoystick += MovePlayer;
        UserInputController.Instance.OnAimingJoystick += Aim;
        UserInputController.Instance.OnCancelAiming += HideAimingZone;
    }
    private void OnDestroy()
    {
        UserInputController.Instance.OnMovementJoystick -= MovePlayer;
        UserInputController.Instance.OnAimingJoystick -= Aim;
        UserInputController.Instance.OnCancelAiming -= HideAimingZone;
    }
    float lasttimeShot = 0;
    private void Aim(float hzInput, float vInput)
    {
        if (Controller.IsAiming == true)
        {
            Vector2 AimInput = new Vector2(hzInput, vInput);
            Vector2 Direction = (new Vector2(hzInput, vInput)).normalized;
            if (AimInput.magnitude > 0.1f)
            {

                float rotation = Mathf.Atan2(Direction.x, Direction.y) * Mathf.Rad2Deg + Controller.ForwardAxis.eulerAngles.y;
                rotation = (rotation % 360 + 360) % 360;
                float currentRotation = (transform.eulerAngles.y % 360 + 360) % 360;
                if (Mathf.Abs(currentRotation - rotation) > 180)
                {
                    if (currentRotation < 180) currentRotation += 360;
                    else rotation += 360;
                }
                transform.eulerAngles = Vector3.up * Mathf.Lerp(currentRotation, rotation, 10 * Time.deltaTime);
            }

            //shoot
        }
    }
    private void MovePlayer(float hzInput, float vInput)
    {
        if (Controller.IsAiming == true)
        {

            Vector2 MoveInput = new Vector2(hzInput, vInput);

            if (MoveInput.magnitude > 0.1f)
            {
                //move
                Vector2 Direction = (new Vector2(hzInput, vInput)).normalized;
                float rotation = Mathf.Atan2(Direction.x, Direction.y) * Mathf.Rad2Deg + Controller.ForwardAxis.eulerAngles.y;
                Direction = new Vector2(Mathf.Sin(rotation * Mathf.Deg2Rad), Mathf.Cos(rotation * Mathf.Deg2Rad)).normalized;
                Controller.charController.Move(new Vector3(Direction.x, 0, Direction.y) * Speed * Time.deltaTime);

                //Anim
                rotation = rotation - transform.eulerAngles.y;
                Direction = new Vector2(Mathf.Sin(rotation * Mathf.Deg2Rad), Mathf.Cos(rotation * Mathf.Deg2Rad)).normalized;
                Controller.SetAimingMovement(Direction.x, Direction.y);
            }
            else
            {
                Controller.SetAimingMovement(0, 0);

            }


        }
    }

    private void HideAimingZone()
    {
    }
}
