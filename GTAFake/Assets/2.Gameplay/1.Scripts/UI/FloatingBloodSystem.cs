using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using DG.Tweening;
using Lean.Pool;

public class FloatingBloodSystem : MonoBehaviour
{
    public static FloatingBloodSystem Instance;
    [SerializeField] private TextMeshProUGUI BloodPrefab;
    [SerializeField] private RectTransform Viewport;

    private void Awake()
    {
        Instance = this;
    }
    public void ShowFloatingBlood(Vector3 worldPosition, string value)
    {
        TextMeshProUGUI newbl = LeanPool.Spawn(BloodPrefab, transform);
        newbl.SetText(value);
        Vector3 PosInCamera = Camera.main.WorldToViewportPoint(worldPosition);

        newbl.rectTransform.anchoredPosition = new Vector2((PosInCamera.x - 0.5f) * Viewport.sizeDelta.x, (PosInCamera.y - 0.5f) * Viewport.sizeDelta.y);
        newbl.DOFade(0.8f, 0.01f);
        newbl.transform.localScale = Vector3.zero;
        newbl.transform.DOScale(1, 0.1f).OnComplete(() =>
        {
            newbl.rectTransform.DOAnchorPos3DY(newbl.rectTransform.anchoredPosition.y + 80, 0.7f).OnComplete(() =>
            {
                LeanPool.Despawn(newbl);
            });
            newbl.DOFade(0, 0.6f).SetEase(Ease.Linear);
            newbl.transform.DOScale(0.6F, 0.6f);
        });
    }
}
