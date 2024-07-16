using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkill : MonoBehaviour
{
    [HideInInspector] public PlayerController Controller;
    [HideInInspector] public PlayerMovement Movement;
    [HideInInspector] public PlayerWeaponManager WeaponManager;
    protected bool IsPlayingSkill = false;
    public virtual void Start()
    {
        Controller = GetComponent<PlayerController>();
        Movement = GetComponent<PlayerMovement>();
        WeaponManager = GetComponent<PlayerWeaponManager>();
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
        Controller.ActiveLayerSkill(true);
        Controller.PlaySkill();
        Movement.Stop = true;
        WeaponManager.HideWeapon();
        IsPlayingSkill = true;
    }
    public virtual void OnEndSkill()
    {
        Controller.ActiveLayerSkill(false);
        Movement.Stop = false;
        WeaponManager.ShowWeapon();
        IsPlayingSkill = false;

    }
    public virtual void TakeDmg()
    {

    }
}
