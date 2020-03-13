using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class NPCInfo : MonoBehaviour
{
    public string _characterName = ""; 
    private int _playerMask;
    private int _npcMask;


    [FormerlySerializedAs("startNode")]
    public string talkToNode = "";

    [Header("Optional")]
    public TextAsset scriptToLoad;

    public void Awake(){
        _playerMask = LayerMask.GetMask("Player");
        _npcMask = LayerMask.GetMask("NPC");

        Physics2D.IgnoreLayerCollision(9, 10);
    }



    // Use this for initialization
    void Start() {
        if (scriptToLoad != null) {
            FindObjectOfType<Yarn.Unity.DialogueRunner>().AddScript(scriptToLoad);
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
