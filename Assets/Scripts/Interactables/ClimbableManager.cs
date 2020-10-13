using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClimbableManager : MonoBehaviour
{
    [SerializeField] GameObject startCollider, endCollider;

    [SerializeField] float climbingSpeed = 0.5f;

    //At the Start of the Game send to the Colliders the start and end point of the Climbable
    public void SetUpPlayer(GameObject Player, bool isDown) {
        Debug.Log("Setup Player for " + gameObject.name);
        PlayerController controller = Player.GetComponent<PlayerController>();
        controller.SetClimbableData(true, startCollider.transform.position, endCollider.transform.position, isDown, climbingSpeed);
    }
    public void PlayerOffClimbable(GameObject Player) {
        PlayerController controller = Player.GetComponent<PlayerController>();
        controller.OffClimbable();
    }
}
