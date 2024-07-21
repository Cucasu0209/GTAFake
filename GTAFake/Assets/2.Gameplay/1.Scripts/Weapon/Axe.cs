using DG.Tweening;
using Lean.Pool;
using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class Axe : BaseWeapon
{
    public GameObject HitEffect;
    //  public LayerMask EnemyMask;
    //  public GameObject SlashFx;

    public override void Attack(Transform character)
    {
        base.Attack(character);
        foreach (var enemy in PlayerTakeDmgSystem.Instance.GetEnemyInCircleArea(character.transform.position, 4))
        {
            enemy.TakeDmg(Data.BaseDmg);
        }
    }
}
