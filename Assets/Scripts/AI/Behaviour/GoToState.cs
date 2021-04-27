using Chronos;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Yarn.Unity;

public class GoToState : BaseState {

    private NPC npc;
    private NPCController controller;



    //private int _destPoint = 0;
    private Vector2 target;



    public GoToState(NPC _npc) : base(_npc.gameObject) {
        npc = _npc;
        controller = npc.GetComponent<NPCController>();
    }

    public override Type Tick() {

        controller.MoveTowards(npc.Target.position);
        return null;
    }
}
