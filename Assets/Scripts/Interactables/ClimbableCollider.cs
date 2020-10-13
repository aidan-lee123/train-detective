using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClimbableCollider : MonoBehaviour
{

    [SerializeField] bool isDown;

    ClimbableManager climbableManager;

    private void Awake() {
        climbableManager = GetComponentInParent<ClimbableManager>();
    }

    #region Triggers
    void OnTriggerEnter2D(Collider2D other) {

        if (other.tag == "NPC") { }
            climbableManager.SetUpPlayer(other.gameObject, isDown);
    }

    void OnTriggerExit2D(Collider2D other) {
        if (other.tag == "NPC")
            climbableManager.PlayerOffClimbable(other.gameObject);
    }
    #endregion
}
