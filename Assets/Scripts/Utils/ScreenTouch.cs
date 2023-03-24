using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
[RequireComponent(typeof(RectTransform))]
public class ScreenTouch : MonoBehaviour, IDragHandler, IPointerDownHandler, IPointerUpHandler
{
    public float Height { get; private set; }
    public float Inv_Height { get; private set; }


    private void Awake()
    {
        Height = GetComponent<RectTransform>().rect.height;
        Inv_Height = 1f / Height;
    }

    public void OnPointerDown(PointerEventData data)
    {
        InputControl.Instance.On_Pointer_Down(data.position);
    }

    public void OnPointerUp(PointerEventData data)
    {
        InputControl.Instance.On_Pointer_Up(data.position);
    }

    public void OnDrag(PointerEventData data)
    {
        InputControl.Instance.On_Drag(data.delta);
    }
}
