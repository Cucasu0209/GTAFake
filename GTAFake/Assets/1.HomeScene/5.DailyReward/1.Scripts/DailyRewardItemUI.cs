
using System;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;
using Sirenix.OdinInspector;

public class DailyRewardItemUI : SerializedMonoBehaviour
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

    [HideInInlineEditors] public Action<int> OnAttendance;
    public Image BigGrow;

    [SerializeField] public Dictionary<DailyRewardItem, Sprite> RewardIcons;
    public Image Icon;
    public TextMeshProUGUI Count;
    public TextMeshProUGUI Day;

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
    public void Setup(int day, DailyRewardData data)
    {
        TodayIndex = day - 1;
        Day.SetText("Day " + day);
        Count.SetText("x" + data.Count.ToString());
        Icon.sprite = RewardIcons[data.Type];
        Icon.SetNativeSize();
    }
}
[Serializable]
public class DailyRewardData
{
    public DailyRewardItem Type;
    public int Count;

}
public enum DailyRewardItem
{
    Gold,
    Coal,
    Gunpower,
    Steel,
    Vitamin,
    Uranium,
    Chest
}