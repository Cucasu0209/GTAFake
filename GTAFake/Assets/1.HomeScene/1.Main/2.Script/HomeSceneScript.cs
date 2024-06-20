using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomeSceneScript: MonoBehaviour
{
    public static HomeSceneScript Instance;
    public Action<PopupType> OnOpenPopupType;
    public Action OnReloadMail;
    public Action<PopupType, bool> OnShowNotification;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }
}
