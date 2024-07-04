
using System;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class DailyRewardItemUI : MonoBehaviour
{
    public int TodayIndex;
    public enum State
    {
        PassBy, Today, Future
    }
    public GameObject InactiveBackground;
    public GameObject ActiveBackground;

    public Image Tick;
    public Button SelfButton;

    public Action<int> OnAttendance;
    public Image BigGrow;
    private void Start()
    {
        SelfButton.onClick.AddListener(OnClick);
    }
    public void ActiveButton(bool isActive)
    {
        if (isActive)
        {
            InactiveBackground.SetActive(false);
            ActiveBackground.SetActive(true);
            BigGrow.transform.DOKill();
            BigGrow.transform.rotation = Quaternion.Euler(0, 0, 0);
            BigGrow.transform.DORotate(Vector3.forward * 90, 6).SetEase(Ease.Linear).SetLoops(-1, LoopType.Incremental);
            BigGrow.transform.DOScale(1.3f, 1).SetEase(Ease.Linear).SetLoops(-1, LoopType.Yoyo);
        }
        else
        {
            ActiveBackground.SetActive(false);
            InactiveBackground.SetActive(true);
        }
    }

    public void OnClick()
    {
        OnAttendance?.Invoke(TodayIndex);
    }
    public void Setup(State state)
    {
        switch (state)
        {
            case State.PassBy:
                Tick.gameObject.SetActive(true);
                BigGrow.gameObject.SetActive(false);
                ActiveButton(false);
                break;
            case State.Today:
                Tick.gameObject.SetActive(false);
                BigGrow.gameObject.SetActive(true);
                ActiveButton(true);
                break;
            case State.Future:
                Tick.gameObject.SetActive(false);
                BigGrow.gameObject.SetActive(false);
                ActiveButton(false);
                break;

        }
    }
}
