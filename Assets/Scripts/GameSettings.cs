using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSettings : MonoBehaviour {
    [SerializeField]
    private float robotSpeed = 2f;

    public static float RobotSpeed => Instance.robotSpeed;

    [SerializeField]
    private float aggroRadius = 4f;

    public static float AggroRadius => Instance.aggroRadius;

    [SerializeField]
    private float attackRange = 3f;

    public static float AttackRange => Instance.attackRange;


    public static GameSettings Instance { get; private set; }

    private void Awake() {
        if (Instance != null)
            Destroy(gameObject);
        else
            Instance = this;
    }
}
