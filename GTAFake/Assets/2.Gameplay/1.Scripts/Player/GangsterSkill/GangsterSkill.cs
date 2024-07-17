using Lean.Pool;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GangsterSkill : PlayerSkill
{

    public Bomb_GangsterSkill BombPrefab;
    public Transform RightHand;
    public float SkillDelay = 1;
    public override void Start()
    {
        base.Start();
    }
    public override void PlaySkill()
    {
        base.PlaySkill();
        Jumping.SetCanJump(false);
        WeaponManager.SetCanSwitchWeapon(false);
        WeaponManager.SetCanAttack(false);
    }
    public override void TakeDmg()
    {
        base.TakeDmg();
        Bomb_GangsterSkill NewBomb = LeanPool.Spawn(BombPrefab, RightHand);
        NewBomb.transform.localPosition = Vector3.zero;
        NewBomb.transform.parent = null;
        NewBomb.Fly(Controller.transform);

    }
    public override void OnEndSkill()
    {
        base.OnEndSkill();
        Jumping.SetCanJump(true);
        WeaponManager.SetCanSwitchWeapon(true);
        WeaponManager.SetCanAttack(true);
    }
}
