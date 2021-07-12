using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Alphabet : MonoBehaviour, IDragHandler, IEndDragHandler, IBeginDragHandler
{
    [SerializeField]
    bool drag;
    [SerializeField]
    Vector2 startPoint;
    [SerializeField]
    Vector2 endPoint;
    [SerializeField]
    private RectTransform dragRectTransform;

    void Start()
    {
        dragRectTransform = GetComponent<RectTransform>();
    }
    void IBeginDragHandler.OnBeginDrag(PointerEventData eventData)
    {
        startPoint = eventData.pressPosition;
    }

    public void OnDrag(PointerEventData eventData)
    {
        dragRectTransform.anchoredPosition += eventData.delta;
        Debug.Log("dragging");
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        drag = false;
    }


}
