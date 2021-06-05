using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Item
{
    public int id;
    public string name;
    public string title;
    public string description;
    public Sprite icon;

    public Item(int id, string name, string title, string description) {
        this.id = id;
        this.name = name;
        this.title = title;
        this.description = description;
        this.icon = Resources.Load<Sprite>("Sprites/Items/" + title);
    }

    public Item(Item item) {
        this.id = item.id;
        this.name = item.name;
        this.title = item.title;
        this.description = item.description;
        this.icon = Resources.Load<Sprite>("Sprites/Items/" + item.title);
    }
}

[System.Serializable]
public class Items {
    public Item[] items;
}
