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
    [HideInInspector] public Transform ForwardAxis;

    [Header("Attack")]
    public GameObject Bullet;
    public Transform HeadGun;

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
    }


    private void OnDestroy()
    {
        UserInputController.Instance.OnAimingJoystick -= SetAimingState;
        UserInputController.Instance.OnCancelAiming -= CancelAiming;
        UserInputController.Instance.OnStartAiming -= StartAiming;
        UserInputController.Instance.OnStartFlying -= OnStartFlying;
        UserInputController.Instance.OnEndFlying -= OnEndFlying;
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

    float LastTimeShoot = 0;
    private void SetAimingState(float hz, float v)
    {
        IsAiming = true;
        SetAimingState(IsAiming);
        if (Time.time - LastTimeShoot > 0.2f)
        {
            LastTimeShoot = Time.time;
            GameObject newbu = LeanPool.Spawn(Bullet, HeadGun.position, Quaternion.identity);
            newbu.GetComponent<Bullet>().SetVelocity(transform.forward);
        }
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
