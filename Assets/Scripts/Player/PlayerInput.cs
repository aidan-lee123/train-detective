using Chronos;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class PlayerInput : MonoBehaviour
{
    private PlayerController _controller;
    public GameObject InventoryUI;
    private Timeline time;

    private bool showInventory = true;

    public float gravity = -10;
    public float moveSpeed = 6;
    private float moveSpeedModifier = 1f;
    Vector3 velocity;

    private void Start() {

        _controller = GetComponent<PlayerController>();
        time = GetComponent<Timeline>();
        
    }


    private void Update() {

        if(Input.GetKeyDown(KeyCode.LeftShift)) {
            moveSpeedModifier = 1.5f;
        } else if (Input.GetKeyUp(KeyCode.LeftShift)){
            moveSpeedModifier = 1f;
        }

        if(_controller.collisions.above || _controller.collisions.below) {
            velocity.y = 0;
        }

        Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        velocity.y += gravity * time.deltaTime;
        velocity.x = input.x * (moveSpeed * moveSpeedModifier);
        _controller.Move(velocity);

        //Interacting with things
        if (Input.GetKeyDown(KeyCode.E)) {
            _controller.CheckForNearbyNPC();
            _controller.CheckForNearbyInteractable();
        }

        //Open Inventory
        if (Input.GetKeyDown(KeyCode.Tab)) {
            showInventory = !showInventory;

            InventoryUI.GetComponent<UIInventory>().HideInventory(showInventory);
        }
    }
}
