using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CabinManager : MonoBehaviour
{

    public static CabinManager Instance;

    private void Awake() {
        Instance = this;
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
}
