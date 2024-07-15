using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Lean.Pool;
using System;
using System.Reflection;
public class PlayerController : MonoBehaviour
{
    public Animator PlayerAnimator;
    public CharacterController charController;

    #region Variables

    [Header("Gravity")]
    [SerializeField] public float Gravity = -50;
    public readonly float DefaultGravity = -50;
    public Vector3 Velocity;
    [SerializeField] private LayerMask GroundMask;
    [SerializeField] private float GroundYOffset;

    [Header("Movement and Aiming")]
    public bool IsAiming;
    public readonly string MeeleLayerName = "Melee";
    public readonly string MeeleBodyLayerName = "MeleeAnimBody";
    public readonly string PistolLayerName = "Pistol";
    public readonly string PistolBodyLayerName = "PistolAnimBody";
    public readonly string RifleLayerName = "Rifle";
    public readonly string RifleBodyLayerName = "RifleAnimBody";

    [SerializeField] private Transform ForwardAxis;
    private Vector3 CurrentTarget;

    //[Header("Flying")]
    //public bool IsFlying;
    [Header("Weapon")]
    public bool ChangingWeapon = false;
    public bool IsFiring = false;
    private WeaponType CurrentWeaponType;
    #endregion

    #region Callbacks
    public Action ChangeWeaponDataCallback;
    public Action AttackCallback;
    public Action EndAttackCallback;
    public Action ReloadBulletCallback;
    public Action EndReloadBulletCallback;
    public Action OnEndChangeWeapon;
    #endregion

    #region Monobehaviour
    private void Start()
    {
        ForwardAxis = Camera.main.transform;
        UserInputController.Instance.OnAimingJoystick += SetAimingState;
        UserInputController.Instance.OnCancelAiming += CancelAiming;
        UserInputController.Instance.OnStartAiming += StartAiming;
        //UserInputController.Instance.OnStartFlying += OnStartFlying;
        //UserInputController.Instance.OnEndFlying += OnEndFlying;
        UserInputController.Instance.OnChangeTargetAim += UpdateTarget;
    }


    private void OnDestroy()
    {
        UserInputController.Instance.OnAimingJoystick -= SetAimingState;
        UserInputController.Instance.OnCancelAiming -= CancelAiming;
        UserInputController.Instance.OnStartAiming -= StartAiming;
        //UserInputController.Instance.OnStartFlying -= OnStartFlying;
        //UserInputController.Instance.OnEndFlying -= OnEndFlying;
        UserInputController.Instance.OnChangeTargetAim -= UpdateTarget;

    }
    private void Update()
    {
        ApplyGravity();
    }
    #endregion

