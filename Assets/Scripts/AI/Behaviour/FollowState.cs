using Chronos;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Yarn.Unity;

public class FollowState : BaseState {
    private Vector2 _destination;
    private NPC npc;
    private NPCController controller;

    private Vector3 _velocity = Vector3.zero;
    private float _movementSmoothing = 0f;

    //private int _destPoint = 0;
    private Transform target;
    private Rigidbody2D _rigidBody;

    private bool _facingRight = true;
    private float _rayDistance = 0.5f;
    private Animator _animator;

    private Timeline time;


    public FollowState(NPC _npc, Transform _target) : base(_npc.gameObject) {
        npc = _npc;
        target = _target;
        _rigidBody = npc.GetComponent<Rigidbody2D>();
        _animator = npc.GetComponent<Animator>();
        time = npc.GetComponent<Timeline>();
        controller = npc.GetComponent<NPCController>();
    }

    public override Type Tick() {

        controller.Follow(target);


        return null;
    }

    private void Move() {

        //Vector2 dir = transform.position - destination.transform.position;

        Vector2 targetVelocity = Vector2.right * npc.Speed;

        if (!_facingRight)
            targetVelocity = Vector2.left * npc.Speed;

        if (npc.DialogueRunner.isDialogueRunning == true) {
            targetVelocity = Vector3.zero;
        }

        _animator.SetFloat("Speed", Mathf.Abs(targetVelocity.x));

        if (_facingRight)
            time.rigidbody2D.velocity = Vector3.SmoothDamp(_rigidBody.velocity, targetVelocity, ref _velocity, _movementSmoothing);
        else
            time.rigidbody2D.velocity = Vector3.SmoothDamp(_rigidBody.velocity, targetVelocity, ref _velocity, _movementSmoothing);
    }


    public void CheckForward() {

        Vector2 direction = new Vector2(1, 0);



        RaycastHit2D wallInfo = Physics2D.Raycast(npc.RayStart.transform.position, -direction, _rayDistance, LayerMask.GetMask("Environment"));

        Debug.DrawRay(npc.RayStart.transform.position, -direction, Color.white);

        if (wallInfo.collider == true) {
            Debug.DrawRay(npc.RayStart.transform.position, -direction, Color.red);
            if (_facingRight == true) {
                _facingRight = false;
                transform.eulerAngles = new Vector3(0, -180, 0);
            }
            else {
                transform.eulerAngles = new Vector3(0, 0, 0);
                _facingRight = true;
            }

        }

    }
}
