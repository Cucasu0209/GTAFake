using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AutoCanvas: MonoBehaviour
{
    [SerializeField] private CanvasScaler SelfCanvas;
    void Awake()
    {
        AutoCanvasHorizontalScreen();
    }
    /// <summary>
    /// Auto canvas của màn ngang
    /// CANVAS SIZE LUÔN LÀ 1920 X 1080
    /// </summary>
    private void AutoCanvasHorizontalScreen()
    {
        if ((float)Screen.height / Screen.width - 0.63f > 0)
        {
            // => tablet hoặc màn 2:3 => fit width
            SelfCanvas.matchWidthOrHeight = 0;
        }
        else
        {
            // => phone => fit height
            SelfCanvas.matchWidthOrHeight = 1f;
        }
    }
    /// <summary>
    /// Auto canvas của màn dọc
    /// CANVAS SIZE LUÔN LÀ 1080 X 1920
    /// </summary>
    private void AutoCanvasVerticalScreen()
    {
        if ((float)Screen.width / Screen.height - 0.63f > 0)
        {
            // => tablet hoặc màn 2:3 => fit height
            SelfCanvas.matchWidthOrHeight = 1;
        }
        else
        {
            // => phone => fit width
            SelfCanvas.matchWidthOrHeight = 0f;
        }
    }
}
