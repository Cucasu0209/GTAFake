using DG.Tweening;
using Lean.Pool;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spider2Web : MonoBehaviour
{
    public Vector2 StartVelocity = new Vector2(3, 5);
    public Rigidbody Body;
    public MeshRenderer Renderer;
    public GameObject ParticleSystem;
    bool canTrigger = false;
    private void OnEnable()
    {
        ParticleSystem.active = false;
        Body.useGravity = true;
        Body.velocity = Vector3.zero;
        Body.isKinematic = false;
        Renderer.enabled = true;
        canTrigger = false;
    }
    public void Fly(Transform forward)
    {
        Vector3 newfir = forward.forward.normalized * StartVelocity.y + Vector3.up * StartVelocity.x;
        Body.velocity = newfir;
        DOVirtual.DelayedCall(0.3f, () => canTrigger = true);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (canTrigger)
        {
            ParticleSystem.active = true;
            Body.useGravity = false;
            Body.velocity = Vector3.zero;
            Body.isKinematic = true;
            Renderer.enabled = false;

            LeanPool.Despawn(gameObject, 3);


        }
    }
}
