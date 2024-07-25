using Cinemachine.Utility;
using DG.Tweening;
using Lean.Pool;
using Sirenix.OdinInspector.Editor;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour, IActor
{
    public NavMeshAgent Agent;
    public Animator Anim;
    public PlayerController player;

    public float RangeAttack;

    float LastTimeCheckFollow = 0;
    private float Health = 10;
    [SerializeField] private float MaxHealth = 10;
    private Vector3 LastPlayerPos = Vector3.up * 999;
    public bool Run = true;

    public GameObject FxStun;
    private bool isStunning = false;
    public void Respawn()
    {
        player = FindAnyObjectByType<PlayerController>();
        Anim.SetBool("death", false);
        CancelStun();
        Agent.enabled = true;
        Health = MaxHealth;
    }
    public float GetHealth()
    {

        return Health;
    }

    public void TakeDmg(float dmg)
    {
        Health -= dmg;
        FloatingBloodSystem.Instance.ShowFloatingBlood(transform.position + Vector3.up * 3.5f, (-dmg).ToString());
        if (Health <= 0)
        {
            OnDeath();
        }
    }
    public void OnDeath()
    {
        Anim.SetBool("death", true);
        Agent.enabled = false;
        CancelStun();
        GameManager.Instance.RemoveEnemy(this);
        DOVirtual.DelayedCall(3, () =>
        {
            LeanPool.Despawn(gameObject);
        });
    }
    public void Attack()
    {
        Anim.SetBool("attacking", true);

    }
    public void CancelAttack()
    {
        Anim.SetBool("attacking", false);

    }

    // Update is called once per frame
    void Update()
    {
        if (player != null && Agent.enabled && isStunning == false)
        {
            Anim.SetFloat("speed", Agent.velocity.magnitude);

            if (Time.time - LastTimeCheckFollow > Random.Range(1f, 1.5f) && Run
             && Vector3.Distance(LastPlayerPos, player.transform.position) > 2)
            {

                LastTimeCheckFollow = Time.time;
                Agent.SetDestination(player.transform.position);
                LastPlayerPos = player.transform.position;
            }

            if (Vector3.Distance(transform.position, player.transform.position) < RangeAttack)
            {
                Agent.isStopped = true;
                Attack();
            }
            else
            {
                Agent.isStopped = false;

                CancelAttack();
            }
        }
        else if (Agent.enabled && isStunning)
        {

            Agent.velocity = Vector3.zero;
            Agent.isStopped = true;
            Anim.SetFloat("speed", 0);
            Anim.SetBool("attacking", false);

        }
    }
    #region Stun
    private void Stun()
    {
        FxStun.SetActive(true);
        isStunning = true;
    }
    private void CancelStun()
    {
        FxStun.SetActive(false);

        isStunning = false;

    }
    IEnumerator IStun;
    public void StunFor(float time)
    {
        if (IStun != null) StopCoroutine(IStun);
        IStun = IStunFor(time);
        StartCoroutine(IStun);
    }
    private IEnumerator IStunFor(float time)
    {
        Stun();
        yield return new WaitForSeconds(time);
        CancelStun();
    }
    #endregion
}