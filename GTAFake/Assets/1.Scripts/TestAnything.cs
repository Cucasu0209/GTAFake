using System.Collections.Generic;
using UnityEngine;


public class TestAnything : MonoBehaviour
{
    public Transform player; // Transform của player
    public Camera camera;    // Camera đang sử dụng
    public LayerMask layerMask;
    public List<MeshRenderer> currentHidden = new List<MeshRenderer>();
    void Update()
    {
        //string a = " ";
        List<GameObject> ObjectBlockingView = IsObjectBlockingView();
        List<MeshRenderer> currentHiddenCache = new List<MeshRenderer>(currentHidden);
        foreach (var obj in ObjectBlockingView)
        {
            //a += obj.name + " ";
            if (obj.GetComponent<MeshRenderer>() != null && currentHidden.Contains(obj.GetComponent<MeshRenderer>()) == false)
            {
                currentHidden.Add(obj.GetComponent<MeshRenderer>());
                obj.GetComponent<MeshRenderer>().enabled = false;
                obj.transform.GetChild(0).GetComponent<Collider>().enabled = false;
            }

        }
        foreach (var obj in currentHiddenCache)
        {
            if (ObjectBlockingView.Contains(obj.gameObject) == false)
            {
                currentHidden.Remove(obj);
                obj.enabled = true;
                obj.transform.GetChild(0).GetComponent<Collider>().enabled = true;
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

        hits = Physics.RaycastAll(ray, distanceToPlayer, layerMask);
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

        Collider[] colliders = Physics.OverlapBox(target.transform.position, Vector3.up * 1000, Quaternion.Euler(0, 0, 0), mask);

        for (int i = 0; i < colliders.Length; i++)
        {
            overlappingObjects.Add(colliders[i].gameObject);
        }

        return overlappingObjects;
    }



}
