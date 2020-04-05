using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public GameObject arrow;
    public GameObject link;
    public Collider2D bounds;
    public float _rayLength = 2f;
    public List<GameObject> _actors = new List<GameObject>();


    private void OnDrawGizmos() {
        if(link != null)
            Gizmos.DrawLine(transform.position, link.transform.position);
    }

    private void Update() {

        

        if (Input.GetKeyDown(KeyCode.E)) {
            foreach(GameObject actor in _actors) {
                if(actor.layer == 9) {
                    MoveCharacter(actor);
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        DisplayArrow();
        _actors.Add(collision.gameObject);
        Debug.Log(collision.gameObject.name + " entered " + gameObject.name);
    }

    private void OnTriggerExit2D(Collider2D collision) {
        HideArrow();
        _actors.Remove(collision.gameObject);
        Debug.Log(collision.gameObject.name + " left " + gameObject.name);
    }

    public void DisplayArrow() {
        arrow.SetActive(true);
    }

    public void HideArrow() {
        arrow.SetActive(false);
    }

    public void MoveCharacter(GameObject character) {
        if(link != null) {
            character.transform.position = new Vector2(link.transform.position.x, character.transform.position.y);
        }
        else {
            Debug.Log("Assign a link to this door!");
        }
        
        //CameraController.Instance.DisableConfines();

        //StartCoroutine(DelayCinemachine(0.08f));

    }




    IEnumerator DelayCinemachine(float seconds) {
        yield return new WaitForSeconds(seconds);
        CameraController.Instance.EnableConfines(bounds);
    }
}
