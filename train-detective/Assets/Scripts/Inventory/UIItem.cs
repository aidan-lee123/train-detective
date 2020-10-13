﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UIItem : MonoBehaviour, IPointerClickHandler
{
    public Item item;
    private Image spriteImage;
    private UIItem selectedItem;

    private void Awake() {
        spriteImage = GetComponent<Image>();
        UpdateItem(null);
        selectedItem = GameObject.Find("SelectedItem").GetComponent<UIItem>();
    }

    public void UpdateItem(Item item) {
        this.item = item;
        if(this.item != null) {
            spriteImage.color = Color.white;
            spriteImage.sprite = this.item.icon;
        }
        else {
            spriteImage.color = Color.clear;
        }
    }

    public void OnPointerClick(PointerEventData eventData) {
        Debug.Log(name + " Game Object Clicked!");

        //If the item that is being clicked on is not null
        if (this.item != null) {
            //if the selected item already has an item when we click we replace the one below it
            if(selectedItem.item != null) {
                Item clone = new Item(selectedItem.item);
                selectedItem.UpdateItem(this.item);
                UpdateItem(clone);
            }
            //Otherwise update the item being clicked to null and select the selected item to this item
            else {
                selectedItem.UpdateItem(this.item);
                UpdateItem(null);
            }
        }
        //if the selected item does not equal null we replace the item we are clicking on with it and set selected item to null
        else if (selectedItem.item != null) {
             UpdateItem(selectedItem.item);
             selectedItem.UpdateItem(null);
            
        }
    }
}
