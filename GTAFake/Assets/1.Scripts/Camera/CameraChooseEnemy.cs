using UnityEngine;
using UnityEngine.UI;
public class CameraChooseEnemy : MonoBehaviour
{
    Vector3 PosInCamera;
    public Image Centra;
    public RectTransform Viewport;
    Vector2 LastPos = new Vector3(0, 111);
    float LastScale = 1;
    float Maxdistance = 0.25f;
    EnemyController ChosenEnemy;

    private void Update()
    {
        UpdateTarget();
    }

    private void UpdateTarget()
    {
        ChosenEnemy = null;
        Maxdistance = 0.25f;
        foreach (EnemyController enemy in GameManager.Instance.GetEnemies())
        {

            if (enemy.GetHealth() > 0)
            {
                PosInCamera = Camera.main.WorldToViewportPoint(enemy.transform.position + Vector3.up);
                if (Mathf.Abs(PosInCamera.x - 0.5f) < 0.5f && Mathf.Abs(PosInCamera.y - 0.5f) < 0.5f && PosInCamera.z > 0)
                {
                    if (Mathf.Abs(PosInCamera.x - 0.5f) < Maxdistance)
                    {
                        ChosenEnemy = enemy;
                        Maxdistance = Mathf.Abs(PosInCamera.x - 0.5f);
                    }
                }
            }
        }

        if (ChosenEnemy != null)
        {
            PosInCamera = Camera.main.WorldToViewportPoint(ChosenEnemy.transform.position + Vector3.up);
            LastScale = Mathf.Lerp(LastScale, (Mathf.Clamp((ChosenEnemy.transform.position + Vector3.up - Camera.main.transform.position).magnitude, 9, 40) - 40) / (9 - 40) * 1f + 0.4f, 10 * Time.deltaTime);
            LastPos = Vector2.Lerp(LastPos, new Vector2((PosInCamera.x - 0.5f) * Viewport.sizeDelta.x, (PosInCamera.y - 0.5f) * Viewport.sizeDelta.y), 50 * Time.deltaTime);
            Centra.color = Color.red;
            UserInputController.Instance.OnChangeTargetAim?.Invoke(ChosenEnemy.transform.position);
        }
        else
        {
            LastPos = Vector2.Lerp(LastPos, new Vector3(0, 111), 20 * Time.deltaTime);
            Centra.color = Color.white;
            LastScale = Mathf.Lerp(LastScale, 0.6f, 10 * Time.deltaTime);
            UserInputController.Instance.OnChangeTargetAim?.Invoke(Camera.main.transform.position + 100 * Camera.main.transform.forward);
        }
        Centra.rectTransform.anchoredPosition = LastPos;
        Centra.transform.localScale = Vector3.one * LastScale;
    }
}
