using System.Collections;
using UnityEngine;

public class PlayerHeartUI : MonoBehaviour
{
    Vector3 PosInCamera;
    Transform player;
    public RectTransform Viewport;
    public RectTransform Slider;

    // Start is called before the first frame update
    IEnumerator Start()
    {
        yield return null;
        yield return null;
        yield return null;
        player = GameManager.Instance.playerController.transform;
    }

    // Update is called once per frame
    void Update()
    {
        if ((player != null))
        {
            PosInCamera = Camera.main.WorldToViewportPoint(player.position + Vector3.up * 2.5f);
            Slider.anchoredPosition = new Vector2((PosInCamera.x - 0.5f) * Viewport.sizeDelta.x, (PosInCamera.y - 0.5f) * Viewport.sizeDelta.y);
        }

    }
}
