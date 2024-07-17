using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkill : MonoBehaviour
{
    [HideInInspector] public PlayerController Controller;
    [HideInInspector] public PlayerMovement Movement;
    [HideInInspector] public PlayerJumping Jumping;
    [HideInInspector] public PlayerWeaponManager WeaponManager;
    protected bool IsPlayingSkill = false;
    public virtual void Start()
    {
        Controller = transform.parent.GetComponent<PlayerController>();
        Movement = transform.parent.GetComponent<PlayerMovement>();
        WeaponManager = transform.parent.GetComponent<PlayerWeaponManager>();
        Jumping = transform.parent.GetComponent<PlayerJumping>();
        UserInputController.Instance.OnPlayerPlaySkill += PlaySkill;
        Controller.OnEndSkill = OnEndSkill;
        Controller.OnTakeDmgSkill = TakeDmg;
    }
    public virtual void OnDestroy()
    {
        UserInputController.Instance.OnPlayerPlaySkill -= PlaySkill;
    }
    public virtual void PlaySkill()
    {
        if (IsPlayingSkill == false)
        {
            Controller.ActiveLayerSkill(true);
            Controller.PlaySkill();
            Movement.SetCanRun(false);
            WeaponManager.HideWeapon();
            IsPlayingSkill = true;
        }

    }
    public virtual void OnEndSkill()
    {
        Controller.ActiveLayerSkill(false);
        Movement.SetCanRun(true);
        WeaponManager.ShowWeapon();
        IsPlayingSkill = false;

    }
    public virtual void TakeDmg()
    {

    }
}
