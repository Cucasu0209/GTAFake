using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class PlayerAnimationCallbacks : MonoBehaviour
{
    public PlayerController Controller;
    public MultiAimConstraint RhandConstraint;

    public void Start()
    {
        if (Controller == null) Controller = GetComponentInParent<PlayerController>();
        UserInputController.Instance.OnStartAiming += OnStartAiming;
        UserInputController.Instance.OnCancelAiming += OnCancelAiming;
    }
    public void OnDestroy()
    {
        UserInputController.Instance.OnStartAiming -= OnStartAiming;
        UserInputController.Instance.OnCancelAiming -= OnCancelAiming;
    }

    private void OnStartAiming()
    {
        RhandConstraint.weight = 1.0f;
    }
    private void OnCancelAiming()
    {
        RhandConstraint.weight = 0f;
    }
}
