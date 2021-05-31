using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class ItemDragHandler : MonoBehaviour, IDragHandler, IEndDragHandler, IBeginDragHandler
{
    private CanvasGroup canvasGroup;
    public Transform parentSlot;

    private bool droppedOnSlot;
    public bool DroppedOnSlot {
        get {
            return droppedOnSlot;
        }
        set {
            value = droppedOnSlot;
        }
    }

    private void Awake() {
        canvasGroup = GetComponent<CanvasGroup>();
        parentSlot = transform.parent;
    }

    public void OnBeginDrag(PointerEventData eventData) {
        UIItem clickedItem = eventData.pointerDrag.GetComponent<UIItem>();
        canvasGroup.blocksRaycasts = false;
        droppedOnSlot = false;
    }

    public void OnDrag(PointerEventData eventData) {
        transform.position = Mouse.current.position.ReadValue();
    }

    public void OnEndDrag(PointerEventData eventData) {
        UIItem droppedItem = eventData.pointerDrag.GetComponent<UIItem>();
        transform.localPosition = Vector3.zero;
        canvasGroup.blocksRaycasts = true;

    }

}
