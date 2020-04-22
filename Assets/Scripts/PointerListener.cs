using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class PointerListener : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public Action onDown;
    public Action onUp;

    public void OnPointerDown(PointerEventData eventData)
    {
        onDown?.Invoke();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        onUp?.Invoke();
    }
}
