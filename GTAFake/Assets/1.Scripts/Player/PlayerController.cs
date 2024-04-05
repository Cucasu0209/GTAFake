using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Lean.Pool;
public class PlayerController : MonoBehaviour
{
    public Animator PlayerAnimator;
    public CharacterController charController;

    [Header("Gravity")]
    [SerializeField] public float Gravity = -50;
    public readonly float DefaultGravity = -50;
    [HideInInspector] public Vector3 Velocity;
    [SerializeField] private LayerMask GroundMask;
    [SerializeField] private float GroundYOffset;

    [Header("Movement and Aiming")]
    public bool IsAiming;/* { get; private set; }*/
    public bool IsAutoAiming = false;
    public readonly string AimLayerName = "Aiming";
    [HideInInspector] public Transform ForwardAxis;

    [Header("Attack")]
    public GameObject Bullet;
    public Transform HeadGun;


    #region Monobehaviour
    private void Start()
    {
        ForwardAxis = Camera.main.transform;
        UserInputController.Instance.OnAimingJoystick += SetAimingState;
        UserInputController.Instance.OnCancelAiming += CancelAiming;
    }
    private void OnDestroy()
    {
        UserInputController.Instance.OnAimingJoystick -= SetAimingState;
        UserInputController.Instance.OnCancelAiming -= CancelAiming;
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
            newbu.GetComponent<TrailRenderer>().enabled = true;
            newbu.GetComponent<Bullet>().Speed = 150 * transform.forward;
            DOVirtual.DelayedCall(1f, () =>
            {
                newbu.GetComponent<Bullet>().Speed = Vector3.zero;
                newbu.GetComponent<TrailRenderer>().enabled = false;
                newbu.transform.position = transform.position;
                DOVirtual.DelayedCall(0.2f, () => LeanPool.Despawn(newbu));


            });
        }
    }
    private void CancelAiming()
    {
        IsAiming = false;
        SetAimingState(IsAiming);
    }
    #endregion
}
