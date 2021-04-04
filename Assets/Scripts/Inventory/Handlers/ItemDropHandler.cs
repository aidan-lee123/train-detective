using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemDropHandler : MonoBehaviour, IDropHandler
{
    [SerializeField] private Transform itemChild;

    public void OnDrop(PointerEventData eventData) {
         if(eventData.pointerDrag != null) {

            //Set The itemChild's data to be the same as the new one effectively swapping them.
            ItemDragHandler newChild = eventData.pointerDrag.GetComponent<ItemDragHandler>();
            itemChild.SetParent(newChild.parentSlot);
            itemChild.localPosition = Vector3.zero;
            itemChild.GetComponent<ItemDragHandler>().parentSlot = newChild.parentSlot;

            //Set the Parent of the newChild to the new slot
            eventData.pointerDrag.transform.SetParent(transform);
            //Set the parentSlot of the Old slot to be the old item
            newChild.parentSlot.GetComponent<ItemDropHandler>().UpdateChild();


            newChild.parentSlot = transform;
            itemChild = eventData.pointerDrag.transform;
            itemChild.localPosition = Vector3.zero;





            Debug.Log("Updating Dropped On Slot");
        }
    }

    public void UpdateChild() {
        Debug.Log("Updating Child for " + name);
        itemChild = transform.GetChild(0);
    }

}
