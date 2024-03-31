using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    public NavMeshAgent Agent;
    public Animator Anim;
    public Rigidbody Body;

    float LastTimeCheckFollow = 0;
    // Update is called once per frame
    void Update()
    {
        if (Time.time - LastTimeCheckFollow > 2f)
        {
            LastTimeCheckFollow = Time.time;
            PlayerController player = FindAnyObjectByType<PlayerController>();
            if (player != null)
            {
                Agent.SetDestination(player.transform.position);
                Anim.SetFloat("speed", Agent.velocity.magnitude);
               

            }
            if (Body.velocity.y < -3) Body.velocity = Vector3.zero;
        }
    }
}
