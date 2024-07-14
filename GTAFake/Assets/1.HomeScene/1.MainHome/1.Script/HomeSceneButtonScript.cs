using UnityEngine;
using UnityEngine.UI;

public class HomeSceneButtonScript : MonoBehaviour
{
    [SerializeField] protected PopupType PopupType;
    [SerializeField] protected Button SelfButton;
    [SerializeField] protected GameObject Notification;

    protected virtual void Start()
    {
        SelfButton.onClick.AddListener(OnClickButton);
        HomeSceneScript.Instance.OnShowNotification += SetNotification;
    }

    protected virtual void OnDestroy()
    {
        HomeSceneScript.Instance.OnShowNotification -= SetNotification;
    }

    private void SetNotification(PopupType popupType, bool hasNoti)
    {
        if (popupType == PopupType)
        {
            Notification.SetActive(hasNoti);
        }
    }
    protected void OnClickButton()
    {
        HomeSceneScript.Instance.OnOpenPopupType(PopupType);
    }
}
public enum PopupType
{
    Setting = 0,
    MissionInfo = 1,
    Shop = 2,
    DailyReward = 3,
    DailyMission = 4,
    Chest = 5,

}