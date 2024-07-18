using DG.Tweening;
using Lean.Pool;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Bomb_GangsterSkill : MonoBehaviour
{
    public Vector2 StartVelocity = new Vector2(3, 5);
    public Rigidbody Body;
    public MeshRenderer Renderer;
    public GameObject ParticleSystem;
    bool canTrigger = false;
    private void OnEnable()
    {
        transform.rotation = Quaternion.Euler(0, 0, 0);
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
        DOVirtual.DelayedCall(0.4f, () => canTrigger = true);
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
