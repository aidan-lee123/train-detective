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
    private NPCController controller;

    //Pathfinding stuff
    public Node[] path;
    int targetIndex;
    Node currentNode;

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

    private void Start() {
        NPCManager.Instance.AddNPC(this);
    }

    private void Update() {

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
        Vector2 previousPosition = transform.position;
        currentNode = path[0];
        Node nextNode = path[1];
        Debug.Log("Beginning Follow Path");

        //Check if the next node has a linked door and its link is not the current node

        while (true) {
            //Debug.Log(Vector2.Distance(transform.position, currentNode.worldPosition));
            if (Vector2.Distance(new Vector2(transform.position.x, currentNode.worldPosition.y), currentNode.worldPosition) < 0.1f) {
                Debug.Log("Just Arrived at: " + currentNode.NodeName);
                Debug.Log("Parent Node is: " + currentNode.parent.NodeName);
                if (currentNode.door != null && currentNode.door.link == nextNode.door) {
                    print("currentNode door is not null");
                    controller.TraverseDoor(currentNode.door, currentNode.door.link, 5f);
                }

                targetIndex++;
                if (targetIndex >= path.Length) {
                    targetIndex = 0;
                    path = new Node[0];
                    yield break;
                }
                previousPosition = currentNode.worldPosition;
                currentNode = path[targetIndex];
                if (path.Length - 1 > targetIndex + 1)
                    nextNode = path[targetIndex+1];



                Debug.Log("Heading To: " + currentNode.NodeName);
            }

            controller.MoveTowards(currentNode.worldPosition);


            yield return null;
        }
    }

    public void OnDrawGizmos() {

        if (path != null) {
            for (int i = targetIndex; i < path.Length; i++) {
                Gizmos.color = Color.black;
                UnityEditor.Handles.Label(new Vector3(path[i].worldPosition.x - Vector3.one.x, path[i].worldPosition.y + 1), path[i].NodeName);
                UnityEditor.Handles.Label(new Vector3(path[i].worldPosition.x - Vector3.one.x, path[i].worldPosition.y + 1.3f), path[i].worldPosition.ToString());
                Gizmos.DrawCube(path[i].worldPosition, Vector3.one);
                if (i == targetIndex) {
                    Gizmos.DrawLine(transform.position, path[i].worldPosition);
                }
                else {
                    Gizmos.DrawLine(path[i - 1].worldPosition, path[i].worldPosition);
                }
            }
        }

        if(currentNode != null) {
            UnityEditor.Handles.Label(new Vector3(transform.position.x - 1.5f, transform.position.y + 1.5f), "Heading to: " + currentNode.NodeName);
        }
    }

}
