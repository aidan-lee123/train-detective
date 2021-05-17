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
    int targetIndex;
    bool requestedPath = false;

    //private int _destPoint = 0;
    private Node[] path;
    private Vector3 target;


    public GoToState(NPC _npc) : base(_npc.gameObject) {
        npc = _npc;
        controller = npc.GetComponent<NPCController>();
    }



    public override Type Tick() {

        //if we have dont have a target find a route
        if (requestedPath == false) {
            Debug.Log("Requesting Path");
            PathRequestManager.RequestPath(npc.transform.position, npc.Target.position, PathFound);
            requestedPath = true;
        }
        return null;
    }




    public void PathFound(Node[] newPath, bool pathSuccessful) {
        Debug.Log("Path Found");
        path = newPath;
        npc.FollowPath(newPath, pathSuccessful);


    }


}
