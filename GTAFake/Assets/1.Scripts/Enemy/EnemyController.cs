using DG.Tweening;
using Lean.Pool;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour, IActor
{
    public NavMeshAgent Agent;
    public Animator Anim;
    public Rigidbody Body;

    float LastTimeCheckFollow = 0;
    private float health = 10;

    public void Respawn()
    {
        foreach (Rigidbody r in GetComponentsInChildren<Rigidbody>())
        {
            r.useGravity = false;
        }
        Anim.enabled = true;
        Agent.enabled = true;
        health = 10;
    }
    public float GetHealth()
    {

        return health;
    }

    public void TakeDmg(float dmg)
    {
        health -= dmg;
        if (health <= 0)
        {
            OnDeath();
        }
    }
    public void OnDeath()
    {
        Anim.enabled = false;
        foreach (Rigidbody r in GetComponentsInChildren<Rigidbody>())
        {
            r.useGravity = true;
        }

        Agent.enabled = false;
        GameManager.Instance.RemoveEnemy(this);
        DOVirtual.DelayedCall(3, () =>
        {
            LeanPool.Despawn(gameObject);
        });
    }

    // Update is called once per frame
    void Update()
    {
        Anim.SetFloat("speed", Agent.velocity.magnitude);
        if (Time.time - LastTimeCheckFollow > Random.Range(2, 4f))
        {
            LastTimeCheckFollow = Time.time;
            PlayerController player = FindAnyObjectByType<PlayerController>();
            if (player != null && Agent.enabled)
            {
                Agent.SetDestination(player.transform.position);



            }
            if (Body.velocity.y < -3) Body.velocity = Vector3.zero;
        }
    }


}
