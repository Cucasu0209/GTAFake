using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;  
public class BaseBullet : MonoBehaviour
{
    public LayerMask Mask;
    private float Dmg = 0;
    private bool IsFlying = false;
    private Vector3 Dir= Vector3.forward;
    private float Speed = 100;
    public void SetDmg(float dmg)
    {
        Dmg = dmg;
    }
    public void Fly(Vector3 dir)
    {
        Dir = dir;
        IsFlying= true;
        DOVirtual.DelayedCall(4, () => Destroy(gameObject));
    }
    private void Update()
    {
        if (IsFlying)
        {
            transform.Translate(Dir * Speed * Time.deltaTime);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (Physics.CheckSphere(transform.position, 0.1f, Mask))
        {
            IActor actor = other. GetComponent<IActor>();
            if(actor != null)
            {
                actor.TakeDmg(Dmg);
                Destroy(gameObject);
            }
        }
    }
}
