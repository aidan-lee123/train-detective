using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDatabase : MonoBehaviour
{
    public List<Item> items = new List<Item>();

    public TextAsset[] itemFiles;

    private void Awake() {
        BuildDatabase();
           
    }

    public Item GetItem(int id) {
        return items.Find(item => item.id == id);
    }

    public Item GetItem(string itemName) {
        return items.Find(item => item.name == itemName);
    }

    void BuildDatabase() {

        foreach(TextAsset file in itemFiles) {
            Items itemsInFile = JsonUtility.FromJson<Items>(file.text);

            foreach(Item item in itemsInFile.items) {
                item.icon = Resources.Load<Sprite>("Sprites/Items/" + item.title);
                items.Add(item);
            }
        }
        
    }
}
