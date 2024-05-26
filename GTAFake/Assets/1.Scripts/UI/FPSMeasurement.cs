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
#if and
        //QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 60;
#endif
        m_TextMeshProUGUI = GetComponent<TextMeshProUGUI>();
    }
    void Update()
    {
        LastTimeCheck += Time.deltaTime;
        i++;
        if (LastTimeCheck >= 0.5f)
        {
            if (m_TextMeshProUGUI != null)
            {
                m_TextMeshProUGUI.SetText("FPS:" + i * 2 + "\nTris:" + (UnityEditor.UnityStats.triangles/1000) + "k\n Verts:" + (UnityEditor.UnityStats.vertices/1000)+"k");
            }
            else
                Debug.Log("FPS:" + i);
            i = 0;
            LastTimeCheck = 0;
        }
    }
}
