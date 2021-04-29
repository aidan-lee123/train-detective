using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public List<Item> characterItems = new List<Item>();
    public ItemDatabase itemDatabase;
    public UIInventory inventoryUI;

    // Start is called before the first frame update
    void Start()
    {
        GiveItem("Test");
        GiveItem("Test");
    }

    public void GiveItem(int id) {
        Item itemToAdd = itemDatabase.GetItem(id);
        characterItems.Add(itemToAdd);
        inventoryUI.AddNewItem(itemToAdd);
        //Debug.Log("Added Item: " + itemToAdd.title);
    }

    public void GiveItem(string name) {
        Item itemToAdd = itemDatabase.GetItem(name);
        characterItems.Add(itemToAdd);
        inventoryUI.AddNewItem(itemToAdd);
        Debug.Log("Added Item: " + itemToAdd.title);
    }
}
