using DG.Tweening;
using Lean.Pool;
using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class Axe : BaseWeapon
{
    public GameObject HitEffect;
    public LayerMask EnemyMask;

    public override void Attack(Transform character)
    {
        base.Attack(character);
        Collider[] cols = Physics.OverlapBox(character.position + Vector3.up + character.forward * 1.5f, new Vector3(1.5f, 2, 0.5f), transform.rotation, EnemyMask);
        foreach (var col in cols)
        {
            IActor enemy = col.gameObject.GetComponent<IActor>();
            if (enemy != null && enemy.GetHealth() > 0)
            {
                enemy.TakeDmg(Data.BaseDmg);
                GameObject newFx = LeanPool.Spawn(HitEffect, col.gameObject.transform.position + Vector3.up * (transform.position.y - col.gameObject.transform.position.y), Quaternion.identity);
                newFx.transform.localScale = Vector3.one * 0.7f;
                LeanPool.Despawn(newFx, 1);
            }

        }
    }
}
