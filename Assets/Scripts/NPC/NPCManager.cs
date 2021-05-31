using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCManager : MonoBehaviour
{
    public static NPCManager Instance { get; private set; }


    public List<NPC> NPCs = new List<NPC>();


    private void Awake() {
        if (Instance != null)
            Destroy(gameObject);
        else
            Instance = this;
    }

    public void AddNPC(NPC npc) {
        NPCs.Add(npc);
    }

    public void KillAllNPCs() {
        foreach(NPC npc in NPCs) {
            Destroy(npc.gameObject);
        }
    }



}
