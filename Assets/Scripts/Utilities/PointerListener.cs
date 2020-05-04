using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class PointerListener : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler
{
    public Action onDown;
    public Action onUp;
    public Action onEnter;
    public Action onExit;

    public void OnPointerDown(PointerEventData eventData)
    {
        onDown?.Invoke();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        onUp?.Invoke();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        onEnter?.Invoke();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        onExit?.Invoke();
    }
}
