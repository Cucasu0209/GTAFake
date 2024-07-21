
using UnityEngine;

public class PlayerShadow : MonoBehaviour
{
    [SerializeField] private Transform Shadow;
    [SerializeField] private Transform PlayerTrans;
    public LayerMask GroundMask;
    private void Start()
    {
        transform.parent = null;
    }
    public void SetShadowScale(float scale)
    {
        Shadow.transform.localScale = Vector3.one * scale;
    }
    public Vector3 GetPos()
    {
        Ray ray = new Ray(PlayerTrans.position + Vector3.up * 1, Vector3.down);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 20, GroundMask))
        {
            return hit.point;
        }
        return PlayerTrans.position;
    }
    private void Update()
    {
        transform.position = GetPos() + Vector3.up * 0.05f;

    }
}
