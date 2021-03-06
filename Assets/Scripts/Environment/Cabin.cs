﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;
using System;

public class Cabin : MonoBehaviour
{
    public int cabinId;
    public string cabinName;

    public CabinBounds cabinBounds;


    public List<Door> doors => new List<Door>(GetComponentsInChildren<Door>());


    public List<GameObject> _actors = new List<GameObject>();
    public GameObject cabinSprite;
    public List<NPC> _npcs = new List<NPC>();
    public List<Node> nodes = new List<Node>();

    public DialogueRunner DialogueRunner => FindObjectOfType<DialogueRunner>();

    public bool IsNodeInCabin(Node node) {
        if (cabinBounds.cabinCollider.bounds.Contains(node.worldPosition)) {
            return true;
        }
        return false;
    }

    public bool AddNode(Node node) {
        if (!nodes.Contains(node)) {
            nodes.Add(node);
            return true;
        }
        return false;

    }

    public void CabinEnter(GameObject actor) {
        _actors.Add(actor);
        if (actor.GetComponent<NPC>() != null) {
            _npcs.Add(actor.GetComponent<NPC>());
        }

    }

    public void CabinExit(GameObject actor) {
        _actors.Remove(actor);
        if (actor.GetComponent<NPC>() != null) {
            _npcs.Remove(actor.GetComponent<NPC>());
        }
    }


    public void OnPlayerEnter(int id) {
        if(id == cabinId) {
            //print("Player Entered " + name);
            //Array.Clear(DialogueRunner.sourceText, 0, DialogueRunner.sourceText.Length);
            cabinSprite.SetActive(false);

            foreach(NPC npc in _npcs) {
                //npc.AddScriptToDialogue();
            }

            //print(DialogueRunner.sourceText.Length);

        }

    }

    public void OnPlayerExit(int id) {
        if(id == cabinId) {
            cabinSprite.SetActive(true);
            //print("Player Exited " + name);


        }

    }

    void Start()
    {
        cabinSprite.SetActive(true);
        CabinManager.Instance.onCabinEnter += OnPlayerEnter;
        CabinManager.Instance.onCabinExit += OnPlayerExit;
    }

}
