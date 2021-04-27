using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CabinManager : MonoBehaviour
{

    private List<Cabin> cabins;

    public static CabinManager Instance;

    private void Awake() {
        
        Instance = this;

        Instance.cabins = new List<Cabin>(GameObject.FindObjectsOfType<Cabin>());
    }

    public event Action<int> onCabinEnter;
    public void CabinEnter(int id) {
        if(onCabinEnter != null) {
            onCabinEnter(id);
        }
    }

    public event Action<int> onCabinExit;
    public void CabinExit(int id) {
        if(onCabinExit != null) {
            onCabinExit(id);
        }
    }

    public static List<Cabin> GetCabins() {
        return Instance.cabins;
    }

    public static Cabin GetCabin(string name) {
        return Instance.cabins.Find(x => x.cabinName == name);
    }
}
