using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System;
using DG.Tweening;
using UnityEngine.UI;
public class ProgressCircle : MonoBehaviour
{
    [SerializeField] private FixedJoystick joystick;
    private float InteractThreadhold = 0;
    private bool IsShowed = false;
    private bool IsDragging = false;
    [SerializeField] private List<Transform> BG;
    [SerializeField] private List<Transform> Items;
    [Header("Display")]
    [SerializeField] private Image CentraItem;
    [SerializeField] private List<Sprite> Icons;

    private float TimeToShow = 0.3f;

    private int CurrentIndex = -1;
    private int CacheIndex = 0;
    private Vector2 CurrentVector;
    private float CurrentAngle = 0;
    private void Start()
    {
        joystick.OnStartDrag += OnStartDrag;
        joystick.OnEndDrag += OnEndDrag;
        UserInputController.Instance.OnChooseWeaponIndex += UpdateIcon;
        for (int i = 0; i < BG.Count; i++)
        {
            BG[i].localScale = Vector3.zero;
        }
        for (int i = 0; i < Items.Count; i++)
        {
            Items[i].localScale = Vector3.zero;
        }
    }
    private void OnDestroy()
    {
        joystick.OnStartDrag -= OnStartDrag;
        joystick.OnEndDrag -= OnEndDrag;
        UserInputController.Instance.OnChooseWeaponIndex -= UpdateIcon;
    }


    private void Update()
    {
        if (IsDragging)
        {
            InteractThreadhold += Time.deltaTime;
            if (InteractThreadhold > TimeToShow || new Vector2(joystick.Horizontal, joystick.Vertical).magnitude > 0.8f)
            {
                if (IsShowed == false) ShowIcon();
                if (IsShowed)
                {
                    CurrentVector = new Vector2(joystick.Horizontal, joystick.Vertical);
                    if (CurrentVector.magnitude > 0.5f)
                    {
                        CurrentAngle = ((-Vector2.SignedAngle(CurrentVector, Vector2.right)) % 360 + 360) % 360;
                        if (CurrentIndex != ((((int)(CurrentAngle - 180 / Items.Count)) % 360 + 360) % 360) / (360 / Items.Count))
                        {
                            CurrentIndex = ((((int)(CurrentAngle - 180 / Items.Count)) % 360 + 360) % 360) / (360 / Items.Count);
                            for (int i = 0; i < Items.Count; i++)
                            {
                                Items[i].DOKill();
                                Items[i].DOScale(i == CurrentIndex ? 1.1f : 0.9f, 0.1f).SetEase(Ease.Linear);
                            }
                        }
                    }
                    else
                    {
                        if (CurrentIndex != -1)
                        {
                            CurrentIndex = -1;
                            for (int i = 0; i < Items.Count; i++)
                            {
                                Items[i].DOKill();
                                Items[i].DOScale(1, 0.1f).SetEase(Ease.Linear);
                            }
                        }
                    }

                }
            }
        }

    }
    private void ShowIcon()
    {
        IsShowed = true;
        for (int i = 0; i < BG.Count; i++)
        {
            BG[i].DOKill();
            BG[i].DOScale(1, 0.1f).SetEase(Ease.Linear);
        }

        for (int i = 0; i < Items.Count; i++)
        {
            Items[i].DOKill();
            Items[i].DOScale(1, 0.2f).SetDelay(0.05f + (i % 2) * 0.05f).SetEase(Ease.Linear);
        }
        CurrentIndex = -1;

    }
    private void HideIcons()
    {
        if (IsShowed)
        {
            for (int i = 0; i < BG.Count; i++)
            {
                BG[i].DOKill();
                BG[i].DOScale(0, 0.2f).SetEase(Ease.Linear).SetDelay(0.15f);
            }

            for (int i = 0; i < Items.Count; i++)
            {
                Items[i].DOKill();
                Items[i].DOScale(0, 0.2f).SetDelay((i % 2) * 0.05f).SetEase(Ease.Linear);
            }

            DOVirtual.DelayedCall(0.2f, () => { IsShowed = false; });
        }
    }
    private void OnStartDrag()
    {
        InteractThreadhold = 0;
        if (new Vector2(joystick.Horizontal, joystick.Vertical).magnitude < 0.5f) IsDragging = true;
    }
    private void OnEndDrag()
    {
        if (CurrentIndex == -1)
        {
            CurrentIndex = CacheIndex;
        }
        CacheIndex = CurrentIndex;
        UserInputController.Instance.OnChooseWeaponIndex?.Invoke(CurrentIndex);
        IsDragging = false;
        InteractThreadhold = 0;
        HideIcons();
    }
    private void UpdateIcon(int id)
    {
        CentraItem.sprite = Icons[id];
    }
}
