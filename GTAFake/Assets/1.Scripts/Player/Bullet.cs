using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lean.Pool;

public class Bullet : MonoBehaviour
{
    public Vector3 Speed = Vector3.zero;

    // Update is called once per frame
    void Update()
    {
        transform.position += Speed * Time.deltaTime;
    }
}
