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
    public HeartBar HeartBar;

    float LastTimeCheckFollow = 0;
    private float Health = 10;
    [SerializeField] private float MaxHealth = 10;

    public void Respawn()
    {
        Anim.applyRootMotion = false;
        DOVirtual.DelayedCall(0.05f, () =>
        {
            Anim.transform.localPosition = Vector3.zero;
            Anim.transform.localEulerAngles = Vector3.zero;
        });

        Agent.enabled = true;
        Health = MaxHealth;
        HeartBar.UpdatePercentage(1);
        HeartBar.gameObject.SetActive(true);
    }
    public float GetHealth()
    {

        return Health;
    }

    public void TakeDmg(float dmg)
    {
        Health -= dmg;
        HeartBar.UpdatePercentage(Health / MaxHealth);
        FloatingBloodSystem.Instance.ShowFloatingBlood(HeartBar.transform.position + Vector3.up / 2, (-dmg).ToString());
        if (Health <= 0)
        {
            OnDeath();
            HeartBar.gameObject.SetActive(false);
        }
    }
    public void OnDeath()
    {
        Anim.applyRootMotion = true;
        Anim.SetTrigger("death");
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
