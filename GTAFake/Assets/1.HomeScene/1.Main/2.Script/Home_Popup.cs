using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class Home_Popup: CustomAPI
{
    [SerializeField] protected PopupType PopupType;
    [SerializeField] private GameObject GreyBG;
    [SerializeField] private Transform SelfPopup;
    [SerializeField] private Button CloseButton;
    protected virtual void Start()
    {
        HomeSceneScript.Instance.OnOpenPopupType += OnOpenPopupType;
        CloseButton.onClick.AddListener(OnClosePopup);
    }
    protected virtual void OnDestroy()
    {
        HomeSceneScript.Instance.OnOpenPopupType -= OnOpenPopupType;
    }

    private void OnOpenPopupType(PopupType popupType)
    {
        if (PopupType == popupType)
        {
            GreyBG.SetActive(true);
            SelfPopup.gameObject.SetActive(true);
            SelfPopup.localScale = Vector3.zero;
            SelfPopup.DOScale(1, 0.25f);
            OpenPopup();
        }
        else
        {
            OnClosePopup();
        }
    }

    protected virtual void OpenPopup()
    {

    }

    protected virtual void OnClosePopup()
    {
        GreyBG.SetActive(false);
        SelfPopup.gameObject.SetActive(false);
    }
}
