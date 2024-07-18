using System.Collections;
using UnityEngine;
using System;
using System.Diagnostics;
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
    public readonly string MeeleFullBodyLayerName = "MeleeFullBody";
    public readonly string PistolLayerName = "Pistol";
    public readonly string PistolBodyLayerName = "PistolAnimBody";
    public readonly string RifleLayerName = "Rifle";
    public readonly string RifleBodyLayerName = "RifleAnimBody";
    public readonly string SkillLayerName = "Skill";

    [SerializeField] private Transform ForwardAxis;
    private Vector3 CurrentTarget;
    private int MeeleAttackIndex = 0;
    private float Speed = 0;

    [Header("Weapon")]
    public bool ChangingWeapon = false;
    public bool IsFiring = false;
    public WeaponType CurrentWeaponType;
    #endregion

    #region Callbacks
    public Action ChangeWeaponDataCallback;
    public Action AttackCallback;
    public Action EndAttackCallback;
    public Action ReloadBulletCallback;
    public Action EndReloadBulletCallback;
    public Action OnEndChangeWeapon;
    public Action OnTakeDmgSkill;
    public Action OnEndSkill;
    #endregion

    #region Monobehaviour
    private void Start()
    {
        if (PlayerAnimator == null) PlayerAnimator = GetComponentInChildren<Animator>();
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
        Speed = speed;
        if ((PlayerAnimator.GetLayerWeight(PlayerAnimator.GetLayerIndex(MeeleBodyLayerName)) == 1 && speed < 0.5f) || (PlayerAnimator.GetLayerWeight(PlayerAnimator.GetLayerIndex(MeeleFullBodyLayerName)) == 1 && speed > 0.5f))
        {
            StartAttackAnim(WeaponType.Melee, true);
        }
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
        StartCoroutine(IEndChangeWeapon(MeeleBodyLayerName, type == WeaponType.Melee && turnOn && Speed > 0.5f));
        StartCoroutine(IEndChangeWeapon(MeeleFullBodyLayerName, type == WeaponType.Melee && turnOn && Speed < 0.5f));
        StartCoroutine(IEndChangeWeapon(RifleBodyLayerName, type == WeaponType.Rifle && turnOn));
        StartCoroutine(IEndChangeWeapon(PistolBodyLayerName, type == WeaponType.Pistol && turnOn));
    }
    public void StartChangeWeapon(WeaponType type)
    {
        StartAttackAnim(type, true);
        StartCoroutine(IEndChangeWeapon(MeeleLayerName, type == WeaponType.Melee));
        StartCoroutine(IEndChangeWeapon(RifleLayerName, type == WeaponType.Rifle));
        StartCoroutine(IEndChangeWeapon(PistolLayerName, type == WeaponType.Pistol));

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
    IEnumerator IEndChangeWeapon(string Name, bool turnOn, float time = 0.15f)
    {
        int id = PlayerAnimator.GetLayerIndex(Name);

        float loopCount = 30;
        float intensity = PlayerAnimator.GetLayerWeight(PlayerAnimator.GetLayerIndex(Name));
        while (turnOn ? intensity < 1 : intensity > 0)
        {
            yield return new WaitForSeconds(time / loopCount);
            intensity = intensity + (turnOn ? 1 : -1) / loopCount;
            PlayerAnimator.SetLayerWeight(id, intensity);
        }

        PlayerAnimator.SetLayerWeight(id, turnOn ? 1 : 0);

    }
    public void SetAnimReload()
    {
        PlayerAnimator.SetTrigger("reload");
    }
    public void ActiveLayerSkill(bool active)
    {
        PlayerAnimator.SetLayerWeight(PlayerAnimator.GetLayerIndex(SkillLayerName), active ? 1 : 0);
    }
    public void SetNextMeeleAttack()
    {
        MeeleAttackIndex++;
        UnityEngine.Debug.Log(MeeleAttackIndex);
        MeeleAttackIndex %= 4;
        PlayerAnimator.SetFloat("AttackMeeleIndex", MeeleAttackIndex);
    }
    public void ResetMeeleAttackIndex()
    {
        MeeleAttackIndex = 0;
        PlayerAnimator.SetFloat("AttackMeeleIndex", MeeleAttackIndex);
    }

    public void PlaySkill()
    {
        PlayerAnimator.SetTrigger("skill");

    }
    public void EndSkill()
    {
        PlayerAnimator.SetTrigger("endskill");

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
