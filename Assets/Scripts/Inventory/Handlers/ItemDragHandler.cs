using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemDragHandler : MonoBehaviour, IDragHandler, IEndDragHandler, IBeginDragHandler
{

    private CanvasGroup canvasGroup;
    private RectTransform rectTransform;

    private void Awake() {
        canvasGroup = GetComponent<CanvasGroup>();
        rectTransform = GetComponent<RectTransform>();
    }

    public void OnBeginDrag(PointerEventData eventData) {
        Debug.Log("Dragging Item");
        canvasGroup.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData) {
        transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData) {
        canvasGroup.blocksRaycasts = true;

    }


}
