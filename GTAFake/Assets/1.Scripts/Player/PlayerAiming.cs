using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;
using DG.Tweening;
public class PlayerAiming : MonoBehaviour
{
    [SerializeField] private float Speed = 5;
    float CurrenthzInput = 0;
    float CurrentvInput = 0;
    private PlayerController Controller;
    //[SerializeField] private PlayerEnemyDetection detection;
    private void Start()
    {
        Controller = GetComponent<PlayerController>();
        UserInputController.Instance.OnMovementJoystick += MovePlayer;
        UserInputController.Instance.OnAimingJoystick += Aim;
        UserInputController.Instance.OnStartAiming += OnStartAiming;
    }
    private void OnDestroy()
    {
        UserInputController.Instance.OnMovementJoystick -= MovePlayer;
        UserInputController.Instance.OnAimingJoystick -= Aim;
        UserInputController.Instance.OnStartAiming -= OnStartAiming;
    }
    private void Aim(float hzInput, float vInput)
    {
        if (Controller.IsAiming == true)
        {
            transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, Controller.ForwardAxis.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
        }
    }


    private void OnStartAiming()
    {
        CurrenthzInput = 0;
        CurrentvInput = 0;
    }
    private void MovePlayer(float hzInput, float vInput)
    {
        if (Controller.IsAiming == true && Controller.IsFlying == false)
        {

            Vector2 MoveInput = new Vector2(hzInput, vInput);

            if (MoveInput.magnitude > 0.1f)
            {
                //move
                Vector2 Direction = (new Vector2(hzInput, vInput)).normalized;
                float rotation = Mathf.Atan2(Direction.x, Direction.y) * Mathf.Rad2Deg + Controller.ForwardAxis.eulerAngles.y;
                Direction = new Vector2(Mathf.Sin(rotation * Mathf.Deg2Rad), Mathf.Cos(rotation * Mathf.Deg2Rad)).normalized;
                Controller.charController.Move(new Vector3(Direction.x, 0, Direction.y) * Speed * Time.deltaTime);


            }
            CurrenthzInput = Mathf.Lerp(CurrenthzInput, hzInput, 10 * Time.deltaTime);
            CurrentvInput = Mathf.Lerp(CurrentvInput, vInput, 10 * Time.deltaTime);
            Controller.SetAimingMovement(CurrenthzInput, CurrentvInput);
        }
    }


}
