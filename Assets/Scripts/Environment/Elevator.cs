using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elevator : MonoBehaviour {
    public GameObject arrow;
    public Elevator[] links;
    public Collider2D bounds;
    public bool locked;
    public float _rayLength = 2f;
    public List<GameObject> _actors = new List<GameObject>();
    public Node node;

    public int levels;

    /*
    private void OnDrawGizmos() {
        if(link != null)
            Gizmos.DrawLine(transform.position, link.transform.position);
    }
    */

    private void Update() {
        if (Input.GetKeyDown(KeyCode.E)) {
            foreach (GameObject actor in _actors) {
                if (actor.layer == 9) {
                    MoveCharacter(actor, 1);
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        DisplayArrow();
        _actors.Add(collision.gameObject);
        //Debug.Log(collision.gameObject.name + " entered " + gameObject.name);
    }

    private void OnTriggerExit2D(Collider2D collision) {
        HideArrow();
        _actors.Remove(collision.gameObject);
        //Debug.Log(collision.gameObject.name + " left " + gameObject.name);
    }

    public void DisplayArrow() {
        arrow.SetActive(true);
    }

    public void HideArrow() {
        arrow.SetActive(false);
    }

    public void MoveCharacter(GameObject character, int level) {
        if (links != null) {
            ShowElevatorGUI();
            //character.transform.position = new Vector2(link.transform.position.x, link.transform.position.y);
        }
        else {
            Debug.Log("Assign a link to this door!");
        }

    }

    // When they interact with the Elevator show the
    // procedural elevator UI, when they select a 
    // level fire off PickLevel(int level) with the
    // supplied level.
    public void ShowElevatorGUI() {

    }

    // Mainly just teleport the character to the level.
    // This entire elevator thing may force me to refactor
    // cabins again as they may need to be able to tell
    // how many levels they have and where a node is /
    // what level it is on so that the pathfinder knows
    // where to go.

    // Maybe instead of redoing all of the Cabin code
    // Add an int to Node that contains the level it is on
    // That is created when the node is created. All nodes
    // check their nearest neighbour in order to determine
    // this if they are not elevators. THINK ABOUT THIS
    // SOME MORE AS THIS MAY NOT BE A GOOD WAY OF DOING THIS
    public void PickLevel(int level) {

    }

    public void Lock(bool state) {
        switch (state) {
            case true:
                locked = true;
                break;
            case false:
                locked = false;
                break;
        }
    }


    IEnumerator DelayCinemachine(float seconds) {
        yield return new WaitForSeconds(seconds);
        CameraController.Instance.EnableConfines(bounds);
    }
}
