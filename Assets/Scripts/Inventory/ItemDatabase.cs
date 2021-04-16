using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDatabase : MonoBehaviour
{
    public List<Item> items = new List<Item>();

    public TextAsset itemListText;

    private void Awake() {
        BuildDatabase();
           
    }

    public Item GetItem(int id) {
        return items.Find(item => item.id == id);
    }

    public Item GetItem(string itemName) {
        return items.Find(item => item.title == itemName);
    }

    void BuildDatabase() {

        items = new List<Item> {
            new Item(0, "Test", "This is a test"),
            new Item(1, "Test2", "Test2")
        };
        
    }
}
