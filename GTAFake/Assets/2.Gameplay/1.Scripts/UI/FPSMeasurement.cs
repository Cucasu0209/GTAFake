using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class FPSMeasurement : MonoBehaviour
{
    float LastTimeCheck = 0;
    int i = 0;
    public MatrixMap map;
    private TextMeshProUGUI m_TextMeshProUGUI;
    private void Start()
    {
        //#if and
        //QualitySettings.vSyncCount = 0;
#if UNITY_EDITOR
        Application.targetFrameRate = 60;
#else
        Application.targetFrameRate = 60;
#endif

        //#endif
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
                m_TextMeshProUGUI.SetText("FPS:" + i * 2 + "" +
                    "\n" + map.row + " " + map.column + " " + map.CellMarked.Count);
            }
            else
                Debug.Log("FPS:" + i);
            i = 0;
            LastTimeCheck = 0;
        }
    }
}
