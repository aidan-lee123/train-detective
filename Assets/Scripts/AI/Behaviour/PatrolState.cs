using Chronos;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Yarn.Unity;

public class PatrolState : BaseState {
    private Vector2 _destination;
    private NPC _npc;

    private Vector3 _velocity = Vector3.zero;
    private float _movementSmoothing = 0f;

    //private int _destPoint = 0;
    private Transform[] _patrolPoints;
    private Rigidbody2D _rigidBody;

    private bool _facingRight = true;
    private float _rayDistance = 0.5f;
    private Animator _animator;

    private Timeline time;


    public PatrolState(NPC npc) : base(npc.gameObject) {
        _npc = npc;
        _patrolPoints = _npc.GetPoints();
        _rigidBody = npc.GetComponent<Rigidbody2D>();
        _animator = npc.GetComponent<Animator>();
        time = npc.GetComponent<Timeline>();

    }

    public override Type Tick() {

        //var followTarget = CheckForAggro();
        /* Debug.Log(_destPoint);
         Debug.Log(_destination + " " + transform.position); */
        //Does it have a Destination
        //No Give it one
        //transform.Translate(Vector2.right * _npc.Speed * Time.deltaTime);

        // Remove all player control when we're in dialogue
        


        Move();
        CheckForward();


        return null;
    }

    private void Move() {

        //Vector2 dir = transform.position - destination.transform.position;
        
        Vector2 targetVelocity = Vector2.right * _npc.Speed;

        if (!_facingRight)
            targetVelocity = Vector2.left * _npc.Speed;

        if (_npc.DialogueRunner.isDialogueRunning == true) {
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



        RaycastHit2D wallInfo = Physics2D.Raycast(_npc.RayStart.transform.position, -direction, _rayDistance, LayerMask.GetMask("Environment"));

        Debug.DrawRay(_npc.RayStart.transform.position, -direction, Color.white);

        if(wallInfo.collider == true) {
            Debug.DrawRay(_npc.RayStart.transform.position, -direction, Color.red);
            if (_facingRight == true) {
                _facingRight = false;
                transform.eulerAngles = new Vector3(0, -180, 0);
            } else {
                transform.eulerAngles = new Vector3(0, 0, 0);
                _facingRight = true;
            }

        }

    }
}
