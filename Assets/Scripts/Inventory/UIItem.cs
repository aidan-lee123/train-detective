using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class UIItem : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler {
    public Item item;
    private Image spriteImage;
    private ItemDragHandler dragHandler;
    private UIInventoryTitle itemTitle;
    private UIInspectItem inspector;

    private static LTDescr delay;

    private void Awake() {
        spriteImage = GetComponent<Image>();
        dragHandler = GetComponent<ItemDragHandler>();
        itemTitle = GameObject.Find("ItemName_Inventory").GetComponent<UIInventoryTitle>();
        inspector = GameObject.Find("InventoryInspector").GetComponent<UIInspectItem>();
        UpdateItem(null);
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

    public void OnPointerEnter(PointerEventData eventData) {
        if(item != null) {
            delay = LeanTween.delayedCall(0.5f, () => {
                Debug.Log("Hovered Over " + item.title);
                //itemTitle.HideText(false);
                //itemTitle.UpdateText(item.title);

                TooltipSystem.Show(item.title);
            });

        }

      }

    public void OnPointerExit(PointerEventData eventData) {
        if(item != null) {
            //itemTitle.HideText(true);
            TooltipSystem.Hide();
        }

    }

    public void OnPointerClick(PointerEventData eventData) {
        Debug.Log(name + " clicked");
        inspector.SetItem(item);
    }
}
