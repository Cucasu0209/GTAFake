using UnityEngine;
using UnityEngine.UI;
public class CameraChooseEnemy : MonoBehaviour
{
    Vector3 PosInCamera;
    public Image centra;
    public RectTransform viewport;
    Vector2 LastPos = new Vector3(0, 111);
    float lastScale = 1;

    private void Update()
    {
        EnemyController[] enemies = FindObjectsByType<EnemyController>(FindObjectsSortMode.None);
        float maxdistance = 0.25f;
        EnemyController chossenenemy = null;

        foreach (EnemyController enemy in enemies)
        {
            if (enemy.GetHealth() > 0)
            {
                PosInCamera = Camera.main.WorldToViewportPoint(enemy.transform.position + Vector3.up);
                if (Mathf.Abs(PosInCamera.x - 0.5f) < 0.5f && Mathf.Abs(PosInCamera.y - 0.5f) < 0.5f && PosInCamera.z > 0)
                {
                    if (Mathf.Abs(PosInCamera.x - 0.5f) < maxdistance)
                    {
                        chossenenemy = enemy;
                        maxdistance = Mathf.Abs(PosInCamera.x - 0.5f);
                    }
                }
            }
        }

        if (chossenenemy != null)
        {
            PosInCamera = Camera.main.WorldToViewportPoint(chossenenemy.transform.position + Vector3.up);
            Debug.Log((chossenenemy.transform.position + Vector3.up - Camera.main.transform.position).magnitude);
            lastScale = Mathf.Lerp(lastScale, (Mathf.Clamp((chossenenemy.transform.position + Vector3.up - Camera.main.transform.position).magnitude, 9, 40) - 40) / (9 - 40) * 1f + 0.4f, 10 * Time.deltaTime);
            LastPos = Vector2.Lerp(LastPos, new Vector2((PosInCamera.x - 0.5f) * viewport.sizeDelta.x, (PosInCamera.y - 0.5f) * viewport.sizeDelta.y), 50 * Time.deltaTime);
            centra.color = Color.red;
            UserInputController.Instance.OnChangeTargetAim?.Invoke(chossenenemy.transform.position);
        }
        else
        {
            LastPos = Vector2.Lerp(LastPos, new Vector3(0, 111), 20 * Time.deltaTime);
            centra.color = Color.white;
            lastScale = Mathf.Lerp(lastScale, 0.6f, 10 * Time.deltaTime);
            UserInputController.Instance.OnChangeTargetAim?.Invoke(Camera.main.transform.position + 100 * Camera.main.transform.forward);
        }
        centra.rectTransform.anchoredPosition = LastPos;
        centra.transform.localScale = Vector3.one * lastScale;
    }
}
