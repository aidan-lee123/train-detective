using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

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
        Debug.Log("Dragging Item");
        UIItem clickedItem = eventData.pointerDrag.GetComponent<UIItem>();
        canvasGroup.blocksRaycasts = false;
        droppedOnSlot = false;

        //selectedItem.UpdateItem(clickedItem.item);
        //selectedItem.originalItem = clickedItem;
        //clickedItem.UpdateItem(null);

    }

    public void OnDrag(PointerEventData eventData) {
        transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData) {
        UIItem droppedItem = eventData.pointerDrag.GetComponent<UIItem>();
        StartCoroutine(WasDropped(droppedItem));
        canvasGroup.blocksRaycasts = true;

    }

    private IEnumerator WasDropped(UIItem droppedItem) {
        Debug.Log(droppedOnSlot);
        yield return new WaitForEndOfFrame();

        if (!droppedOnSlot) {
            Debug.Log("Not Dropped on Slot");
            transform.localPosition = Vector3.zero;
        }
        else {
            Debug.Log("Dropped on Slot");
        }
    }


}
