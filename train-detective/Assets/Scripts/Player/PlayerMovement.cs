﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class PlayerMovement : MonoBehaviour
{

    public float _runSpeed = 20f;
    float _horizontalMove = 0f;
    private PlayerController _controller;

    private void Awake() {

        _controller = GetComponent<PlayerController>();

    }


    private void Update() {

        _horizontalMove = Input.GetAxisRaw("Horizontal") * _runSpeed;

        if (Input.GetKeyDown(KeyCode.E)) {
            _controller.CheckForNearbyNPC();
        }

         /* OBSOLETE
        if(_controller.CheckForward() != null)
            _controller.CheckForward().GetComponent<Door>().DisplayArrow();

        if(_controller.CheckBack() != null)
            _controller.CheckBack().GetComponent<Door>().HideArrow();
        */
    }

    private void FixedUpdate() {
        _controller.Move(_horizontalMove * Time.fixedDeltaTime);
        //Debug.Log(_controller.CheckForward().name);
    }
}
