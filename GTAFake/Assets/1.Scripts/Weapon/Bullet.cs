using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lean.Pool;

public class Bullet : MonoBehaviour
{
    private Vector3 Velocity = Vector3.zero;
    public LayerMask EnemyMask;
    public GameObject EffectExplode;

    public void SetVelocity(Vector3 velocity)
    {
        Velocity = velocity;
        transform.LookAt(transform.position + Velocity * 10);
        LeanPool.Despawn(gameObject, 2);
    }
    RaycastHit hit;

    // Update is called once per frame
    void Update()
    {
        transform.position += 150 * Velocity.normalized * Time.deltaTime;
        if (Physics.Raycast(transform.position - Velocity.normalized * 3, Velocity.normalized, out hit, 6, EnemyMask))
        {
            if (hit.collider.gameObject.GetComponent<IActor>() != null)
            {
                hit.collider.gameObject.GetComponent<IActor>().TakeDmg(1);
            }
            GameObject newFx = LeanPool.Spawn(EffectExplode, transform.position, Quaternion.identity);
            LeanPool.Despawn(newFx, 1);
            Velocity = Vector3.zero;
            LeanPool.Despawn(gameObject);


        }
    }
}
