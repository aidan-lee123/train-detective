﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Yarn.Unity;

public class NPC : MonoBehaviour
{
    public string _characterName = ""; 
    private int _playerMask;
    private int _npcMask;


    [FormerlySerializedAs("startNode")]
    public string talkToNode = "";

    [Header("Optional")]
    public TextAsset scriptToLoad;

    public StateMachine StateMachine => GetComponent<StateMachine>();

    public Transform Target;

    [SerializeField]
    public float Speed = 2f;
    public GameObject RayStart;
    private NPCController controller;

    public Transform[] Points;
    public Node[] path;
    int targetIndex;
    public DialogueRunner DialogueRunner => FindObjectOfType<DialogueRunner>();

    public SpriteRenderer spriteRenderer;

    public float moveSpeed = 0.01f;


    public void Awake() {
        _playerMask = LayerMask.GetMask("Player");
        _npcMask = LayerMask.GetMask("NPC");
        spriteRenderer = GetComponent<SpriteRenderer>();
        controller = GetComponent<NPCController>();
        Physics2D.IgnoreLayerCollision(9, 10);

        InitializeStateMachine();

        
        if (scriptToLoad != null) {
            DialogueRunner.AddScript(scriptToLoad);
        }
        
    }

    private void Update() {

    }

    public void AddScriptToDialogue() {
        if (scriptToLoad != null) {
            DialogueRunner.AddScript(scriptToLoad);
        }
    }

    [YarnCommand("GoTo")]
    public void GoTo(string targetName) {
        print("GOING TO " + targetName);
        GameObject targetObj = GameObject.Find(targetName);
        Target = targetObj.transform;

        //print(Target.position);

        StateMachine.SwitchToNewState(typeof(GoToState));
    }


    private void InitializeStateMachine() {
        var states = new Dictionary<Type, BaseState>() {
            {typeof(IdleState), new IdleState(this) },
            {typeof(GoToState), new GoToState(this) },
            //{typeof(PatrolState), new PatrolState(this) },
        };

        GetComponent<StateMachine>().SetStates(states);
    }

    public void SetTarget(Transform target) {
        Target = target;
    }

    public Transform[] GetPoints() {
        return Points;
    }

    public void FollowPath(Node[] _path, bool pathSuccessful) {

        if (pathSuccessful) {
            path = _path;


            StopCoroutine("NavigatePath");
            StartCoroutine("NavigatePath");
            //isWalking = false;
            
        }
        else {
            Debug.Log("Could not Path to Node");
            
            StopCoroutine("NavigatePath");
            //isWalking = false;
            
        }
    }

    IEnumerator NavigatePath() {
        Node currentNode = path[0];
        Debug.Log("Beginning Follow Path");

        //Check if the next node has a linked door and its link is not the current node

        while (true) {
            //Debug.Log(Vector2.Distance(transform.position, currentNode.worldPosition));
            if (Vector2.Distance(transform.position, currentNode.worldPosition) < 0.1f) {
                targetIndex++;

                if (currentNode.door != null) {
                    print("currentNode door is not null");
                    controller.TraverseDoor(currentNode.door, currentNode.door.link, 5f);
                    targetIndex++;
                }


                if (targetIndex >= path.Length) {
                    targetIndex = 0;
                    path = new Node[0];
                    yield break;
                }

                currentNode = path[targetIndex];
            }

            controller.MoveTowards(currentNode.worldPosition);


            yield return null;
        }
    }

    public void OnDrawGizmos() {

        if (path != null) {
            for (int i = targetIndex; i < path.Length; i++) {
                Gizmos.color = Color.black;
                Gizmos.DrawCube(path[i].worldPosition, Vector3.one);
                if (i == targetIndex) {
                    Gizmos.DrawLine(transform.position, path[i].worldPosition);
                }
                else {
                    Gizmos.DrawLine(path[i - 1].worldPosition, path[i].worldPosition);
                }
            }
        }
    }

}
