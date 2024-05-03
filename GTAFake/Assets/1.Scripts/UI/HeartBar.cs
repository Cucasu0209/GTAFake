using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartBar : MonoBehaviour
{
    private void LookAtCamera()
    {
        transform.LookAt(Camera.main.transform);
    }
    private void Update()
    {
        LookAtCamera();
    }
}
