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
    private Vector3[] path;
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


        //controller.MoveTowards(npc.Target.position);
        return null;
    }

    IEnumerator FollowPath() {
        Vector3 currentWaypoint = path[0];
        Debug.Log("Beginning Follow Path");

        //Check if the next node has a linked door and its link is not the current node

        while (true) {
            if (transform.position == currentWaypoint) {
                targetIndex++;
                if (targetIndex >= path.Length) {
                    targetIndex = 0;
                    path = new Vector3[0];
                    yield break;
                }

                currentWaypoint = path[targetIndex];
            }

            controller.MoveTowards(currentWaypoint);


            yield return null;
        }
    }


    public void PathFound(Vector3[] newPath, bool pathSuccessful) {
        Debug.Log("Path Found");
        if (pathSuccessful) {
            path = newPath;
            npc.path = newPath;
            /*
            StopCoroutine("FollowPath");
            StartCoroutine("FollowPath");
            isWalking = false;
            */
        }
        else {
            Debug.Log("Could not Path to Node");
            /*
            StopCoroutine("FollowPath");
            isWalking = false;
            */
        }

    }


}
