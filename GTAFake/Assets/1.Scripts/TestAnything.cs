using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Burst.CompilerServices;
using UnityEditor.Media;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;
using UnityEngine.UI;
using Unity.VisualScripting;

public class TestAnything : MonoBehaviour
{
    public Transform player; // Transform của player
    public Camera camera;    // Camera đang sử dụng
    public LayerMask layerMask;
    public List<MeshRenderer> currentHidden = new List<MeshRenderer>();
    void Update()
    {
        string a = " ";
        List<GameObject> ObjectBlockingView = IsObjectBlockingView();
        List<MeshRenderer> currentHiddenCache = new List<MeshRenderer>(currentHidden);
        foreach (var obj in ObjectBlockingView)
        {
            a += obj.name + " ";
            if (obj.GetComponent<MeshRenderer>() != null && currentHidden.Contains(obj.GetComponent<MeshRenderer>()) == false)
            {
                currentHidden.Add(obj.GetComponent<MeshRenderer>());
                obj.GetComponent<MeshRenderer>().enabled = false;
            }
        }
        foreach (var obj in currentHiddenCache)
        {
            if (ObjectBlockingView.Contains(obj.gameObject) == false)
            {
                currentHidden.Remove(obj);
                obj.enabled = true;
            }
        }
    }

    List<GameObject> IsObjectBlockingView()
    {
        List<GameObject> result = new List<GameObject>();
        List<GameObject> Cache = GetOverlappingObjects(player.gameObject, layerMask);
        Cache.AddRange(GetOverlappingObjects(camera.gameObject, layerMask));
        foreach (var obj in Cache)
        {
            if (result.Contains(obj) == false && obj != player.gameObject && obj != camera.gameObject)
            {
                result.Add(obj);
            }
        }

        // Tính toán vị trí bắt đầu của ray (từ vị trí của camera)
        Vector3 rayOrigin = camera.transform.position;
        // Tính toán hướng của ray (từ camera đến player)
        Vector3 rayDirection = player.position - camera.transform.position;

        // Tạo ray từ vị trí camera đến vị trí của player
        Ray ray = new Ray(rayOrigin, rayDirection);
        RaycastHit[] hits;

        // Tính khoảng cách từ camera đến player
        float distanceToPlayer = Vector3.Distance(camera.transform.position, player.position);

        hits = Physics.RaycastAll(ray, distanceToPlayer);
        // Thực hiện Raycast, kiểm tra va chạm với tất cả các object trong scene
        if (hits.Length > 0)
        {
            // Nếu ray va chạm với một object, kiểm tra xem object đó có phải là player hay không
            foreach (var hit in hits)
            {
                if (result.Contains(hit.collider.gameObject) == false && hit.collider.gameObject != player.gameObject && hit.collider.gameObject != camera.gameObject)
                {
                    result.Add(hit.collider.gameObject);
                }
            }
        }
        // Nếu ray không va chạm với bất kỳ object nào hoặc chỉ va chạm với player
        return result;
    }


    List<GameObject> GetOverlappingObjects(GameObject target, LayerMask mask)
    {
        List<GameObject> overlappingObjects = new List<GameObject>();

        Collider targetCollider = target.GetComponent<Collider>();
        if (targetCollider == null)
        {
            Debug.LogWarning("The target object does not have a Collider component.");
            return overlappingObjects;
        }

        Vector3 targetCenter = targetCollider.bounds.center;
        Vector3 targetExtents = targetCollider.bounds.extents;
        Collider[] colliders = Physics.OverlapBox(targetCenter, targetExtents * 30, Quaternion.identity, mask);

        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject != target && Physics.OverlapBox(colliders[i].bounds.center, colliders[i].bounds.extents, Quaternion.identity, mask).Contains(targetCollider))
            {
                overlappingObjects.Add(colliders[i].gameObject);
            }
        }

        return overlappingObjects;
    }
}
