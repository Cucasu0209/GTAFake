using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Axe : BaseWeapon
{
    public GameObject Fx;
    public LayerMask Mask;

    public override void StartAttack(Transform effectParent, Vector3 forward)
    {

    }
    public override void Damage(Transform parent, Vector3 forward)
    {
        GameObject fx = Instantiate(Fx, parent);
        fx.transform.localPosition = Vector3.zero;
        DOVirtual.DelayedCall(1, () => Destroy(fx.gameObject));

        Collider[] hitEnemies = Physics.OverlapBox(parent.transform.position + forward + Vector3.up * 0.5f, new Vector3(2, 0.5f, 2), Quaternion.identity, Mask);

        foreach (Collider enemy in hitEnemies)
        {
            IActor actor = enemy.GetComponent<IActor>();
            if (actor != null)
            {
                actor.TakeDmg(Data.BaseDmg);
            }
        }
    }
}
