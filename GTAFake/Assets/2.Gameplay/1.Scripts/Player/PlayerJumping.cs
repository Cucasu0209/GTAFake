using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJumping : MonoBehaviour
{
    private PlayerController Controller;
    private bool CanJump = true;
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
    public void SetCanJump(bool capable)
    {
        CanJump = capable;
    }
    private void Jump()
    {
        if (Controller.IsGrounded() && CanJump)
        {
            Controller.Velocity += Vector3.up * (JumpStartVeloc - Controller.Velocity.y);
            Controller.SetJumpAnim();
        }
    }
}
