using UnityEngine;

public class FollowPlayerCamera : MonoBehaviour
{
    [Header("Camera Follow Player")]
    [SerializeField] private Vector2 MinMaxClampY;
    private Vector2 Angle = new Vector2(90 * Mathf.Deg2Rad, 0);
    private Vector2 NearPlaneSize;

    [SerializeField] private Transform TransformFollow;

    [SerializeField] private float MaxDistance;

    private Vector2 Sensitivity;//độ nhạy
    [SerializeField] private Vector2 NormalSensitivity;
    [SerializeField] private Vector2 AimingSensitivity;
    [SerializeField] private Camera Camera;

    private void Start()
    {
        Sensitivity = NormalSensitivity;
        CalculateNearPlaneSize();
        UserInputController.Instance.OnCameraAxisChange += FollowPlayer;
        UserInputController.Instance.OnStartAiming += StartAiming;
        UserInputController.Instance.OnCancelAiming += CancelAiming;
    }
    private void OnDestroy()
    {
        UserInputController.Instance.OnCameraAxisChange -= FollowPlayer;
        UserInputController.Instance.OnStartAiming -= StartAiming;
        UserInputController.Instance.OnCancelAiming -= CancelAiming;
    }
    private void CalculateNearPlaneSize()
    {
        float height = Mathf.Tan(Camera.fieldOfView * Mathf.Deg2Rad / 2) * Camera.nearClipPlane;
        float width = height * Camera.aspect;

        NearPlaneSize = new Vector2(width, height);
    }

    private void FollowPlayer(float hor, float ver)
    {
        //transform.position = TransformFollow.position - Vector3.forward * MaxDistance;


        Angle.x += hor * Mathf.Deg2Rad * Sensitivity.x;
        Angle.y += ver * Mathf.Deg2Rad * Sensitivity.y;
        Angle.y = Mathf.Clamp(Angle.y, MinMaxClampY.x * Mathf.Deg2Rad, MinMaxClampY.y * Mathf.Deg2Rad);
        Debug.Log(Angle.x / Mathf.Deg2Rad + " a " + Angle.y / Mathf.Deg2Rad);
        Vector3 direction = new Vector3(
            Mathf.Cos(Angle.x) * Mathf.Cos(Angle.y),
            -Mathf.Sin(Angle.y),
            -Mathf.Sin(Angle.x) * Mathf.Cos(Angle.y));

        float distance = MaxDistance;
        //transform.position = Vector3.Lerp(transform.position, TransformFollow.position + direction * distance, 30 * Time.deltaTime);
        // transform.rotation = Quaternion.LookRotation(TransformFollow.position - transform.position);
        transform.position = TransformFollow.position + direction * distance;
        transform.rotation = Quaternion.Euler(-Angle.y / Mathf.Deg2Rad, Angle.x / Mathf.Deg2Rad - 90, 0);
        Debug.Log(transform.rotation.eulerAngles);

    }

    public void StartAiming()
    {
        Sensitivity = AimingSensitivity;
    }
    public void CancelAiming()
    {
        Sensitivity = NormalSensitivity;

    }
}
