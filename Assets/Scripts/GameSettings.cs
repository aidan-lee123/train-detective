using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSettings : MonoBehaviour {
    [SerializeField]
    private ItemDatabase itemDatabase;

    public static ItemDatabase ItemDatabase => Instance.itemDatabase;


    public static GameSettings Instance { get; private set; }

    private void Awake() {
        if (Instance != null)
            Destroy(gameObject);
        else
            Instance = this;
    }
}
