using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class HeartBar : MonoBehaviour
{
    [SerializeField] private Slider Heartbar;
    [SerializeField] private Image HeartbarFill;
    [SerializeField] private Color Color100;
    [SerializeField] private Color Color50;
    [SerializeField] private Color Color25;
    public void UpdatePercentage(float percentage)
    {
        Heartbar.SetValueWithoutNotify(percentage);
        HeartbarFill.DOColor(GetColor(percentage), 0.1f);
    }
    private Color GetColor(float percentage)
    {
        if (percentage < 0.25f) return Color25;
        if (percentage < 0.5f) return Color50;
        return Color100;
    }
    private void LookAtCamera()
    {
        transform.LookAt(Camera.main.transform);
    }
    private void Update()
    {
        LookAtCamera();
    }
}
