using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineSpriteRenderer : MonoBehaviour
{
    // ĐÂY LÀ CLASS THAY CHO LINERENDERER CHỈ CÓ 2 POS COUNT
    [SerializeField] private SpriteRenderer SelfSpriteRenderer;
    [SerializeField] private float Height;

    private Vector2 EndLineOffset;
    public void ShowLine(Vector2 targetPos, float width)
    {
        gameObject.SetActive(true);
        EndLineOffset = targetPos;
        SelfSpriteRenderer.size = new Vector2(width, Height);
        transform.eulerAngles = new Vector3(0, 0, Vector2.SignedAngle(Vector2.right, targetPos));
    }

    public float GetSqrDistanceFromTargetToLine(Vector2 targetPos)
    {
        Vector2 startLinePoint = transform.position;
        Vector2 endLinePoint = (Vector2)transform.position + EndLineOffset; 
        return Mathf.Pow((endLinePoint.x - startLinePoint.x) * (startLinePoint.y - targetPos.y) - (endLinePoint.y - startLinePoint.y) * (startLinePoint.x - targetPos.x), 2)
            / (Mathf.Pow(endLinePoint.x - startLinePoint.x, 2) + Mathf.Pow(endLinePoint.y - startLinePoint.y, 2));
    }
}
