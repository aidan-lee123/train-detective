using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class PlayerMovement : MonoBehaviour
{
    private PlayerController _controller;
    public GameObject InventoryUI;

    private bool showInventory = true;

    private void Awake() {

        _controller = GetComponent<PlayerController>();
        
    }


    private void Update() {



        if (Input.GetKeyDown(KeyCode.E)) {
            _controller.CheckForNearbyNPC();
            _controller.CheckForNearbyInteractable();
        }

        if (Input.GetKeyDown(KeyCode.Tab)) {
            showInventory = !showInventory;

            InventoryUI.GetComponent<UIInventory>().HideInventory(showInventory);
        }
    }

    private void FixedUpdate() {

        //Debug.Log(_controller.CheckForward().name);
    }
}
