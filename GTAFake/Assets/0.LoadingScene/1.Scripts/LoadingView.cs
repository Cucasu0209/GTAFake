using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingView : MonoBehaviour
{
    [SerializeField] private Slider LoadingLoadingProgress;
    private void Start()
    {
        LoadingManager.Instance.OnStartLoading += UpdateCurrentProgress;
        LoadingManager.Instance.OnActionComplete += UpdateCurrentProgress;

    }
    private void OnDestroy()
    {
        LoadingManager.Instance.OnStartLoading -= UpdateCurrentProgress;
        LoadingManager.Instance.OnActionComplete -= UpdateCurrentProgress;
    }

    private void SetSliderValue(float progress)
    {
        LoadingLoadingProgress.DOKill();
        LoadingLoadingProgress.DOValue(progress, 0.1f).SetEase(Ease.Linear);
    }
    private void UpdateCurrentProgress()
    {
        SetSliderValue(LoadingManager.Instance.GetCurrentProgress());
    }
}
