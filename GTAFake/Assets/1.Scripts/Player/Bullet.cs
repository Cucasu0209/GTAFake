using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public Vector3 Speed = Vector3.zero;

    float timeExist = 0;
    // Update is called once per frame
    void Update()
    {
        timeExist += Time.deltaTime;
        if(timeExist > 2 ) Destroy(gameObject);
        transform.position += Speed * Time.deltaTime;
    }
}
