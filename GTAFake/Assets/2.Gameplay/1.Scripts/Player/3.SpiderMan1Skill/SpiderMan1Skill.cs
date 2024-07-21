using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SpiderMan1Skill : PlayerSkill
{
    [SerializeField] private float FlyingHeight;
    public bool IsCliningAhead;

    public override void Start()
    {
        base.Start();
        UserInputController.Instance.OnMovementJoystick += OnMove;

    }
    public override void OnDestroy()
    {
        base.OnDestroy();
        UserInputController.Instance.OnMovementJoystick -= OnMove;

    }
    private void OnMove(float hz, float ver)
    {
        if (IsPlayingSkill)
        {
            Vector2 MoveInput = new Vector2(hz, ver);

            if (MoveInput.magnitude > 0.1f)
            {
                if (IsCliningAhead == false)
                {
                    //move
                    transform.DOKill();
                    transform.DOLocalRotate(Vector3.right * 30, 0.2f).SetEase(Ease.Linear);
                    transform.DOLocalMoveZ(-0.8f, 0.2f).SetEase(Ease.Linear);
                }
                IsCliningAhead = true;

            }
            else
            {
                if (IsCliningAhead)
                {
                    transform.DOKill();
                    transform.DOLocalRotate(Vector3.zero, 0.2f).SetEase(Ease.Linear);
                    transform.DOLocalMoveZ(0, 0.2f).SetEase(Ease.Linear);

                }
                IsCliningAhead = false;
            }
        }

    }
    public override void PlaySkill()
    {
        if (IsPlayingSkill == false)
        {
            Controller.IsAntigravity = true;
            base.PlaySkill();
            Jumping.SetCanJump(false);
            WeaponManager.SetCanSwitchWeapon(false);
            WeaponManager.SetCanAttack(false);
        }
        else
        {
            Controller.IsAntigravity = false;
            transform.DOKill();
            transform.DOLocalRotate(Vector3.zero, 0.2f).SetEase(Ease.Linear);
            transform.DOLocalMoveZ(0, 0.2f).SetEase(Ease.Linear);

            Controller.EndSkill();
            Movement.SetScaleSpeed(1);
            Movement.SetCanRun(false);
            IsPlayingSkill = false;
        }
    }
    public override void TakeDmg()
    {
        Controller.transform.DOMove(Controller.transform.position + FlyingHeight * Vector3.up, 0.5f).SetEase(Ease.Linear);
        base.TakeDmg();
        Movement.SetScaleSpeed(2);
        Movement.SetCanRun(true);
    }
    public override void OnEndSkill()
    {
        base.OnEndSkill();
        Jumping.SetCanJump(true);
        WeaponManager.SetCanSwitchWeapon(true);
        WeaponManager.SetCanAttack(true);
    }
}
