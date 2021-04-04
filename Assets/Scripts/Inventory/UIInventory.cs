using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIInventory : MonoBehaviour
{
    public List<UIItem> UIItems = new List<UIItem>();
    public GameObject slotPrefab;
    public Transform slotPanel;

    public int numberOfSlots = 16;

    private void Awake() {
        for (int i = 0; i < numberOfSlots; i++) {
            GameObject instance = Instantiate(slotPrefab);
            instance.transform.SetParent(slotPanel);
            instance.name = "Slot " + (i + 1);
            UIItems.Add(instance.GetComponentInChildren<UIItem>());
        }

        HideInventory(true);
    }

    public void HideInventory(bool state) {

        RectTransform rect = GetComponent<RectTransform>();

        if (state) {
            rect.localScale = new Vector3(0, 0, 0);
        }
        else {
            rect.localScale = new Vector3(1, 1, 1);
        }
    }

    public void UpdateSlot(int slot, Item item) {
        UIItems[slot].UpdateItem(item);
    }
    public void AddNewItem(Item item) {
        UpdateSlot(UIItems.FindIndex(i => i.item == null), item);
    }
    public void RemoveItem(Item item) {
        UpdateSlot(UIItems.FindIndex(i => i.item == item), null);
    }
}
