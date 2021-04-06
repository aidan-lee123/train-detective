using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class UIItem : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {
    public Item item;
    private Image spriteImage;
    private ItemDragHandler dragHandler;
    private UIInventoryTitle itemTitle;

    private void Awake() {
        spriteImage = GetComponent<Image>();
        dragHandler = GetComponent<ItemDragHandler>();
        itemTitle = GameObject.Find("ItemName_Inventory").GetComponent<UIInventoryTitle>();
        //itemName.gameObject.SetActive(false);
        UpdateItem(null);
    }

    public void UpdateItem(Item item) {
        //Debug.Log("Updating Item " + name);
        this.item = item;
        if(this.item != null) {
            spriteImage.color = Color.white;
            spriteImage.sprite = this.item.icon;
        }
        else {
            spriteImage.color = Color.clear;
        }
    }

    public void OnPointerEnter(PointerEventData eventData) {
        if(item != null) {
            Debug.Log("Hovered Over " + item.title);
            itemTitle.HideText(false);
            itemTitle.UpdateText(item.title);
        }

        /*
        if(eventData.pointerEnter.GetComponent<Item>() != null) {
            itemName.text = item.title;
            itemName.gameObject.SetActive(true);
        }
        */
      }

    public void OnPointerExit(PointerEventData eventData) {
        if(item != null) {
            itemTitle.HideText(true);
        }
        /*
        itemName.text = null;
        itemName.gameObject.SetActive(false);
        */
    }
}
