using DG.Tweening;
using Lean.Pool;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Bomb_GangsterSkill : MonoBehaviour
{
    public Vector2 StartVelocity = new Vector2(3, 5);
    public float gravityScale = 6;
    public MeshRenderer Renderer;
    public GameObject ParticleSystem;
    public float radious = 10;
    bool canTrigger = false;
    private Vector3 currentVelocity = Vector3.zero;
    private bool flying = false;
    private float gravity = -9.8f;
    private void OnEnable()
    {
        transform.rotation = Quaternion.Euler(0, 0, 0);
        ParticleSystem.active = false;

        Renderer.enabled = true;
        canTrigger = false;
        flying = false;
        currentVelocity = Vector3.zero;
    }
    public void Fly(Transform forward)
    {
        currentVelocity = forward.forward.normalized * StartVelocity.y + Vector3.up * StartVelocity.x;
        DOVirtual.DelayedCall(0.4f, () => canTrigger = true);
        flying = true;
    }
    private void Update()
    {
        if (flying)
        {
            transform.position += currentVelocity * Time.deltaTime;
            currentVelocity += Vector3.up * gravity * gravityScale * Time.deltaTime;

        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (canTrigger)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);

            ParticleSystem.active = true;
            Renderer.enabled = false;

            LeanPool.Despawn(gameObject, 3);
            PlayerTakeDmgSystem.Instance.TakeDmgInCircleArea(transform.position, radious, 999);

            Debug.Log(transform.rotation.eulerAngles);
            flying = false;
        }
    }
}
