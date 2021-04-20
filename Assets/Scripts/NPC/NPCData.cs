using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCData : ScriptableObject
{
    public new string name;
    public string description;
    public Sprite sprite;
    public TextAsset script;

    public Cabin cabin;
}
