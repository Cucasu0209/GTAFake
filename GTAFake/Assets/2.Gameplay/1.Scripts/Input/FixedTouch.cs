using UnityEngine;
using UnityEngine.EventSystems;

public class FixedTouch : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [HideInInspector]
    public Vector2 TouchDist;
    [HideInInspector]
    public Vector2 PointerOld;
    [HideInInspector]
    protected int PointerId;
    [HideInInspector]
    public bool Pressed;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Pressed)
        {
            if (PointerId >= 0 && PointerId < Input.touches.Length)
            {
                TouchDist = Input.touches[PointerId].position - PointerOld;
                PointerOld = Input.touches[PointerId].position;
            }
            else
            {
                TouchDist = new Vector2(Input.mousePosition.x, Input.mousePosition.y) - PointerOld;
                PointerOld = Input.mousePosition;
            }
            UserInputController.Instance.OnCameraAxisChange?.Invoke(TouchDist.x, TouchDist.y);
        }
        else if (TouchDist.magnitude != 0)
        {
            TouchDist = Vector2.zero;
            UserInputController.Instance.OnCameraAxisChange?.Invoke(0, 0);
        }
        else
        {
            UserInputController.Instance.OnCameraAxisChange?.Invoke(0, 0);
        }


    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Pressed = true;
        PointerId = eventData.pointerId;
        PointerOld = eventData.position;


    }


    public void OnPointerUp(PointerEventData eventData)
    {
        Pressed = false;
    }

}