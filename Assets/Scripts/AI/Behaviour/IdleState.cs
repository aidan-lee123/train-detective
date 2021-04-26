using Chronos;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Yarn.Unity;

public class IdleState : BaseState {
    private NPC npc;
    private NPCController controller;

    private Vector3 _velocity = Vector3.zero;
    private Animator _animator;
    private Timeline time;


    public IdleState(NPC _npc) : base(_npc.gameObject) {
        npc = _npc;
        _animator = npc.GetComponent<Animator>();
        time = npc.GetComponent<Timeline>();
        controller = npc.GetComponent<NPCController>();
    }

    public override Type Tick() {


        return null;
    }

}
