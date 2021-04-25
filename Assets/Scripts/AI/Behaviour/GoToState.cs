using Chronos;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Yarn.Unity;

public class GoToState : BaseState {
    private Vector2 _destination;
    private NPC npc;
    private NPCController controller;

    private Vector3 _velocity = Vector3.zero;
    private float _movementSmoothing = 0f;

    //private int _destPoint = 0;
    private Vector2 target;
    private Rigidbody2D _rigidBody;

    private bool _facingRight = true;
    private float _rayDistance = 0.5f;
    private Animator _animator;

    private Timeline time;


    public GoToState(NPC _npc) : base(_npc.gameObject) {
        npc = _npc;
        _rigidBody = npc.GetComponent<Rigidbody2D>();
        _animator = npc.GetComponent<Animator>();
        time = npc.GetComponent<Timeline>();
        controller = npc.GetComponent<NPCController>();
    }

    public override Type Tick() {

        controller.MoveTowards(npc.Target.position);
        return null;
    }
}
