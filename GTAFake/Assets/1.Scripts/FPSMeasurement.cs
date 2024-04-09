using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class FPSMeasurement : MonoBehaviour
{
    float LastTimeCheck = 0;
    int i = 0;
    private TextMeshProUGUI m_TextMeshProUGUI;
    private void Start()
    {
        //QualitySettings.vSyncCount = 0;
        //Application.targetFrameRate = 60;
        m_TextMeshProUGUI = GetComponent<TextMeshProUGUI>();
    }
    void Update()
    {
        LastTimeCheck += Time.deltaTime;
        i++;
        if (LastTimeCheck >= 1)
        {
            if (m_TextMeshProUGUI != null)
            {
                m_TextMeshProUGUI.SetText("FPS:" + i);
            }
            else
                Debug.Log("FPS:" + i);
            i = 0;
            LastTimeCheck = 0;
        }
    }
}
