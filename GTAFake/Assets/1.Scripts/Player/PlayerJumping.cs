using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJumping : MonoBehaviour
{
    private PlayerController Controller;
    [SerializeField] private float JumpStartVeloc = 10;

    private void Start()
    {
        Controller = GetComponent<PlayerController>();
        UserInputController.Instance.OnJumpBtnClick += Jump;
    }

    private void OnDestroy()
    {
        UserInputController.Instance.OnJumpBtnClick -= Jump;

    }
    private void Jump()
    {
        if (Controller.IsGrounded() && Controller.IsFlying == false)
        {
            Controller.Velocity += Vector3.up * (JumpStartVeloc - Controller.Velocity.y);
            Controller.SetJumpAnim();
        }
    }
}
