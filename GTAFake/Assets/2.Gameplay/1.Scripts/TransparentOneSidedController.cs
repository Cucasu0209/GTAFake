using UnityEngine;

public class TransparentOneSidedController : MonoBehaviour
{
    public Camera mainCamera;
    private Renderer objectRenderer;
    private Material objectMaterial;

    void Start()
    {
        objectRenderer = GetComponent<Renderer>();
        objectMaterial = objectRenderer.material;
    }

    void Update()
    {
        // Kiểm tra nếu camera nằm trong bounding box của vật thể
        if (objectRenderer.bounds.Contains(mainCamera.transform.position))
        {
            // Camera nằm trong vật thể, alpha = 0
            SetAlpha(0f);
        }
        else
        {
            // Camera không nằm trong vật thể, alpha như cũ (ví dụ: 0.5f)
            SetAlpha(0.5f); // Bạn có thể thay đổi giá trị này theo nhu cầu của bạn
        }
    }

    void SetAlpha(float alpha)
    {
        Color color = objectMaterial.GetColor("_Color");
        color.a = alpha;
        objectMaterial.SetColor("_Color", color);
    }
}
