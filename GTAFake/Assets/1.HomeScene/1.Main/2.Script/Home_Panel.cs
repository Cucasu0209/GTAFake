using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Home_Panel: SerializedMonoBehaviour
{
    [SerializeField] protected PopupType PopupType;
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
            SelfPopup.gameObject.SetActive(true);
            OpenPanel();
        }
        else
        {
            OnClosePopup();
        }
    }
    protected virtual void OpenPanel()
    {

    }

    private void OnClosePopup()
    {
        SelfPopup.gameObject.SetActive(false);
    }
}
