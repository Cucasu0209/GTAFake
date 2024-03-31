using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSMeasurement : MonoBehaviour
{
    float LastTimeCheck = 0;
    int i = 0;
    void Update()
    {
        LastTimeCheck += Time.deltaTime;
        i++;
        if (LastTimeCheck >= 1)
        {
            Debug.Log("FPS:" + i);
            i = 0;
            LastTimeCheck = 0;
        }
    }
}
