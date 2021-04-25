using System;
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


    public Transform[] Points;
    public DialogueRunner DialogueRunner => FindObjectOfType<DialogueRunner>();

    public SpriteRenderer spriteRenderer;

    public float moveSpeed = 0.01f;


    public void Awake() {
        _playerMask = LayerMask.GetMask("Player");
        _npcMask = LayerMask.GetMask("NPC");
        spriteRenderer = GetComponent<SpriteRenderer>();
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

}
