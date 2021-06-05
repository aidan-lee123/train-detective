using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    public static Player Instance { get; private set; }

    private InventoryManager inventoryManager;
    public ItemDatabase itemDatabase;
    public UIInventory inventoryUI;

    private float _money;

    public float Money { get { return Instance._money; } }

    private void Awake() {
        if (Instance != null)
            Destroy(gameObject);
        else
            Instance = this;

        inventoryManager = GetComponent<InventoryManager>();
    }

    public void AddMoney(float value) {
        Instance._money += value;
        PlayerHUD.Instance.SetMoney();
    }

    public void SetMoney(float value) {
        Instance._money = value;
        PlayerHUD.Instance.SetMoney();
    }

    public void GiveItem(int id) {
        Item itemToAdd = itemDatabase.GetItem(id);
        inventoryManager.characterItems.Add(itemToAdd);
        inventoryUI.AddNewItem(itemToAdd);
    }

    public void GiveItem(string name) {
        Item itemToAdd = itemDatabase.GetItem(name);
        inventoryManager.characterItems.Add(itemToAdd);
        inventoryUI.AddNewItem(itemToAdd);
    }

}
