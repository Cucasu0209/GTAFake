using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJumping : MonoBehaviour
{
    private PlayerController Controller;
    public float JumpStartVeloc = 13;

    public void Start()
    {
        Controller = GetComponent<PlayerController>();
        UserInputController.Instance.OnJumpBtnClick += Jump;
    }
    public void OnDestroy()
    {
        UserInputController.Instance.OnJumpBtnClick -= Jump;

    }
    public void Jump()
    {
        if (Controller.IsGrounded())
        {
            Controller.Velocity += Vector3.up * (JumpStartVeloc - Controller.Velocity.y);
        }
    }
    public void AddForceJump()
    {
    }
}
