using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Lean.Pool;
using System;
public class PlayerController : MonoBehaviour
{
    public Animator PlayerAnimator;
    public CharacterController charController;

    [Header("Gravity")]
    [SerializeField] public float Gravity = -50;
    public readonly float DefaultGravity = -50;
    public Vector3 Velocity;
    [SerializeField] private LayerMask GroundMask;
    [SerializeField] private float GroundYOffset;

    [Header("Movement and Aiming")]
    public bool IsAiming;/* { get; private set; }*/
    public readonly string AimLayerName = "Aiming";
    private Transform ForwardAxis;
    private Vector3 CurrentTarget;

    [Header("Flying")]
    public bool IsFlying;


    #region Monobehaviour
    private void Start()
    {
        ForwardAxis = Camera.main.transform;
        UserInputController.Instance.OnAimingJoystick += SetAimingState;
        UserInputController.Instance.OnCancelAiming += CancelAiming;
        UserInputController.Instance.OnStartAiming += StartAiming;
        UserInputController.Instance.OnStartFlying += OnStartFlying;
        UserInputController.Instance.OnEndFlying += OnEndFlying;
        UserInputController.Instance.OnChangeTargetAim += UpdateTarget;
    }


    private void OnDestroy()
    {
        UserInputController.Instance.OnAimingJoystick -= SetAimingState;
        UserInputController.Instance.OnCancelAiming -= CancelAiming;
        UserInputController.Instance.OnStartAiming -= StartAiming;
        UserInputController.Instance.OnStartFlying -= OnStartFlying;
        UserInputController.Instance.OnEndFlying -= OnEndFlying;
        UserInputController.Instance.OnChangeTargetAim -= UpdateTarget;

    }
    private void Update()
    {
        ApplyGravity();
    }
    #endregion

    #region Animation
    public void SetSpeedAnim(float speed)
    {
        PlayerAnimator.SetFloat("speed", speed);
    }
    public void SetAimingMovement(float hzInput, float vInput)
    {
        PlayerAnimator.SetFloat("hzInput", hzInput);
        PlayerAnimator.SetFloat("vInput", vInput);

    }
    public void SetAimingState(bool IsAiming)
    {
        PlayerAnimator.SetBool("aiming", IsAiming);

    }
    public void SetJumpAnim()
    {
        PlayerAnimator.SetTrigger("jump");
    }
    public void SetHandInWeaponAnim(int index)
    {
        for (int i = 1; i < PlayerAnimator.layerCount; i++)
        {
            PlayerAnimator.SetLayerWeight(i, i == index ? 1 : 0);
        }
    }
    public void SetAttackAnim()
    {
        PlayerAnimator.SetTrigger("attack");
    }
    #endregion

    #region Gravity
    public bool IsGrounded()
    {
        Vector3 SpherePos = transform.position - Vector3.up * GroundYOffset;
        if (Physics.CheckSphere(SpherePos, charController.radius - 0.05f, GroundMask))
        {
            return true;
        }
        return false;
    }
    void ApplyGravity()
    {
        if (IsGrounded() == false)
        {
            Velocity.y += Gravity * Time.deltaTime;
        }
        else if (Velocity.y < 0)
        {
            Velocity.y = 0;
        }

        charController.Move(Velocity * Time.deltaTime);
    }
    #endregion

    #region Aiming
    public void UpdateTarget(Vector3 position)
    {
        CurrentTarget = position;
    }
    public float GetAngle()
    {
        Vector3 dir = CurrentTarget - transform.position;
        float angle = -Vector2.SignedAngle(new Vector2(dir.z, dir.x), Vector2.right);
        return angle;
    }
    public float GetCameraAngle() => ForwardAxis.eulerAngles.y;
    float LastTimeShoot = 0;
    private void SetAimingState(float hz, float v)
    {
        IsAiming = true;
        SetAimingState(IsAiming);
    }
    private void StartAiming()
    {
        LastTimeShoot = Time.time;
    }
    private void CancelAiming()
    {
        IsAiming = false;
        SetAimingState(IsAiming);
    }
    #endregion

    #region Flying
    private void OnStartFlying()
    {
        Gravity = 0;
        Velocity.y = 0;
        IsFlying = true;
    }
    private void OnEndFlying()
    {
        Gravity = DefaultGravity;
        IsFlying = false;
    }
    #endregion
}