    #region Animations
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
    public void StartAttackAnim(WeaponType type, bool turnOn)
    {
        switch (type)
        {
            case WeaponType.Melee:
                PlayerAnimator.SetLayerWeight(PlayerAnimator.GetLayerIndex(MeeleBodyLayerName), turnOn ? 1 : 0);
                PlayerAnimator.SetLayerWeight(PlayerAnimator.GetLayerIndex(RifleBodyLayerName), 0);
                PlayerAnimator.SetLayerWeight(PlayerAnimator.GetLayerIndex(PistolBodyLayerName), 0);
                break;
            case WeaponType.Pistol:
                PlayerAnimator.SetLayerWeight(PlayerAnimator.GetLayerIndex(MeeleBodyLayerName), 0);
                PlayerAnimator.SetLayerWeight(PlayerAnimator.GetLayerIndex(RifleBodyLayerName), 0);
                PlayerAnimator.SetLayerWeight(PlayerAnimator.GetLayerIndex(PistolBodyLayerName), turnOn ? 1 : 0);
                break;
            case WeaponType.Rifle:
                PlayerAnimator.SetLayerWeight(PlayerAnimator.GetLayerIndex(MeeleBodyLayerName), 0);
                PlayerAnimator.SetLayerWeight(PlayerAnimator.GetLayerIndex(PistolBodyLayerName), 0);
                PlayerAnimator.SetLayerWeight(PlayerAnimator.GetLayerIndex(RifleBodyLayerName), turnOn ? 1 : 0);
                break;
            default:
                PlayerAnimator.SetLayerWeight(PlayerAnimator.GetLayerIndex(MeeleBodyLayerName), turnOn ? 1 : 0);
                PlayerAnimator.SetLayerWeight(PlayerAnimator.GetLayerIndex(RifleBodyLayerName), 0);
                PlayerAnimator.SetLayerWeight(PlayerAnimator.GetLayerIndex(PistolBodyLayerName), 0);
                break;

        }
    }
    public void StartChangeWeapon(WeaponType type)
    {
        StartAttackAnim(type, true);
        switch (type)
        {
            case WeaponType.Melee:
                PlayerAnimator.SetLayerWeight(PlayerAnimator.GetLayerIndex(MeeleLayerName), 1);
                PlayerAnimator.SetLayerWeight(PlayerAnimator.GetLayerIndex(RifleLayerName), 0);
                PlayerAnimator.SetLayerWeight(PlayerAnimator.GetLayerIndex(PistolLayerName), 0);
                break;
            case WeaponType.Pistol:
                PlayerAnimator.SetLayerWeight(PlayerAnimator.GetLayerIndex(MeeleLayerName), 0);
                PlayerAnimator.SetLayerWeight(PlayerAnimator.GetLayerIndex(RifleLayerName), 0);
                PlayerAnimator.SetLayerWeight(PlayerAnimator.GetLayerIndex(PistolLayerName), 1);
                break;
            case WeaponType.Rifle:
                PlayerAnimator.SetLayerWeight(PlayerAnimator.GetLayerIndex(MeeleLayerName), 0);
                PlayerAnimator.SetLayerWeight(PlayerAnimator.GetLayerIndex(PistolLayerName), 0);
                PlayerAnimator.SetLayerWeight(PlayerAnimator.GetLayerIndex(RifleLayerName), 1);
                break;
            default:
                PlayerAnimator.SetLayerWeight(PlayerAnimator.GetLayerIndex(MeeleLayerName), 1);
                PlayerAnimator.SetLayerWeight(PlayerAnimator.GetLayerIndex(RifleLayerName), 0);
                PlayerAnimator.SetLayerWeight(PlayerAnimator.GetLayerIndex(PistolLayerName), 0);
                break;

        }

        PlayerAnimator.SetTrigger("changeweapon");
        CurrentWeaponType = type;
        ChangingWeapon = true;
    }
    public void EndChangeWeapon()
    {
        // StartCoroutine(IEndChangeWeapon());
        ChangingWeapon = false;
        OnEndChangeWeapon?.Invoke();
        StartAttackAnim(CurrentWeaponType, false);
        if (IsAiming) UserInputController.Instance.OnStartAiming?.Invoke();
    }
    IEnumerator IEndChangeWeapon()
    {
        int id = PlayerAnimator.GetLayerIndex("ChangeWeapon");
        float intensity = 1;
        float loopCount = 30;
        while (intensity > 0)
        {
            yield return new WaitForSeconds(0.3f / loopCount);
            intensity -= 1 / loopCount;
            PlayerAnimator.SetLayerWeight(id, intensity);
        }
    }
    public void SetAnimReload()
    {
        PlayerAnimator.SetTrigger("reload");
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
        IsFiring = true;
        if (ChangingWeapon == false)
            LastTimeShoot = Time.time;
    }
    private void CancelAiming()
    {
        IsAiming = false;
        IsFiring = false;
        SetAimingState(IsAiming);
    }
    #endregion

    #region Flying
    //private void OnStartFlying()
    //{
    //    Gravity = 0;
    //    Velocity.y = 0;
    //    IsFlying = true;
    //}
    //private void OnEndFlying()
    //{
    //    Gravity = DefaultGravity;
    //    IsFlying = false;
    //}
    #endregion
}
