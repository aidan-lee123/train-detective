using Chronos;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Yarn.Unity;

public class PlayerInput : MonoBehaviour
{
    private PlayerController _controller;
    public GameObject InventoryUI;
    private Timeline time;

    private bool showInventory = true;
    public Door currentDoor;

    public float gravity = -10;
    public float moveSpeed = 6;
    private float moveSpeedModifier = 1f;
    Vector3 velocity;

    Vector2 inputMove = Vector2.zero;

    private void Start() {

        _controller = GetComponent<PlayerController>();
        time = GetComponent<Timeline>();
        
    }


    private void Update() {


        if(_controller.collisions.above || _controller.collisions.below) {
            velocity.y = 0;
        }

        velocity.y += gravity * time.deltaTime;
        velocity.x = inputMove.x * (moveSpeed * moveSpeedModifier);
        _controller.Move(velocity);
    }

    public void OnMove(InputAction.CallbackContext value) {
        inputMove = value.ReadValue<Vector2>();
    }

    public void OnInteract(InputAction.CallbackContext value) {
        if (value.performed) {
            if (currentDoor != null) {
                currentDoor.MoveCharacter(gameObject);
            }
            _controller.CheckForNearbyNPC();
            _controller.CheckForNearbyInteractable();
        }

    }

    public void OnSprint(InputAction.CallbackContext value) {

        if (value.performed) {
            moveSpeedModifier = 1.5f;
        }
        if (value.canceled) {
            moveSpeedModifier = 1f;
        }
        /*
        if (Input.GetKeyDown(KeyCode.LeftShift)) {
            moveSpeedModifier = 1.5f;
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift)) {
            moveSpeedModifier = 1f;
        }
        */
    }

    public void OnInventoryOpen(InputAction.CallbackContext value) {
        showInventory = !showInventory;

        InventoryUI.GetComponent<UIInventory>().HideInventory(showInventory);
    }

}
